var AcoesRow = function (cell, formatterParams, onRendered) {
    return '<div style="display: flex"><div style="color: #219BB8;" onclick="PrepararEditar(' + cell.getData().Id + ')">Editar</div>&nbsp|&nbsp<div style="color: #C73228;" onclick="Deletar(' + cell.getData().Id + ')">Excluir</div></div>';
};

var tabela = new Tabulator("#tabela", {
    columns: [
        { title: "Código", field: "Id" },
        { title: "Nome", field: "Nome" },
        { title: "Descrição", field: "Descricao" },
        { title: "Código de Barras", field: "CodigoDeBarras" },
        { title: "Largura.", field: "Largura" },
        { title: "Altura", field: "Altura" },
        { title: "Profundidade", field: "Profundidade" },
        { title: "Peso", field: "Peso" },
        { title: "Peso Máximo p/ Empilhamento", field: "PesoMaximoEmpilhamento" },
        { title: "Ações", field: "Acoes", formatter: AcoesRow },
    ],
});

$(function () {
    CarregarTabela();
});

function CarregarTabela() {
    $.ajax({
        type: "POST",
        url: "/TipoItemEstoque/RetornarTiposItemEstoque",
        success: function (result) {
            tabela.setData(result);
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function Cadastrar() {
    var json = {};

    json.Nome = $('#nome').val();
    json.Descricao = $('#descricao').val();
    json.Largura = $('#largura').val();
    json.Altura = $('#altura').val();
    json.Profundidade = $('#profundidade').val();
    json.Peso = $('#peso').val();
    json.PesoMaximoEmpilhamento = $('#peso-empilhamento').val();
    json.CodigoDeBarras = $('#codigo-de-barras').val();

    var parametrosAjax = { JsonTipoItemEstoque: JSON.stringify(json) };
    $.ajax({
        type: "POST",
        url: "/TipoItemEstoque/CadastrarTipoItemEstoque",
        data: parametrosAjax,
        success: function (result) {
            CancelarEdicao();
            CarregarTabela();
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function PrepararEditar(id) {
    var tabelaData = tabela.getData();

    var data = tabelaData.find(i => i.Id == id);

    $('#codigo').val(data.Id);
    $('#nome').val(data.Nome);
    $('#descricao').val(data.Descricao);
    $('#largura').val(data.Largura);
    $('#altura').val(data.Altura);
    $('#profundidade').val(data.Profundidade);
    $('#peso').val(data.Peso);
    $('#peso-empilhamento').val(data.PesoMaximoEmpilhamento);
    $('#codigo-de-barras').val(data.CodigoDeBarras);

    $('#codigo-label').css('display', 'block');
    $('#codigo').css('display', 'block');

    $('#button-adicionar').css('display', 'none');
    $('#button-editar').css('display', 'block');
    $('#button-cancelar').css('display', 'block');
}

function Editar() {
    var json = {};

    json.Id = $('#codigo').val();
    json.Nome = $('#nome').val();
    json.Descricao = $('#descricao').val();
    json.Largura = $('#largura').val();
    json.Altura = $('#altura').val();
    json.Profundidade = $('#profundidade').val();
    json.Peso = $('#peso').val();
    json.PesoMaximoEmpilhamento = $('#peso-empilhamento').val();
    json.CodigoDeBarras = $('#codigo-de-barras').val();

    var parametrosAjax = { JsonTipoItemEstoque: JSON.stringify(json) };
    $.ajax({
        type: "POST",
        url: "/TipoItemEstoque/EditarTipoItemEstoque",
        data: parametrosAjax,
        success: function (result) {
            CancelarEdicao();
            CarregarTabela();
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function CancelarEdicao() {
    $('#codigo').val('');
    $('#nome').val('');
    $('#descricao').val('');
    $('#largura').val('');
    $('#altura').val('');
    $('#profundidade').val('');
    $('#peso').val('');
    $('#peso-empilhamento').val('');
    $('#codigo-de-barras').val('');

    $('#codigo-label').css('display', 'none');
    $('#codigo').css('display', 'none');

    $('#button-adicionar').css('display', 'block');
    $('#button-editar').css('display', 'none');
    $('#button-cancelar').css('display', 'none');
}

function Deletar(id) {
    var parametrosAjax = { TipoItemEstoqueId: id };
    $.ajax({
        type: "POST",
        url: "/TipoItemEstoque/DeletarTipoItemEstoque",
        data: parametrosAjax,
        success: function (result) {
            CarregarTabela();
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}