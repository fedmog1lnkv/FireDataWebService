from datetime import datetime


class FiresDataInMemoryStorage:
    _instance = None

    def __new__(cls, *args, **kwargs):
        if not cls._instance:
            cls._instance = super().__new__(cls, *args, **kwargs)
            cls._instance._data_geojson = None
        return cls._instance

    def add_fires_data(self, fires_data):
        self._data_geojson= fires_data

    def get_fires_data_by_date(self, target_date):
        date = datetime.strptime(target_date, '%Y-%m-%d')

        # Фильтрация объектов GeoJSON по заданному году и диапазону дат
        filtered_features = []
        for feature in self._data_geojson['features']:
            dt_start = datetime.strptime(feature['properties']['dt_start'], '%Y-%m-%d %H:%M:%S')
            dt_end = datetime.strptime(feature['properties']['dt_end'], '%Y-%m-%d %H:%M:%S')
            if dt_start <= date <= dt_end:
                filtered_features.append(feature)
        return filtered_features

    def get_all_fires_data(self):
        return self._data_geojson

    def clear(self):
        self._data_geojson = None