import json

from src.data.storages.FiresDataInMemoryStorage import FiresDataInMemoryStorage


class FiresDataRepository:

    @staticmethod
    def load_data_to_memory_storage(geojson_path, storage=FiresDataInMemoryStorage()):
        with open(geojson_path, encoding='utf-8') as f:
            geojson_data = json.load(f)
        storage.add_fires_data(geojson_data)
