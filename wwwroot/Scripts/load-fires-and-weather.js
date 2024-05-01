var weatherIcon = L.icon({
    iconUrl: 'Icons/weather.png',
    iconSize: [32, 37],
    iconAnchor: [15, 34],
    popupAnchor: [15, 3]
});

function fetchWeatherData(startDate, endDate) {
    var url = '/weather/geojson?requestedDate=';

    if (!endDate) {
        url += startDate;
    } else {
        url += startDate + '&endDate=' + endDate;
    }

    fetch(url)
        .then(response => response.json())
        .then(data => {

            var totalTemperature = 0;
            var totalPrecipitation = 0;
            var stationsCount = 0;

            data.features.forEach(station => {
                totalTemperature += station.properties.T || 0;
                totalPrecipitation += station.properties.RRR || 0;
                stationsCount++;
            });

            var avgTemperature = totalTemperature / stationsCount;
            var avgPrecipitation = totalPrecipitation;

            // Обновляем содержимое элементов таблицы
            document.getElementById('avgTemperature').textContent = avgTemperature.toFixed(2);
            document.getElementById('precipitationCount').textContent = avgPrecipitation.toFixed(2);

            L.geoJSON(data, {
                style: function (feature) {
                    return {
                        color: 'blue',
                        fillColor: 'lightblue'
                    };
                },
                pointToLayer: function (feature, latlng) {
                    var marker = L.marker(latlng, {
                        icon: weatherIcon
                    }).on('click', function (e) {
                        var popupContent = '<b>Id метеостанции:</b> ' + feature.properties.weather_station_id + '<br>' +
                            '<b>Средняя температура:</b> ' + feature.properties.T + '<br>' +
                            '<b>Средняя скорость ветра:</b> ' + feature.properties.Ff + '<br>' +
                            '<b>Среднее атмосферное давление:</b> ' + feature.properties.P + '<br>' +
                            '<b>Средняя влажность воздуха:</b> ' + feature.properties.U + '<br>' +
                            '<b>Скорость ветра:</b> ' + feature.properties.V + '<br>' +
                            '<b>Количество осадков:</b> ' + feature.properties.RRR + '<br>' +
                            '<b>Тип погодного явления:</b> ' + feature.properties.WW_type;

                        L.popup()
                            .setLatLng(latlng)
                            .setContent(popupContent)
                            .openOn(map);
                    });

                    return marker;
                }
            }).addTo(weatherLayerGroup);

            var weatherTypesCount = {};

            data.features.forEach(feature => {
                if (feature.properties.WW_type) {
                    var types = feature.properties.WW_type.split(',');

                    types.forEach(type => {
                        type = type.trim();

                        if (weatherTypesCount[type]) {
                            weatherTypesCount[type]++;
                        } else {
                            weatherTypesCount[type] = 1;
                        }
                    });
                }
            });

            var weatherTypesData = [];
            for (var type in weatherTypesCount) {
                weatherTypesData.push({
                    label: type,
                    value: weatherTypesCount[type]
                });
            }

            renderPieChart(weatherTypesData);

        })
        .catch(error => console.error('Ошибка при получении данных о погоде:', error));
}

var fireIcon = L.icon({
    iconUrl: 'Icons/fire.png',
    iconSize: [32, 37],
    iconAnchor: [15, 34],
    popupAnchor: [15, 3]
});

function fetchFireData(startDate, endDate) {
    var url = '/fires/geojson?requestedDate=';

    if (!endDate) {
        url += startDate;
    } else {
        url += startDate + '&endDate=' + endDate;
    }
    fetch(url)
        .then(response => response.json())
        .then(data => {

            document.getElementById('fireCount').innerText = data.features.length; // Количество пожаров

            var totalArea = data.features.reduce((acc, cur) => acc + cur.properties.area, 0);
            document.getElementById('fireArea').innerText = totalArea.toFixed(2); // Общая площадь пожаров

            L.geoJSON(data, {
                style: function (feature) {
                    return {
                        fillColor: 'red',
                        fillOpacity: 0.5,
                        color: 'red'
                    };
                },
                pointToLayer: function (feature, latlng) {
                    var marker = L.marker(latlng, {
                        icon: fireIcon
                    }).on('click', function (e) {
                        var popupContent = '<b>Id пожара:</b> ' + feature.properties.id + '<br>' +
                            '<b>Площадь пожара:</b> ' + feature.properties.area + '<br>' +
                            '<b>Дата начала пожара:</b> ' + feature.properties.dt_start + '<br>' +
                            '<b>Дата конца пожара:</b> ' + feature.properties.dt_end + '<br>' +
                            '<b>Длительность:</b> ' + feature.properties.duration + '<br>' +
                            '<b>Тип пожара:</b> ' + feature.properties.type;

                        // Создаем новый попап и открываем его
                        L.popup()
                            .setLatLng(latlng)
                            .setContent(popupContent)
                            .openOn(map);
                    });
                    return marker;
                }
            }).addTo(fireLayerGroup);
        })
        .catch(error => console.error('Ошибка при получении данных о пожарах:', error));
}