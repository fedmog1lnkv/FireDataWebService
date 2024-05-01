function parseUrl() {
    const urlParams = new URLSearchParams(window.location.search);

    const dateParam = urlParams.get('date');

    if (dateParam) {
        const formattedDateParam = dateParam.replace(/%20/g, ' ');
        const dates = formattedDateParam.split(' to ');

        flatpickr("#datepicker", {
            mode: "range",
            defaultDate: [dates[0], dates[1]],
            onChange: function (selectedDates, dateStr, instance) {
                fetchData(formattedDateParam);
            }

        });
        fetchData(dateParam);
    } else {
        flatpickr("#datepicker", {
            mode: "range",
            onChange: function (selectedDates, dateStr, instance) {
                fetchData(formattedDateParam);
            }
        });
    }

    const layersParam = urlParams.get('layers');
    if (layersParam) {
        const layers = layersParam.split(',');

        layers.forEach(layer => {
            const capitalizedLayer = capitalizeFirstLetter(layer);
            if (overlayMaps[capitalizedLayer]) {
                overlayMaps[capitalizedLayer].addTo(map);
            }
        });
    }

    setTimeout(() => {
        const baseLayersParam = urlParams.get('base');

        if (baseLayersParam) {
            const baseLayer = capitalizeFirstLetter(baseLayersParam.split(',')[0]);
            const selectedBaseLayer = registeredGeoRasterLayers[baseLayer];

            if (selectedBaseLayer) {
                selectedBaseLayer.addTo(map);
            }
        }
    }, 5000);
}