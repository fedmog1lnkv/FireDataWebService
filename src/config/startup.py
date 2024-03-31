from dotenv import load_dotenv
import os
from src.config.EnvConfig import config_env, ENV_CONF
from src.data.repositories.WeatherDataRepository import WeatherDataRepository
from src.routers import index_router
from src.routers.data import data_router


def startup(app):
    config_env()
    load_data_storages()
    include_routers(app)


def include_routers(app):
    app.register_blueprint(index_router)
    app.register_blueprint(data_router)


def load_data_storages():
    weather_data_list = WeatherDataRepository.load_weather_data_from_csv("data/data/weather_coords.csv")
    WeatherDataRepository.load_data_to_memory_storage(weather_data_list)
