from datetime import datetime
import json

from flask import Blueprint, request, render_template, redirect
from folium import Map, Marker, GeoJson, folium, Icon
from shapely import wkt

from src.data.storages.FiresDataInMemoryStorage import FiresDataInMemoryStorage
from src.data.storages.WeatherDataInMemoryStorage import WeatherDataInMemoryStorage

index_router = Blueprint('index', __name__)


@index_router.route('/')
def index():
    date = request.args.get('date')
    options = request.args.getlist('options[]')
    return render_template('index.html', date=date, options=options)


@index_router.route('/generate_map', methods=['POST'])
def generate_map():
    target_date = request.form['date']
    selected_options = request.form.getlist('options[]')

    m = Map(location=[52.17, 104.18], zoom_start=5)
    print("fires")
    add_fires_to_map(m, target_date)
    # Add rivers from rivers.geojson if "rivers" option is selected
    if 'rivers' in selected_options:
        print("rivers")
        add_rivers_to_map(m)

    if 'regions' in selected_options:
        regions_data = 'data/data/MO_Irk_region_4326.geojson'
        with open(regions_data, encoding='utf-8') as f:
            regions_geojson = json.load(f)

        GeoJson(
            regions_geojson,
            name='geojson'
        ).add_to(m)

    if 'weather' in selected_options:
        print("weather")
        add_weather_to_map(m, target_date)

    map_html = m.get_root().render()
    print("map готова")
    return map_html


def add_fires_to_map(m, target_date, storage=FiresDataInMemoryStorage()):
    print("fires target_date", target_date)

    # Фильтрация объектов GeoJSON по заданному году и диапазону дат
    filtered_features = storage.get_fires_data_by_date(target_date)

    # Добавление геоданных областей на карту
    GeoJson(
        {'type': 'FeatureCollection', 'features': filtered_features},
        name='Regions'
    ).add_to(m)

    # Отображение маркеров для центроидов каждой области
    print(f"fires count {len(filtered_features)}")
    for feature in filtered_features:
        centroid_str = feature['properties']['centroid']
        centroid_point = wkt.loads(centroid_str)
        centroid_lat_lon = centroid_point.y, centroid_point.x  # получаем координаты в формате (lat, lon)
        new_fire_id_unique = feature['properties']['new_fire_id_unique']
        area = feature['properties']['area']
        dt_start = feature['properties']['dt_start']
        dt_end = feature['properties']['dt_end']

        # Создание текста для всплывающей подсказки маркера
        popup_text = f"Fire ID: {new_fire_id_unique}<br>" \
                     f"Area: {area}<br>" \
                     f"Start date: {dt_start}<br>" \
                     f"End date: {dt_end}"

        # Создание кастомного иконки маркера
        custom_icon = Icon(icon='fire', prefix='fa', color='red')

        # Добавление маркера на карту
        Marker(location=centroid_lat_lon, popup=popup_text, icon=custom_icon).add_to(m)


def add_rivers_to_map(m):
    rivers_data = 'data/data/rivers.geojson'
    with open(rivers_data, encoding='utf-8') as f:
        rivers_geojson = json.load(f)

    GeoJson(
        rivers_geojson,
        name='geojson'
    ).add_to(m)


def add_weather_to_map(m, target_date):
    storage = WeatherDataInMemoryStorage()
    parsed_data = storage.get_weather_data_by_date(target_date)
    for weather_data in parsed_data:
        if weather_data.WW == " " or weather_data.WW_type == "":
            continue
        custom_icon = Icon(icon='weather', prefix='fa', color='blue')
        Marker(
            location=[weather_data.lat, weather_data.lon],
            popup=f'Date: {target_date}\nInfo: {weather_data.WW}\nWeather: {weather_data.WW_type}', icon=custom_icon
        ).add_to(m)
