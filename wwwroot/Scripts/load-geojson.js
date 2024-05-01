function fetchAndDrawGeoJson(layerName, filepath, style) {
    fetch(filepath)
        .then(response => response.json())
        .then(data => {
            var layer = L.geoJSON(data, {
                style: style // передаем стили как параметр
            });
            layer.eachLayer(function(layer) {
                var name = layer.feature.properties.name;
                layer.bindTooltip(name);
            });
            overlayMaps[layerName] = layer;
            layerControl.addOverlay(layer, layerName); // исправляем ошибку - передаем имя слоя как переменную, а не строку "layerName"
        })
        .catch(error => console.error(`Ошибка загрузки GeoJSON файла с ${layerName}:`, error));
}


fetchAndDrawGeoJson("Regions", "Data/regions.geojson", function(feature) {
    return {
        color: 'green',
        fillColor: 'lightgreen',
        opacity: 0.5
    };
});

fetchAndDrawGeoJson("Rivers", "Data/rivers.geojson", function(feature) {
    return {
        color: 'blue',
        weight: 2
    };
});