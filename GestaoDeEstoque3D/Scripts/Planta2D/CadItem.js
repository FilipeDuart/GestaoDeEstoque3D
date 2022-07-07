class CadItem {
    static Abrir(id = null) {
        this.Limpar();

        $('#modal-cadastro-item').css('display', 'flex');

        if (id != null) {
            $('#cad-item-header').css('display', 'none');
            $('#edit-item-header').css('display', 'block');

            this.Carregar(id);
        } else {
            $('#edit-item-header').css('display', 'none');            
            $('#cad-item-header').css('display', 'block');
        }
    }

    static Fechar() {
        $('#modal-cadastro-item').css('display', 'none');
    }

    static Limpar() {
        $('#modal-cadastro-item input').val('');
    }

    static Carregar(id) {
        var data = ModalLista.Objetos.find(i => i.Id == id);

        $('#cad-item-id').val(data.Id);
        $('#cad-item-nome').val(data.Nome);
        $('#cad-item-descricao').val(data.Descricao);
        $('#cad-item-largura').val(data.Largura);
        $('#cad-item-altura').val(data.Altura);
        $('#cad-item-profundidade').val(data.Profundidade);
        $('#cad-item-peso').val(data.Peso);
        $('#cad-item-peso-maximo').val(data.PesoMaximoEmpilhamento);
        $('#cad-item-codigo-barras').val(data.CodigoDeBarras);
    }

    static Finalizar() {
        if ($('#cad-item-id').val() == '')
            this.Cadastrar();
        else
            this.Editar();
    }

    static Cadastrar() {
        var json = {};

        json.Nome = $('#cad-item-nome').val();
        json.Descricao = $('#cad-item-descricao').val();
        json.Largura = $('#cad-item-largura').val();
        json.Altura = $('#cad-item-altura').val();
        json.Profundidade = $('#cad-item-profundidade').val();
        json.Peso = $('#cad-item-peso').val();
        json.PesoMaximoEmpilhamento = $('#cad-item-peso-maximo').val();
        json.CodigoDeBarras = $('#cad-item-codigo-barras').val();

        var parametrosAjax = { JsonTipoItemEstoque: JSON.stringify(json) };
        $.ajax({
            type: "POST",
            url: "/TipoItemEstoque/CadastrarTipoItemEstoque",
            data: parametrosAjax,
            success: function (result) {
                ModalLista.ItensDeEstoque.Carregar(() => { ModalLista.Filtrar(); });
                CadItem.Fechar();
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }

    static Editar() {
        var json = {};

        json.Id = $('#cad-item-id').val();
        json.Nome = $('#cad-item-nome').val();
        json.Descricao = $('#cad-item-descricao').val();
        json.Largura = $('#cad-item-largura').val();
        json.Altura = $('#cad-item-altura').val();
        json.Profundidade = $('#cad-item-profundidade').val();
        json.Peso = $('#cad-item-peso').val();
        json.PesoMaximoEmpilhamento = $('#cad-item-peso-maximo').val();
        json.CodigoDeBarras = $('#cad-item-codigo-barras').val();

        var parametrosAjax = { JsonTipoItemEstoque: JSON.stringify(json) };
        $.ajax({
            type: "POST",
            url: "/TipoItemEstoque/EditarTipoItemEstoque",
            data: parametrosAjax,
            success: function (result) {
                ModalLista.ItensDeEstoque.Carregar(() => { ModalLista.Filtrar(); });
                CadItem.Fechar();
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }

    static Deletar = class Deletar {
        static Abrir(id) {
            $('#deletar-item-id').val(id);

            $('#modal-deletar-item').css('display', 'flex');
        }

        static Fechar() {
            $('#modal-deletar-item').css('display', 'none');
        }

        static Finalizar() {
            var parametrosAjax = { TipoItemEstoqueId: $('#deletar-item-id').val() };
            $.ajax({
                type: "POST",
                url: "/TipoItemEstoque/DeletarTipoItemEstoque",
                data: parametrosAjax,
                success: function (result) {
                    ModalLista.ItensDeEstoque.Carregar(() => { ModalLista.Filtrar(); });
                    CadItem.Deletar.Fechar();

                    $('#deletar-item-id').val('');
                },
                error: function (req, status, error) {
                    console.log("Erro.");
                }
            });
        }
    }

    static Estocar(TipoItemId, ItemId = null) {
        $.ajax({
            type: "POST",
            url: "/ControleDeEstoque/EstocarItem",
            data: { TipoItemEstoqueId: TipoItemId, ItemEstoqueId: ItemId },
            success: async function (result) {
                if (result.NovoItemId != -1) {
                    await PackContainers();

                    view3D.UnpackAllItemsInRender();

                    var containerPackingResult = ContainerPackingResult.find(elem => elem.ContainerID == result.PrateleiraId);

                    if (containerPackingResult != null) {
                        view3D.ShowPackingView(containerPackingResult, result.NovoItemId);
                        view3D.PackAllItemsInRender();
                    }

                    ExibirRota(BalcaoAncoragem, containerPackingResult.Ancoragem, containerPackingResult.ContainerID);

                    ModalLista.Fechar();
                }
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }

    static async Retirar(id) {
        await PackContainers();

        $.ajax({
            type: "POST",
            url: "/ControleDeEstoque/RetirarItem",
            data: { TipoItemEstoqueId: id },
            success: async function (result) {
                if (result.ItemId != -1) {
                    view3D.UnpackAllItemsInRender();

                    var containerPackingResult = ContainerPackingResult.find(elem => elem.ContainerID == result.PrateleiraId);

                    if (containerPackingResult != null) {
                        view3D.ShowPackingView(containerPackingResult, result.ItemId);
                        view3D.PackAllItemsInRender();
                    }

                    ExibirRota(BalcaoAncoragem, containerPackingResult.Ancoragem, containerPackingResult.ContainerID);

                    ModalLista.Fechar();
                }
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }

    static NaoEstocados = class NaoEstocados {
        static Objetos = [];

        static Abrir(id) {
            $('#modal-pesquisa-nao-estocados').val('');
            this.Filtrar();

            this.Carregar();

            $('#modal-nao-estocados').css('display', 'flex');
        }

        static Fechar() {
            $('#modal-lista-nao-estocados-corpo').empty();

            $('#modal-nao-estocados').css('display', 'none');
        }

        static Carregar(callback = () => { }) {
            $('#modal-lista-nao-estocados-corpo').empty();

            $.ajax({
                type: "POST",
                url: "/TipoItemEstoque/RetornarNaoEstocados",
                success: function (result) {
                    for (var i in result) {
                        var item = result[i];

                        CadItem.NaoEstocados.Objetos.push(item);

                        $('#modal-lista-nao-estocados-corpo').append(`
                            <div class="modal-item-container modal-item-nao-estocados-container" data-id="${item.Id}">
                                <div class="item-header">
                                    <span>${item.Nome}</span>
                                </div>
                                <div class="item-corpo">
                                    <div>
                                        <span>
                                            Quantidade
                                        </span>
                                        <span>${item.Quantidade}</span>
                                    </div>
                                    <hr>
                                    <div>
                                        <span>
                                            Código de Barras
                                        </span>
                                        <span>${item.CodigoDeBarras}</span>
                                    </div>
                                    <hr>
                                    <div>
                                        <span>
                                            Largura
                                        </span>
                                        <span>${item.Largura}m</span>
                                    </div>
                                    <hr>
                                    <div>
                                        <span>
                                            Altura
                                        </span>
                                        <span>${item.Altura}m</span>
                                    </div>
                                    <hr>
                                    <div>
                                        <span>
                                            Profundidade
                                        </span>
                                        <span>${item.Profundidade}m</span>
                                    </div>
                                    <hr>
                                    <div>
                                        <span>
                                            Peso
                                        </span>
                                        <span>${item.Peso}kg</span>
                                    </div>
                                    <hr>
                                    <div>
                                        <span>
                                            Peso Máximo Empilhamento
                                        </span>
                                        <span>${item.PesoMaximoEmpilhamento}kg</span>
                                    </div>
                                </div>
                                <div class="item-footer">
                                    <div class="item-botao" onclick="{CadItem.Estocar(${item.TipoItemEstoqueId}, ${item.Id}); CadItem.NaoEstocados.Fechar();}">
                                        <img src="/Content/Icones/plus.svg" width="24" height="24">
                                    </div>
                                    <div class="item-botao item-botao-deletar" onclick="CadItem.NaoEstocados.DeletarItem.Abrir(${item.Id})">
                                        <img src="/Content/Icones/trash.svg" width="24" height="24">
                                    </div>
                                </div>
                            </div>
                        `);
                    }

                    if ($('#modal-lista-nao-estocados-corpo').html() == '') {
                        $('#modal-lista-nao-estocados-corpo').append(`
                        <div class="sem-itens-container">
                            <span class="sem-itens-span">Não há itens não estocados.</span>
                        </div>
                    `);
                    }

                    callback();
                },
                error: function (req, status, error) {
                    console.log("Erro.");
                }
            });
        }

        static Filtrar() {
            var pesquisa = $('#modal-pesquisa-nao-estocados').val().toLowerCase();

            $(".modal-item-nao-estocados-container").css('display', 'none');

            if (pesquisa == '')
                $(".modal-item-nao-estocados-container").css('display', 'flex');

            var objetosFiltrados = this.Objetos.filter(obj => (obj.Nome != null && obj.Nome.toLowerCase().includes(pesquisa)));

            for (var i in objetosFiltrados) {
                $('.modal-item-nao-estocados-container[data-id="' + objetosFiltrados[i].Id + '"]').css('display', 'flex');
            }
        }

        static DeletarItem = class DeletarItem {
            static DeletarId = null;

            static Abrir(id) {
                this.DeletarId = id;

                $('#modal-deletar-item-nao-estocado').css('display', 'flex');
            }

            static Fechar() {
                this.DeletarId = null;

                $('#modal-deletar-item-nao-estocado').css('display', 'none');
            }

            static Finalizar() {
                $.ajax({
                    type: "POST",
                    url: "/TipoItemEstoque/DeletarItem",
                    data: { ItemEstoqueId: this.DeletarId },
                    success: function (result) {
                        CadItem.NaoEstocados.Carregar();
                        CadItem.NaoEstocados.DeletarItem.Fechar();
                    },
                    error: function (req, status, error) {
                        console.log("Erro.");
                    }
                });
            }
        }
    }
}