import csv

import redis

from src.data.mappers.WeatherDataMapper import weather_data_model_from_dict, weather_data_model_to_dict
from src.data.storages.WeatherDataInMemoryStorage import WeatherDataInMemoryStorage
from src.domain.models.WeatherDataModel import WeatherDataModel


class WeatherDataRepository:

    @staticmethod
    def load_weather_data_from_csv(csv_file):
        weather_data_list = []
        with open(csv_file, newline='', encoding='utf-8') as csvfile:
            csv_reader = csv.DictReader(csvfile)
            for row in csv_reader:
                weather_data = WeatherDataModel(
                    weather_station_id=int(row['weather_station_id']),
                    date=row['date'],
                    WW=row['WW'],
                    T=float(row['T']) if row['T'] else None,
                    Ff=float(row['Ff']) if row['Ff'] else None,
                    P=float(row['P']) if row['P'] else None,
                    U=float(row['U']) if row['U'] else None,
                    VV=float(row['VV']) if row['VV'] else None,
                    Td=float(row['Td']) if row['Td'] else None,
                    RRR=float(row['RRR']) if row['RRR'] else None,
                    WW_code=int(row['WW_code']),
                    lon=float(row['lon']),
                    lat=float(row['lat']),
                    WW_type=row['WW_type']
                )
                weather_data_list.append(weather_data)
        return weather_data_list

    @staticmethod
    def load_data_to_memory_storage(weather_data_list, storage=WeatherDataInMemoryStorage()):
        for weather_data in weather_data_list:
            storage.add_weather_data(weather_data)
