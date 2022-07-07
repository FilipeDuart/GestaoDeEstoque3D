class AssociadosEstante {
    static EstanteId = null;
    static Objetos = [];

    static Abrir(id) {
        this.EstanteId = id;

        $('#modal-pesquisa-associados').val('');
        this.Filtrar();

        this.Carregar();

        $('#modal-associados-estante').css('display', 'flex');
    }

    static Fechar() {
        this.EstanteId = null;

        $('#modal-lista-associados-corpo').empty();

        $('#modal-associados-estante').css('display', 'none');
    }

    static Carregar(callback = () => { }) {
        $('#modal-lista-associados-corpo').empty();

        $.ajax({
            type: "POST",
            url: "/Estante/RetornarAssociados",
            data: { EstanteId: this.EstanteId },
            success: function (result) {
                for (var j in result) {
                    for (var i in result[j].Itens) {
                        var item = result[j].Itens[i];

                        AssociadosEstante.Objetos.push(item);

                        $('#modal-lista-associados-corpo').append(`
                                <div class="modal-item-container modal-item-associado-container" data-id="${item.Id}">
                                    <div class="item-header">
                                        <span>${item.Nome}</span>
                                    </div>
                                    <div class="item-corpo" style="margin-bottom: 10px;">
                                        <div>
                                            <span>
                                                Nível
                                            </span>
                                            <span>${result[j].Nivel}</span>
                                        </div>
                                        <hr>
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
                                </div>
                            `);
                                    //<div class="item-footer">
                                    //    <div class="item-botao" onclick="CadItem.Estocar(${result[i].Id})">
                                    //        <img src="/Content/Icones/plus.svg" width="24" height="24">
                                    //    </div>
                                    //    <div class="item-botao ${result[i].PossuiAssociacao ? '' : 'item-botao-desativado'}" ${result[i].PossuiAssociacao ? `onclick="CadItem.Retirar(${result[i].Id})"` : ''}>
                                    //        <img src="/Content/Icones/minus.svg" width="24" height="24">
                                    //    </div>
                                    //    <div class="item-botao" onclick="CadItem.Abrir(${result[i].Id})">
                                    //        <img src="/Content/Icones/edit.svg" width="24" height="24">
                                    //    </div>
                                    //    <div class="item-botao item-botao-deletar" onclick="CadItem.Deletar.Abrir(${result[i].Id})">
                                    //        <img src="/Content/Icones/trash.svg" width="24" height="24">
                                    //    </div>
                                    //</div>
                    }
                }

                if ($('#modal-lista-associados-corpo').html() == '') {
                    $('#modal-lista-associados-corpo').append(`
                        <div class="sem-itens-container">
                            <span class="sem-itens-span">Não há itens estocados nesta estante</span>
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
        var pesquisa = $('#modal-pesquisa-associados').val().toLowerCase();

        $(".modal-item-associado-container").css('display', 'none');

        if (pesquisa == '')
            $(".modal-item-associado-container").css('display', 'flex');

        var objetosFiltrados = this.Objetos.filter(obj => (obj.Nome != null && obj.Nome.toLowerCase().includes(pesquisa)));

        for (var i in objetosFiltrados) {
            $('.modal-item-associado-container[data-id="' + objetosFiltrados[i].Id + '"]').css('display', 'flex');
        }
    }
}