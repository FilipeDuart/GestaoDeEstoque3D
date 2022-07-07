function CriarToolbar(containers) {
    L.Control.Toolbar = L.Control.extend({
        onAdd: function (map) {
            return this.createContent(containers);
        },

        onRemove: function (map) {
            // Nothing to do here
        },

        createContent: function (_containers) {
            var container = L.DomUtil.create('div');

            container.classList.add("leaflet-bar");

            for (var innerContainer of _containers) {
                container.append(innerContainer);
            }

            L.DomEvent.disableClickPropagation(container);

            return container;
        }
    });

    L.control.Toolbar = function (opts) {
        return new L.Control.Toolbar(opts);
    }

    L.control.Toolbar({ position: 'topleft' }).addTo(planta);
};