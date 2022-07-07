class Ancoragem {
    static EstanteAncoragem = null;
    static BalcaoAncoragem = null;
    static DefinindoAncoragem = false;

    static Iniciar() {
        planta.closePopup()

        $('#snackbar-ancoragem').css('visibility', 'visible');

        this.DefinindoAncoragem = true;

        if (this.EstanteAncoragem != null)
            this.EstanteAncoragem.setStyle({ fillOpacity: 0.6 });
        else
            this.BalcaoAncoragem.setStyle({ fillOpacity: 0.6 });
    }

    static DefinirPonto(latlng, callback = () => { }) {
        var estante = { Id: -1 };

        if (this.EstanteAncoragem != null)
            estante = estantesAssociadas.find(i => i.PoligonoId == this.EstanteAncoragem.feature.properties.PoligonoId);

        var parametrosAjax = { EstanteId: estante.Id, Lat: latlng.lat, Lng: latlng.lng };
        $.ajax({
            type: "POST",
            url: "/Estante/DefinirPontoDeAncoragem",
            data: parametrosAjax,
            success: async function (result) {
                callback();
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }

    static Finalizar() {
        $('#snackbar-ancoragem').css('visibility', 'hidden');

        if (this.EstanteAncoragem != null)
            this.EstanteAncoragem.setStyle({ fillOpacity: 0.2 });
        else
            this.BalcaoAncoragem.setStyle({ fillOpacity: 0.2 });

        this.EstanteAncoragem = null;
        this.BalcaoAncoragem = null;

        this.DefinindoAncoragem = false;
    }
}