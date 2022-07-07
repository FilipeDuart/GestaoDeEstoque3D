var planta;
var camadas = [];
var layerControl;
var desenhandoPoligono = false;
var poligonoSelecionado;
var estantesAssociadas;
var prateleiras;
var dragStartId = 1;
var dragEndId = 0;

//rota
var startRota = null;
var endRota = null;
var linhaRota = null;
var estanteRota = null;

InicializarPlanta();

function InicializarPlanta() {
    planta = L.map('planta', {
        maxZoom: 30,
        minZoom: -30,
        crs: L.CRS.Simple,
        attributionControl: false,
        closePopupOnClick: false,
        preferCanvas: true,
        doubleClickZoom: false,
    }).setView([0, 0], 1);

    layerControl = L.control.layers(null, null, { position: 'topleft' }).addTo(planta);

    planta.pm.addControls({
        position: 'topleft',
        drawMarker: false,
        drawCircleMarker: false,
        drawPolyline: true,
        drawRectangle: false,
        drawPolygon: false,
        drawCircle: false,
        editMode: true,
        cutPolygon: false
    }); 

    planta.pm.setGlobalOptions({ snappable: true, snapDistance: 20, snapMiddle: true });

    //var containers = [];

    //containers.push(CriarControlAdicionarEstante());
    //containers.push(CriarControlItensDeEstoque());

    //CriarToolbar(containers);

    planta.on('click', function (e) {
        console.log(e);

        if (Ancoragem.DefinindoAncoragem === true) {
            Ancoragem.DefinirPonto(e.latlng, () => { Ancoragem.Finalizar() });
        }

        //if (!desenhandoPoligono) {
        //    if (startRota == null) {
        //        startRota = L.marker(e.latlng).addTo(planta);
        //    } else if (endRota == null) {
        //        endRota = L.marker(e.latlng).addTo(planta);
        //        var linha = Gestao.Rota.Gerar(startRota.toGeoJSON(), endRota.toGeoJSON());
        //        if (linha != null) {
        //            linhaRota = linha;
        //            linhaRota.addTo(planta);
        //        }
        //    } else {
        //        if (linhaRota != null)
        //            linhaRota.removeFrom(planta);
        //        linhaRota = null;

        //        startRota.removeFrom(planta);
        //        endRota.removeFrom(planta);
        //        startRota = null;
        //        endRota = null;
        //    }
        //}
    });

    planta.on('pm:drawstart', function (e) {
        desenhandoPoligono = true;
    });

    planta.on('pm:drawend', function (e) {
        desenhandoPoligono = false;
    });

    planta.on('pm:edit', function (event) {
        SalvarPoligono(event.layer, event.layer.feature.properties.CamadaId, event.layer.feature.properties.CamadaNome);
    });

    planta.on('pm:create', function (event) {
        PrepararPoligonoParaSalvar(event.layer);
    });

    var imageUrl = '/Content/Imagens/exemplo-planta.png'
    var imageWidth = 1158;
    var imageHeight = 634;
    var fatorDivisao = 5;

    //SouthWest, NorthEast
    var imageBounds = [[-imageHeight / fatorDivisao, -imageWidth / fatorDivisao], [imageHeight / fatorDivisao, imageWidth / fatorDivisao]];

    var plantaArmazem = L.imageOverlay(imageUrl, imageBounds);
    //layerControl.addOverlay(plantaArmazem, "Planta baixa");
    //plantaArmazem.addTo(planta);

    CarregarCamadas();
}

function LimparRota() {
    if (linhaRota != null)
        linhaRota.removeFrom(planta);

    linhaRota = null;

    if (estanteRota != null)
        estanteRota.setStyle({ fillOpacity: 0.2 });

    estanteRota = null;
}

