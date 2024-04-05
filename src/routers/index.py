from flask import Blueprint, request, render_template
from folium import folium, Marker

from src.data.storages.WeatherDataInMemoryStorage import WeatherDataInMemoryStorage

index_router = Blueprint('index', __name__)


@index_router.route('/')
def index():
    return render_template('index.html')


@index_router.route('/generate_map', methods=['GET', 'POST'])
def generate_map():
    if request.method == 'POST':
        target_date = request.form['date']

        storage = WeatherDataInMemoryStorage()
        parsed_data = storage.get_weather_data_by_date(target_date)

        m = folium.Map(location=[52.17, 104.18], zoom_start=5)
        for weather_data in parsed_data:
            if weather_data.WW == " " or weather_data.WW_type == "":
                continue
            Marker(
                location=[weather_data.lat, weather_data.lon],
                popup=f'Date: {target_date}\nInfo: {weather_data.WW}\nWeather: {weather_data.WW_type}'
            ).add_to(m)

        map_html = m.get_root().render()
        print("map готова")
        return map_html
    else:
        m = folium.Map(location=[67.214, 153.327], zoom_start=5)
        map_html = m.get_root().render()
        return map_html