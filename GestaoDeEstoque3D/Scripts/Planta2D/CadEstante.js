class CadEstante {
    static Abrir(id = null) {
        this.Limpar();

        $('#modal-cadastro-estante').css('display', 'flex');

        if (id != null) {
            $('#cad-estante-header').css('display', 'none');
            $('#aviso-edit-estante').css('display', 'block');
            $('#edit-estante-header').css('display', 'block');

            this.Carregar(id);
        } else {
            $('#edit-estante-header').css('display', 'none');
            $('#aviso-edit-estante').css('display', 'none');
            $('#cad-estante-header').css('display', 'block');
        }
    }

    static Fechar() {
        $('#modal-cadastro-estante').css('display', 'none');
    }

    static Limpar() {
        $('#modal-cadastro-estante input').val('');
    }

    static Carregar(id) {
        var data = ModalLista.Objetos.find(i => i.Id == id);

        $('#cad-estante-id').val(data.Id);
        $('#cad-estante-quant-prateleiras').val(data.QtdPrateleiras);
        $('#cad-estante-largura').val(data.LarguraPrat);
        $('#cad-estante-altura').val(data.AlturaPrat);
        $('#cad-estante-profundidade').val(data.ProfundidadePrat);
        $('#cad-estante-peso-maximo').val(data.PesoMaximoPrat);
    }

    static Finalizar() {
        if ($('#cad-estante-id').val() == '')
            this.Cadastrar();
        else
            this.Editar();
    }

    static Cadastrar() {
        var json = {};

        json.QtdPrateleiras = $('#cad-estante-quant-prateleiras').val();
        json.LarguraPrat = $('#cad-estante-largura').val();
        json.AlturaPrat = $('#cad-estante-altura').val();
        json.ProfundidadePrat = $('#cad-estante-profundidade').val();
        json.PesoMaximoPrat = $('#cad-estante-peso-maximo').val();

        var parametrosAjax = { JsonEstante: JSON.stringify(json) };
        $.ajax({
            type: "POST",
            url: "/Estante/CadastrarEstante",
            data: parametrosAjax,
            success: async function (result) {
                ModalLista.Estantes.Carregar(() => { ModalLista.Filtrar(); });

                CarregarCamadas();
                await PackContainers();

                CadEstante.Fechar();
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }

    static Editar() {
        var json = {};

        json.Id = $('#cad-estante-id').val();
        json.QtdPrateleiras = $('#cad-estante-quant-prateleiras').val();
        json.LarguraPrat = $('#cad-estante-largura').val();
        json.AlturaPrat = $('#cad-estante-altura').val();
        json.ProfundidadePrat = $('#cad-estante-profundidade').val();
        json.PesoMaximoPrat = $('#cad-estante-peso-maximo').val();

        var parametrosAjax = { JsonEstante: JSON.stringify(json) };
        $.ajax({
            type: "POST",
            url: "/Estante/EditarEstante",
            data: parametrosAjax,
            success: async function (result) {
                ModalLista.Estantes.Carregar(() => { ModalLista.Filtrar(); });
                CarregarCamadas();
                await PackContainers();
                CadEstante.Fechar();
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }

    static Deletar = class Deletar {
        static Abrir(id) {
            $('#deletar-estante-id').val(id);

            $('#modal-deletar-estante').css('display', 'flex');
        }

        static Fechar() {
            $('#modal-deletar-estante').css('display', 'none');
        }

        static Finalizar() {
            var parametrosAjax = { EstanteId: $('#deletar-estante-id').val() };
            $.ajax({
                type: "POST",
                url: "/Estante/DeletarEstante",
                data: parametrosAjax,
                success: async function (result) {
                    ModalLista.Estantes.Carregar(() => { ModalLista.Filtrar(); });

                    CarregarCamadas();
                    await PackContainers();

                    CadEstante.Deletar.Fechar();

                    $('#deletar-estante-id').val('');
                },
                error: function (req, status, error) {
                    console.log("Erro.");
                }
            });
        }
    }

    static AdicionarAoMapa(id) {
        var centroMapa = planta.getCenter();
        $.ajax({
            type: "POST",
            url: "/Estante/AdicionarEstanteAoMapa",
            data: { EstanteId: id, Lat: centroMapa.lat, Lng: centroMapa.lng },
            success: function (result) {
                CarregarCamadas();

                ModalLista.Fechar();
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }
}