import json

from flask import Blueprint, request, render_template
from folium import Map, Marker, GeoJson, folium

from src.data.storages.WeatherDataInMemoryStorage import WeatherDataInMemoryStorage

index_router = Blueprint('index', __name__)


@index_router.route('/')
def index():
    return render_template('index.html')


@index_router.route('/generate_map', methods=['POST'])
def generate_map():
    target_date = request.form['date']
    selected_options = request.form.getlist('options[]')

    storage = WeatherDataInMemoryStorage()
    parsed_data = storage.get_weather_data_by_date(target_date)

    m = Map(location=[52.17, 104.18], zoom_start=5)

    # Add rivers from rivers.geojson if "rivers" option is selected
    if 'rivers' in selected_options:
        rivers_data = 'data/data/rivers.geojson'
        with open(rivers_data, encoding='utf-8') as f:
            rivers_geojson = json.load(f)

        GeoJson(
            rivers_geojson,
            name='geojson'
        ).add_to(m)

    if 'regions' in selected_options:
        regions_data = 'data/data/MO_Irk_region_32648.geojson'
        with open(regions_data, encoding='utf-8') as f:
            regions_geojson = json.load(f)

        GeoJson(
            regions_geojson,
            name='geojson'
        ).add_to(m)

    for weather_data in parsed_data:
        if weather_data.WW == " " or weather_data.WW_type == "":
            continue
        if 'rivers' in selected_options and weather_data.type == 'river':
            Marker(
                location=[weather_data.lat, weather_data.lon],
                popup=f'Date: {target_date}\nInfo: {weather_data.WW}\nWeather: {weather_data.WW_type}'
            ).add_to(m)

    map_html = m.get_root().render()
    print("map готова")
    return map_html
