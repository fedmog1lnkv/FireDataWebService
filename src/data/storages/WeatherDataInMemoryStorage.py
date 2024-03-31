class WeatherDataInMemoryStorage:
    _instance = None

    def __new__(cls, *args, **kwargs):
        if not cls._instance:
            cls._instance = super().__new__(cls, *args, **kwargs)
            cls._instance._data = []
        return cls._instance

    def add_weather_data(self, weather_data):
        self._data.append(weather_data)

    def get_all_weather_data(self):
        return self._data

    def clear(self):
        self._data = []