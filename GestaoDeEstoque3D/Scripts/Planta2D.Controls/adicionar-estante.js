$('#btnAdicionarEstante').on('click', function (e) {
    $('#ModalAdicionarEstante').css('display', 'none');
});

function CriarControlAdicionarEstante(classeA = 'toolbar-a') {
    var container = L.DomUtil.create('div');
    container.classList.add("button-container");

    var a = L.DomUtil.create('a');
    a.classList.add("leaflet-buttons-control-button", classeA);
    a.innerHTML = '<i class="far fa-plus-square control-icon" title="Adicionar Estante"></i>';

    container.append(a);

    container.onclick = function () {
        $('#ModalAdicionarEstante').css('display', 'flex');
        CarregarTabelaEstantes();
        tabelaEstantes.redraw(true);
    }

    return container;
}

var AcoesRow = function (cell, formatterParams, onRendered) {
    return '<div style="display: flex"><div style="color: #219BB8;" onclick="AdicionarEstanteAoMapa(' + cell.getData().Id + ')">Adicionar</div></div>';
};

var tabelaEstantes = new Tabulator("#tabela-adicionar-estante", {
    columns: [
        { title: "Código", field: "Id" },
        { title: "Qtd. de Prateleiras", field: "QtdPrateleiras" },
        { title: "Largura das Prat.", field: "LarguraPrat" },
        { title: "Altura das Prat.", field: "AlturaPrat" },
        { title: "Profundidade das Prat.", field: "ProfundidadePrat" },
        { title: "Peso Máximo das Prat.", field: "PesoMaximoPrat" },
        { title: "Ações", field: "Acoes", formatter: AcoesRow },
    ],
});

function CarregarTabelaEstantes() {
    $.ajax({
        type: "POST",
        url: "/Estante/RetornarEstantesNaoPresentesNoMapa",
        success: function (result) {
            tabelaEstantes.setData(result);
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function AdicionarEstanteAoMapa(EstanteId) {
    $.ajax({
        type: "POST",
        url: "/Estante/AdicionarEstanteAoMapa",
        data: { EstanteId },
        success: function (result) {
            CarregarCamadas();

            $('#ModalAdicionarEstante').css('display', 'none');
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}