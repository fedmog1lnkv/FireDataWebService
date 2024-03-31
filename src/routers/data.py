from flask import Blueprint, request
from src.data.storages.WeatherDataInMemoryStorage import WeatherDataInMemoryStorage

data_router = Blueprint('data', __name__)


@data_router.route('/weather-data')
def table():
    # Получаем номер текущей страницы из запроса, либо используем 1, если параметр не указан
    page = int(request.args.get('page', 1))
    table_html = generate_weather_table(page)
    return f"""
    <!DOCTYPE html>
    <html>
    <head>
        <title>Weather Data</title>
        <style>
            table {{
                font-family: Arial, sans-serif;
                border-collapse: collapse;
                width: 100%;
            }}
            th, td {{
                border: 1px solid #dddddd;
                text-align: left;
                padding: 8px;
            }}
            tr:nth-child(even) {{
                background-color: #f2f2f2;
            }}
            .pagination {{
                margin-top: 10px;
            }}
            .pagination a {{
                text-decoration: none;
                padding: 5px 10px;
                border: 1px solid #dddddd;
                margin-right: 5px;
            }}
            .pagination span {{
                padding: 5px 10px;
                border: 1px solid #dddddd;
                margin-right: 5px;
                background-color: #f2f2f2;
            }}
        </style>
    </head>
    <body>
        <h1>Weather Data</h1>
        {table_html}
    </body>
    </html>
    """


def generate_weather_table(page=1, items_per_page=10):
    # Получаем все данные о погоде из хранилища
    weather_data_storage = WeatherDataInMemoryStorage()
    all_weather_data = weather_data_storage.get_all_weather_data()

    # Рассчитываем индекс начала и конца элементов для текущей страницы
    start_index = (page - 1) * items_per_page
    end_index = min(start_index + items_per_page, len(all_weather_data))

    # Фильтруем данные для текущей страницы
    current_page_data = all_weather_data[start_index:end_index]

    # Генерируем HTML-разметку для таблицы
    table_html = "<table>"
    table_html += "<tr><th>Weather Station ID</th><th>Date</th><th>WW</th><th>T</th><th>Ff</th><th>P</th><th>U</th><th>VV</th><th>Td</th><th>RRR</th><th>WW_code</th><th>lon</th><th>lat</th><th>WW_type</th></tr>"
    for data in current_page_data:
        table_html += f"<tr><td>{data.weather_station_id}</td><td>{data.date}</td><td>{data.WW}</td><td>{data.T}</td><td>{data.Ff}</td><td>{data.P}</td><td>{data.U}</td><td>{data.VV}</td><td>{data.Td}</td><td>{data.RRR}</td><td>{data.WW_code}</td><td>{data.lon}</td><td>{data.lat}</td><td>{data.WW_type}</td></tr>"
    table_html += "</table>"

    # Генерируем HTML-разметку для пагинации
    num_pages = (len(all_weather_data) + items_per_page - 1) // items_per_page
    pagination_html = "<div>Pages:"
    for p in range(1, num_pages + 1):
        if p == page:
            pagination_html += f" <span>{p}</span>"
        else:
            pagination_html += f" <a href='?page={p}'>{p}</a>"
    pagination_html += "</div>"

    return table_html + pagination_html