function ExibirRota(latLgnInicio, latLgnFim, prateleiraId) {
    LimparRota();

    var prateleira = prateleiras.find(i => i.Id == prateleiraId);

    var _estante = estantesAssociadas.find(i => i.Id == prateleira.EstanteId);

    $('#select-nivel-prateleira').empty();

    for (var i = 1; i <= _estante.QuantidadePrateleiras; i++) {
        if (i == prateleira.Nivel)
            $('#select-nivel-prateleira').append(`
                        <div class="nivel-seletor nivel-seletor-ativo" data-value="${i}">
                            ${i}
                        </div>
                    `);
        else
            $('#select-nivel-prateleira').append(`
                        <div class="nivel-seletor" data-value="${i}">
                            ${i}
                        </div>
                    `);
    }

    poligonoSelecionado = camadas["Estantes"].leafletLayer.getLayers().find(layer => layer.feature.properties.PoligonoId == _estante.PoligonoId);

    estanteRota = camadas["Estantes"].leafletLayer.getLayers().find(layer => layer.feature.properties.PoligonoId == _estante.PoligonoId);

    if (estanteRota != null)
        estanteRota.setStyle({ fillOpacity: 0.6 });

    var linha = Gestao.Rota.Gerar(L.marker(L.latLng(latLgnInicio.Lat, latLgnInicio.Lng)).toGeoJSON(), L.marker(L.latLng(latLgnFim.Lat, latLgnFim.Lng)).toGeoJSON());
    if (linha != null) {
        linhaRota = linha;
        linhaRota.addTo(planta);
    }
}

function LimparPlanta() {
    for (var camada in camadas) {
        planta.removeLayer(camadas[camada].leafletLayer);
        layerControl.removeLayer(camadas[camada].leafletLayer);
    }

    camadas = [];
}

function CarregarCamadas() {
    LimparPlanta();

    $.ajax({
        type: 'POST',
        url: '/Planta2D/CarregarCamadas',
        success: function (response) {
            estantesAssociadas = response.estantesAssociadas;
            prateleiras = response.prateleiras;

            var camadasGeojson = response.camadasGeojson;

            for (var camada of camadasGeojson) {
                camadas[camada.CamadaNome] = {
                    leafletLayer: null,
                    geojson: JSON.parse(camada.CamadaGeojson),
                    propriedades: {
                        CamadaId: camada.CamadaId,
                        CamadaNome: camada.CamadaNome,
                        CamadaGeojson: camada.CamadaGeojson,
                        CamadaCor: camada.CamadaCor
                    }
                };
            }

            AdicionarCamadas();
        },
        error: function () {
            console.log('Erro.');
        }
    });
}

function AdicionarCamadas() {
    for (var camadaNome in camadas) {
        var camada = camadas[camadaNome];

        var geojsonLayer = L.geoJson(camada.geojson, {
            onEachFeature: onEachFeature,
            style: function (feature) {
                return { color: camada.propriedades.CamadaCor };
            }
        });

        camadas[camada.propriedades.CamadaNome].leafletLayer = geojsonLayer;
        layerControl.addOverlay(geojsonLayer, camada.propriedades.CamadaNome);

        geojsonLayer.addTo(planta);
    }

    planta.fitBounds(camadas["Estantes"].leafletLayer.getBounds());

    Gestao.Rota.Inicializar(camadas["Corredores"].geojson);
}

