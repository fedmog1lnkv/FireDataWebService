from src.domain.models.WeatherDataModel import WeatherDataModel

def weather_data_model_from_dict(data):
    return WeatherDataModel(
        weather_station_id=int(data['weather_station_id']),
        date=data['date'],
        WW=data['WW'],
        T=float(data['T']),
        Ff=float(data['Ff']),
        P=float(data['P']),
        U=float(data['U']),
        VV=float(data['VV']),
        Td=float(data['Td']),
        RRR=float(data['RRR']),
        WW_code=int(data['WW_code']),
        lon=float(data['lon']),
        lat=float(data['lat']),
        WW_type=data['WW_type']
    )

def weather_data_model_to_dict(weather_data_model):
    return {
        'weather_station_id': weather_data_model.weather_station_id,
        'date': weather_data_model.date,
        'WW': weather_data_model.WW,
        'T': weather_data_model.T,
        'Ff': weather_data_model.Ff,
        'P': weather_data_model.P,
        'U': weather_data_model.U,
        'VV': weather_data_model.VV,
        'Td': weather_data_model.Td,
        'RRR': weather_data_model.RRR,
        'WW_code': weather_data_model.WW_code,
        'lon': weather_data_model.lon,
        'lat': weather_data_model.lat,
        'WW_type': weather_data_model.WW_type
    }