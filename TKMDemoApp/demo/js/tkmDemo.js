$(document).ready(function () {
    setTime();
    createMap();
    createPlot();
}
);

function setTime() {
    $("#acqTime").text(acqTime);
    $("#trafficIndex").text(trafficIndex);
};

function createPlot() {
    new Dygraph(
        document.getElementById("fsmGirisAvAn"),
        "data/trafficIndex.csv",
        {
            'Anlık Değer': {
                strokeWidth: 0.0,
                drawPoints: true,
                pointSize: 6,
                highlightCircleSize: 8
            },
            rollPeriod: 0,
            showRoller: false,
            title: 'Ortalama Trafik Yoğunluğu',
            ylabel: 'Yoğunluk',
            legend: 'always',
            labelsDivStyles: { 'textAlign': 'left' },
            showRangeSelector: true
        }
    );
};

function createMap() {
    var layer = L.tileLayer('http://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors, &copy; <a href="http://cartodb.com/attributions">CartoDB</a>'
    });

    var map = L.map('map', {
        center: [41.068436, 29.049846],
        zoom: 13
    });

    map.addLayer(layer);

//    map.locate({ setView: true, maxZoom: 16 });

    L.geoJson(mapData, {
        style: function (feature) {
            var d = feature.properties.s;
            return { color: getColor(d), weight: 4 };
        },
        onEachFeature: onEachFeature
    }).addTo(map);

    // LEGEND
    var legend = L.control({ position: 'bottomright' });
    legend.onAdd = function (map) {
        var div = L.DomUtil.create('div', 'info legend'),
            grades = [0, 20, 40, 60, 100],
            labels = [];

        // loop through our density intervals and generate a label with a colored square for each interval
        for (var i = 0; i < grades.length; i++) {
            div.innerHTML +=
                '<i style="background:' + getColor(grades[i] + 1) + '"></i> ' +
                grades[i] + (grades[i + 1] ? '&ndash;' + grades[i + 1] + ' km/h <br>' : '+ km/h');
        }

        return div;
    };
    legend.addTo(map);
};


function onEachFeature(feature, layer) {
    // does this feature have a property named popupContent?
    if (feature.properties && feature.properties.s)
    {
        if(feature.properties.s!=-1)
            layer.bindPopup("Segment: " + feature.properties.id + "<br/><strong>" + feature.properties.s + " km/h</strong>");
        else
            layer.bindPopup("Segment: " + feature.properties.id + "<br/>" + "<strong>Veri yok.</strong>");
    }
}

function getColor(d)
{
    return d > 100 ? '#76ff03':
        d > 60  ? '#5caf44':
        d > 40  ? '#ff9700':
        d > 20  ? '#FF0000':
        d > 0   ? '#8a0000':
        '#cccccc';
}