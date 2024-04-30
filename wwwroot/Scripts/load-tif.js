function fetchAndAddGeorasterLayer(layerName, filepath) {
    fetch(filepath)
        .then(response => response.arrayBuffer())
        .then(arrayBuffer => {
            parseGeoraster(arrayBuffer).then(georaster => {
                var layer = new GeoRasterLayer({
                    georaster: georaster,
                    opacity: 0.7,
                    resolution: 256
                });
                layerControl.addBaseLayer(layer, layerName);
            });
        })
        .catch(error => {
            console.error(`Ошибка загрузки GeoTIFF для ${layerName}:`, error);
        });
}

var slopeFile = "Data/topographic/slope.tif";
fetchAndAddGeorasterLayer("Slope", slopeFile);

var elevationFile = "Data/topographic/elevation.tif";
fetchAndAddGeorasterLayer("Elevation", elevationFile);

var aspectFile = "Data/topographic/aspect.tif";
fetchAndAddGeorasterLayer("Aspect", aspectFile);
