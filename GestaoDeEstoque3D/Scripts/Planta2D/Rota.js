var PathFinder = require('geojson-path-finder');

var rota;

//geojson = Array de linestring
function InicializarRota(geojson) {
    var FeatureCollection = {
        type: "FeatureCollection",
        features: geojson
    };

    rota = new PathFinder(FeatureCollection, { precision: 1e-1 });
}

//start and finish are two GeoJSON point features
function GerarRota(start, finish) {
    var resultado = rota.findPath(start, finish);
    var polyline = null;

    if (resultado != null) {
        var latlngs = [];

        for (var i = 0; i < resultado.path.length; i++) {
            latlngs.push([ resultado.path[i][1], resultado.path[i][0] ]);
        }

        polyline = L.polyline(latlngs, { color: 'red' });
    }

    return polyline;
}

module.exports = {
    Inicializar: InicializarRota,
    Gerar: GerarRota
};