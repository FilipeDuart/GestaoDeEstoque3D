var AcoesRow = function (cell, formatterParams, onRendered) {
    return '<div style="display: flex"><div style="color: #219BB8;" onclick="PrepararEditarEstante(' + cell.getData().Id + ')">Editar</div>&nbsp|&nbsp<div style="color: #C73228;" onclick="DeletarEstante(' + cell.getData().Id + ')">Excluir</div></div>';
};

var tabelaEstantes = new Tabulator("#tabela-estantes", {
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

$(function () {
    CarregarTabelaEstantes();
});

function CarregarTabelaEstantes() {
    $.ajax({
        type: "POST",
        url: "/Estante/RetornarEstantes",
        success: function (result) {
            tabelaEstantes.setData(result);
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function CadastrarEstante() {
    var json = {};

    json.QtdPrateleiras = $('#estante-quantidade-prateleiras').val();
    json.LarguraPrat = $('#estante-largura-prateleiras').val();
    json.AlturaPrat = $('#estante-altura-prateleiras').val();
    json.ProfundidadePrat = $('#estante-profundidade-prateleiras').val();
    json.PesoMaximoPrat = $('#estante-peso-maximo-prateleiras').val();

    var parametrosAjax = { JsonEstante: JSON.stringify(json) };
    $.ajax({
        type: "POST",
        url: "/Estante/CadastrarEstante",
        data: parametrosAjax,
        success: function (result) {
            CancelarEdicao();
            CarregarTabelaEstantes();
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function PrepararEditarEstante(id) {
    var tabelaData = tabelaEstantes.getData();

    var data = tabelaData.find(i => i.Id == id);

    $('#estante-codigo').val(data.Id);
    $('#estante-quantidade-prateleiras').val(data.QtdPrateleiras);
    $('#estante-largura-prateleiras').val(data.LarguraPrat);
    $('#estante-altura-prateleiras').val(data.AlturaPrat);
    $('#estante-profundidade-prateleiras').val(data.ProfundidadePrat);
    $('#estante-peso-maximo-prateleiras').val(data.PesoMaximoPrat);

    $('#estante-codigo-label').css('display', 'block');
    $('#estante-codigo').css('display', 'block');

    $('#button-adicionar-estante').css('display', 'none');
    $('#button-editar-estante').css('display', 'block');
    $('#button-cancelar').css('display', 'block');
}

function EditarEstante() {
    var json = {};

    json.Id = $('#estante-codigo').val();
    json.QtdPrateleiras = $('#estante-quantidade-prateleiras').val();
    json.LarguraPrat = $('#estante-largura-prateleiras').val();
    json.AlturaPrat = $('#estante-altura-prateleiras').val();
    json.ProfundidadePrat = $('#estante-profundidade-prateleiras').val();
    json.PesoMaximoPrat = $('#estante-peso-maximo-prateleiras').val();

    var parametrosAjax = { JsonEstante: JSON.stringify(json) };
    $.ajax({
        type: "POST",
        url: "/Estante/EditarEstante",
        data: parametrosAjax,
        success: function (result) {
            CancelarEdicao();
            CarregarTabelaEstantes();
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function CancelarEdicao() {
    $('#estante-codigo').val('');
    $('#estante-quantidade-prateleiras').val('');
    $('#estante-largura-prateleiras').val('');
    $('#estante-altura-prateleiras').val('');
    $('#estante-profundidade-prateleiras').val('');
    $('#estante-peso-maximo-prateleiras').val('');

    $('#estante-codigo-label').css('display', 'none');
    $('#estante-codigo').css('display', 'none');

    $('#button-adicionar-estante').css('display', 'block');
    $('#button-editar-estante').css('display', 'none');
    $('#button-cancelar').css('display', 'none');
}

function DeletarEstante(id) {
    var parametrosAjax = { EstanteId: id };
    $.ajax({
        type: "POST",
        url: "/Estante/DeletarEstante",
        data: parametrosAjax,
        success: function (result) {
            CarregarTabelaEstantes();
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}