from dataclasses import dataclass

@dataclass
class WeatherDataModel:
    weather_station_id: int
    date: str
    WW: str
    T: float
    Ff: float
    P: float
    U: float
    VV: float
    Td: float
    RRR: float
    WW_code: int
    lon: float
    lat: float
    WW_type: str