function onEachFeature(feature, layer) {
    if (feature.properties.CamadaNome == 'Balcão' || feature.properties.CamadaNome == 'Estantes') {
        layer.on('contextmenu', function (e) {
            if (e.target.feature.properties.CamadaNome == 'Estantes') {
                Ancoragem.EstanteAncoragem = e.target;
                Ancoragem.BalcaoAncoragem = null;
            } else {
                Ancoragem.BalcaoAncoragem = e.target;
                Ancoragem.EstanteAncoragem = null;
            }

            layer.bindPopup(`
                <button class="opcao-context-menu" onclick="Ancoragem.Iniciar()">Definir ponto de ancoragem</button>
            `).openPopup().unbindPopup();
        });
    }

    layer.on('click', async function (e) {
        await PackContainers();

        poligonoSelecionado = layer;

        console.log(e.target.feature.properties);

        view3D.UnpackAllItemsInRender();

        var estanteAssociada = estantesAssociadas.find(i => i.PoligonoId == e.target.feature.properties.PoligonoId);
        if (estanteAssociada != null) {
            $('#select-nivel-prateleira').empty();

            for (var i = 1; i <= estanteAssociada.QuantidadePrateleiras; i++){
                if(i == 1)
                    $('#select-nivel-prateleira').append(`
                        <div class="nivel-seletor nivel-seletor-ativo" data-value="${i}">
                            ${i}
                        </div>
                    `);
                else
                    $('#select-nivel-prateleira').append(`
                        <div class="nivel-seletor" data-value="${i}">
                            ${i}
                        </div>
                    `);
            }

            var prateleira = prateleiras.find(i => i.EstanteId == estanteAssociada.Id && i.Nivel == 1);

            var containerPackingResult = ContainerPackingResult.find(elem => elem.ContainerID == prateleira.Id);

            if (containerPackingResult != null) {
                view3D.ShowPackingView(containerPackingResult);
                view3D.PackAllItemsInRender();
            }
        }

        LimparRota();

        //if (poligonoSelecionado.feature.properties.CamadaNome == "Estantes") {
        //    var estanteAssociada = estantesAssociadas.find(i => i.PoligonoId == layer.feature.properties.PoligonoId);

        //    if (estanteAssociada == null) {
        //        layer.bindPopup('<button onclick="AbrirModalAdicionarReferencia()">Adicionar referência</button>').openPopup().unbindPopup();
        //    } else {
        //        layer.bindPopup('<button onclick="RemoverReferenciaEstante()">Remover referência</button>').openPopup().unbindPopup();
        //    }
        //}
    });

    layer.on('pm:update', function (event) {
        SalvarPoligono(event.layer, event.layer.feature.properties.CamadaId, event.layer.feature.properties.CamadaNome);
    });

    layer.on('pm:dragstart', function (event) {
        layer.toggleTooltip();
    });

    layer.on('pm:dragend', function (event) {
        SalvarPoligono(event.layer, event.layer.feature.properties.CamadaId, event.layer.feature.properties.CamadaNome);
        layer.toggleTooltip();
    });

    layer.on('pm:remove', function (event) {
        DeletarPoligono(event.layer);
    });

    var estanteAssociada = estantesAssociadas.find(i => i.PoligonoId == layer.feature.properties.PoligonoId);
    
    if (estanteAssociada != null) {
        layer.bindTooltip('Cód.: ' + estanteAssociada.Id + '<br>Prateleiras: ' + estanteAssociada.QuantidadePrateleiras, {
            permanent: true,
            opacity: 1,
            className: 'label-planta',
            direction: 'center'
        });
    }
}

function PrepararPoligonoParaSalvar(poligono) {
    try {
        SalvarPoligono(poligono, poligono.feature.properties.CamadaId, poligono.feature.properties.CamadaNome);
    } catch (e) {
        var feature = poligono.feature = poligono.feature || {}; // Initialize feature
        feature.type = feature.type || "Feature"; // Initialize feature.type
        var props = feature.properties = feature.properties || {}; // Initialize feature.properties

        props.PoligonoId = "-1";

        AbrirModalNovoPoligono(poligono);
    }
}

function AbrirModalNovoPoligono(poligono) {
    var modal = document.getElementById("ModalNovoPoligono");
    var select_camada = $('#select-camada-novo-poligono');
    var btn = document.getElementById('btnNovoPoligono');

    select_camada.empty();

    for (i in camadas) {
        var camada = camadas[i];
        select_camada
            .append($("<option></option>")
                .attr("value", camada.propriedades.CamadaId)
                .text(camada.propriedades.CamadaNome));
    }

    btn.onclick = function () {
        modal.style.display = "none";
        var camadaId = $('#select-camada-novo-poligono').val();
        var camadaNome = $('#select-camada-novo-poligono option').filter(':selected').text();

        poligono.feature.properties.CamadaId = camadaId;
        poligono.feature.properties.CamadaNome = camadaNome;

        SalvarPoligono(poligono, camadaId, camadaNome);
    }

    modal.style.display = "flex";
}

