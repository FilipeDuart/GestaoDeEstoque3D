using CromulentBisgetti.ContainerPacking;
using CromulentBisgetti.ContainerPacking.Entities;
using GestaoDeEstoque3D.Dapper.Core;
using GestaoDeEstoque3D.Dapper.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GestaoDeEstoque3D.Controllers
{
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ContainerPacking()
        {
            var balcaoAncoragemDefinition = new { lat = new double(), lng = new double() };
            var balcaoAncoragem = JsonConvert.DeserializeAnonymousType(new SistemaConfiguracaoCore().RetornarPorNome("balcao-ancoragem-latlng").Valor, balcaoAncoragemDefinition);

            var estantes = new EstanteCore().RetornarTodosComItens();
            var prateleiras = new List<Prateleira>();

            estantes.Select(e => e.Prateleiras).ToList().ForEach(e => prateleiras.AddRange(e));

            var PackResult = prateleiras.Select(e => new
            {
                ContainerID = e.Id,
                PackedItems = e.ItemsEstoque.Select(i => new {
                    ItemId = i.Id,
                    PackDimX = i.TipoItemEstoque.Largura,
                    PackDimY = i.TipoItemEstoque.Altura,
                    PackDimZ = i.TipoItemEstoque.Profundidade,
                    CoordX = i.PackX,
                    CoordY = i.PackY,
                    CoordZ = i.PackZ
                }),
                Ancoragem = new
                {
                    Lat = e.Estante.AncoragemLat,
                    Lng = e.Estante.AncoragemLng,
                }
            });

            var Containers = prateleiras.Select(e => new {
                ID = e.Id,
                Length = e.Estante.ProfundidadePrateleiras,
                Width = e.Estante.LarguraPrateleiras,
                Height = e.Estante.AlturaPrateleiras
            });

            var response = new
            {
                PackResult,
                Containers,
                BalcaoAncoragem = new
                {
                    Lat = balcaoAncoragem.lat,
                    Lng = balcaoAncoragem.lng
                }
            };

            return Json(response);
        }

        //public JsonResult AssociarUmTeste()
        //{
        //    var estanteCore = new EstanteCore();
        //    var itemEstoqueCore = new ItemEstoqueCore();
        //    var estantes = estanteCore.RetornarTodosComItens();

        //    var containers = new List<OnlineContainerPacking.Models.Container>();
        //    estantes.OrderBy(e => e.Id).ToList().ForEach(estante =>
        //    {
        //        var items = new List<OnlineContainerPacking.Models.Item>();
        //        estante.ItemsEstoque.OrderByDescending(i => i.PackY).ToList().ForEach(itemEstoque => {
        //            var TipoItemEstoque = itemEstoque.TipoItemEstoque;
        //            var _itemContainerPacking = new OnlineContainerPacking.Models.Item(itemEstoque.Id, Convert.ToDecimal(TipoItemEstoque.Largura), Convert.ToDecimal(TipoItemEstoque.Altura), Convert.ToDecimal(TipoItemEstoque.Profundidade), Convert.ToDecimal(itemEstoque.PackX), Convert.ToDecimal(itemEstoque.PackY), Convert.ToDecimal(itemEstoque.PackZ), 1, TipoItemEstoque.Id, itemEstoque.ItemBaseId);

        //            if (itemEstoque.PackY == 0) //É um item base
        //            {
        //                _itemContainerPacking.ItensEmpilhados = items.Where(i => i.ItemBaseId == _itemContainerPacking.ID).OrderBy(i => i.CoordY).ToList();

        //                if (Convert.ToDecimal(estante.AlturaPrateleiras) < _itemContainerPacking.Dim2 * (_itemContainerPacking.ItensEmpilhados.Count + 2))
        //                {
        //                    _itemContainerPacking.EmpilhamentoDisponivel = false;
        //                }
        //            }

        //            _itemContainerPacking.IsPacked = true;
        //            items.Add(_itemContainerPacking);
        //        });

        //        containers.Add(new OnlineContainerPacking.Models.Container(estante.Id, Convert.ToDecimal(estante.ProfundidadePrateleiras), Convert.ToDecimal(estante.LarguraPrateleiras), Convert.ToDecimal(estante.AlturaPrateleiras), items));
        //    });

        //    var itemsToPack = new List<OnlineContainerPacking.Models.Item>();

        //    //Todo ajeitar parametros
        //    var tipoItemEstoque = new TipoItemEstoqueCore().RetornarPorId(2);
        //    itemsToPack.Add(new OnlineContainerPacking.Models.Item(-1, Convert.ToDecimal(tipoItemEstoque.Largura), Convert.ToDecimal(tipoItemEstoque.Altura), Convert.ToDecimal(tipoItemEstoque.Profundidade), 1, tipoItemEstoque.Id)); //TODO

        //    OnlineContainerPacking.PackingService.OnlinePack(containers, itemsToPack);

        //    //Parallel.ForEach(containers.Where(c => c.Alterado).ToList(), c =>
        //    //{
        //    //    var estante = estantes.Find(e => e.Id == c.ID);
        //    //    estante.PackingJson = c.PackingJson;

        //    //    estanteCore.Alterar(estante);
        //    //});

        //    Parallel.ForEach(itemsToPack.Where(i => i.IsPacked).ToList(), i =>
        //    {
        //        var itemEstoque = new ItemEstoque()
        //        {
        //            ItemBaseId = i.ItemBaseId,
        //            EstanteId = i.ContainerId,
        //            PackX = Convert.ToDouble(i.CoordX),
        //            PackY = Convert.ToDouble(i.CoordY),
        //            PackZ = Convert.ToDouble(i.CoordZ),
        //            TipoItemEstoqueId = i.TipoDeItemId, //TODO
        //            DataHora = DateTime.Now,
        //            UsuarioId = null,
        //            Ativo = true
        //        };

        //        itemEstoqueCore.Inserir(itemEstoque);
        //    });

        //    var response = "Associado mais um";

        //    return Json(response);
        //}
    }
}