function SalvarPoligono(poligono, camadaId, camadaNome) {
    if (poligono.feature.properties.PoligonoId == -1) {
        camadas[camadaNome].leafletLayer.addLayer(poligono);
        poligono.options.onEachFeature = onEachFeature;
        onEachFeature(poligono.feature, poligono);
    }

    try {
        poligono.bringToFront();
    } catch (e) {
        console.log('nao possui metodo layer.bringToFront()');
    }

    poligonoSelecionado = poligono;

    var parametrosAjax = { PoligonoId: poligono.feature.properties.PoligonoId, CamadaId: camadaId, Geojson: JSON.stringify(poligono.toGeoJSON()) };
    $.ajax({
        type: "POST",
        url: "/Planta2D/SalvarPoligono",
        data: parametrosAjax,
        success: function (result) {
            poligono.feature.properties.PoligonoId = result;
            poligono.setStyle({ color: camadas[camadaNome].propriedades.CamadaCor })

            if (camadaNome == "Corredores")
                Gestao.Rota.Inicializar(camadas["Corredores"].geojson);
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function DeletarPoligono(poligono) {
    var parametrosAjax = { PoligonoId: poligono.feature.properties.PoligonoId };
    $.ajax({
        type: "POST",
        url: "/Planta2D/DeletarPoligono",
        data: parametrosAjax,
        success: function (result) {
            camadas[poligono.feature.properties.CamadaNome].leafletLayer.removeLayer(poligono)

            if (poligono.feature.properties.CamadaNome == "Corredores")
                Gestao.Rota.Inicializar(camadas["Corredores"].geojson);
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function AbrirModalAdicionarReferencia(poligono) {
    var modal = document.getElementById("ModalAdicionarReferencia");
    var codigo_estante_input = $('#codigo-estante-input');
    var btn = document.getElementById('btnAdicionarReferencia');

    codigo_estante_input.val('');

    btn.onclick = function () {
        modal.style.display = "none";

        AdicionarReferenciaEstante(poligonoSelecionado, codigo_estante_input.val());
    }

    modal.style.display = "flex";
}

function AdicionarReferenciaEstante(poligono, estanteId) {
    var parametrosAjax = { PoligonoId: poligono.feature.properties.PoligonoId, EstanteId: estanteId };
    $.ajax({
        type: "POST",
        url: "/Planta2D/AdicionarReferenciaEstante",
        data: parametrosAjax,
        success: function (result) {
            estantesAssociadas.push(result);

            var estanteAssociada = estantesAssociadas.find(i => i.PoligonoId == poligono.feature.properties.PoligonoId);

            if (estanteAssociada != null) {
                poligono.bindTooltip('Cód.: ' + estanteAssociada.Id + '<br>Prateleiras: ' + estanteAssociada.QuantidadePrateleiras, {
                    permanent: true,
                    opacity: 1,
                    className: 'label-planta',
                    direction: 'center'
                });
            }

            planta.closePopup();
        },
        error: function (req, status, error) {
            console.log("Erro.");
        }
    });
}

function RemoverReferenciaEstante() {
    var poligono = poligonoSelecionado;
    var estanteAssociada = estantesAssociadas.find(i => i.PoligonoId == poligono.feature.properties.PoligonoId);

    if (estanteAssociada != null) {
        var parametrosAjax = { EstanteId: estanteAssociada.Id };
        $.ajax({
            type: "POST",
            url: "/Planta2D/RemoverReferenciaEstante",
            data: parametrosAjax,
            success: function () {
                estantesAssociadas = estantesAssociadas.filter(estante => estante.Id !== estanteAssociada.Id)
                poligono.unbindTooltip();
                planta.closePopup();
            },
            error: function (req, status, error) {
                console.log("Erro.");
            }
        });
    }
}

async function TrocarNivel(nivel) {
    await PackContainers();

    view3D.UnpackAllItemsInRender();

    var estanteAssociada = estantesAssociadas.find(i => i.PoligonoId == poligonoSelecionado.feature.properties.PoligonoId);

    if (estanteAssociada != null) {
        var prateleira = prateleiras.find(i => i.EstanteId == estanteAssociada.Id && i.Nivel == nivel);

        var containerPackingResult = ContainerPackingResult.find(elem => elem.ContainerID == prateleira.Id);

        if (containerPackingResult != null) {
            view3D.ShowPackingView(containerPackingResult);
            view3D.PackAllItemsInRender();
        }
    }
}

$(document).on('click', '.nivel-seletor', function (e) {
    $('.nivel-seletor').removeClass('nivel-seletor-ativo');
    $(this).addClass('nivel-seletor-ativo');

    TrocarNivel($(this).data('value'));
});