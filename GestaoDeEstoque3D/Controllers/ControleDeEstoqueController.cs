using GestaoDeEstoque3D.Dapper.Core;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GestaoDeEstoque3D.Controllers
{
    public class ControleDeEstoqueController : Controller
    {
        public JsonResult EstocarItem(int TipoItemEstoqueId, int? ItemEstoqueId = null)
        {
            var tipoItemEstoque = new TipoItemEstoqueCore().RetornarPorId(TipoItemEstoqueId);
            var novoItem = new OnlineContainerPacking.Models.Item(-1, Convert.ToDecimal(tipoItemEstoque.Largura), Convert.ToDecimal(tipoItemEstoque.Altura), Convert.ToDecimal(tipoItemEstoque.Profundidade), 1, tipoItemEstoque.Id);
            novoItem.Peso = tipoItemEstoque.Peso ?? 0;
            novoItem.PesoMaximo = tipoItemEstoque.PesoMaximoEmpilhamento ?? 0;

            //===Carrega containers/estantes=====================
            var estanteCore = new EstanteCore();
            var itemEstoqueCore = new ItemEstoqueCore();
            var estantes = estanteCore.RetornarTodosComItens();

            var containers = new List<OnlineContainerPacking.Models.Container>();
            estantes.OrderBy(e => e.Id).ToList().ForEach(estante =>
            {
                estante.Prateleiras.OrderBy(p => p.Nivel).ToList().ForEach(prateleira =>
                {
                    //Ignora prateleira que nao suportam mais peso
                    var pesoAtual = prateleira.ItemsEstoque.Sum(item => item.TipoItemEstoque.Peso ?? 0);
                    if (pesoAtual + novoItem.Peso > estante.PesoMaximoPrateleiras)
                        return;

                    var items = new List<OnlineContainerPacking.Models.Item>();
                    prateleira.ItemsEstoque.OrderByDescending(i => i.PackY).ToList().ForEach(itemEstoque => {
                        var TipoItemEstoque = itemEstoque.TipoItemEstoque;
                        var _itemContainerPacking = new OnlineContainerPacking.Models.Item(itemEstoque.Id, Convert.ToDecimal(TipoItemEstoque.Largura), Convert.ToDecimal(TipoItemEstoque.Altura), Convert.ToDecimal(TipoItemEstoque.Profundidade), Convert.ToDecimal(itemEstoque.PackX), Convert.ToDecimal(itemEstoque.PackY), Convert.ToDecimal(itemEstoque.PackZ), 1, TipoItemEstoque.Id, itemEstoque.ItemBaseId, TipoItemEstoque.Peso ?? 0, TipoItemEstoque.PesoMaximoEmpilhamento ?? 1);

                        if (itemEstoque.PackY == 0) //É um item base
                        {
                            _itemContainerPacking.ItensEmpilhados = items.Where(i => i.ItemBaseId == _itemContainerPacking.ID).OrderBy(i => i.CoordY).ToList();

                            if (Convert.ToDecimal(estante.AlturaPrateleiras) < _itemContainerPacking.Dim2 * (_itemContainerPacking.ItensEmpilhados.Count + 2))
                            {
                                _itemContainerPacking.EmpilhamentoDisponivel = false;
                            }
                            
                            if ((_itemContainerPacking.ItensEmpilhados.Count + 1) * _itemContainerPacking.Peso > _itemContainerPacking.PesoMaximo)
                            {
                                _itemContainerPacking.EmpilhamentoDisponivel = false;
                            }
                        }

                        _itemContainerPacking.IsPacked = true;
                        items.Add(_itemContainerPacking);
                    });
                    
                    containers.Add(new OnlineContainerPacking.Models.Container(prateleira.Id, Convert.ToDecimal(estante.ProfundidadePrateleiras), Convert.ToDecimal(estante.LarguraPrateleiras), Convert.ToDecimal(estante.AlturaPrateleiras), items));
                });
            });
            //=====================================================

            var itemsToPack = new List<OnlineContainerPacking.Models.Item>();
            itemsToPack.Add(novoItem);

            OnlineContainerPacking.PackingService.OnlinePack(containers, itemsToPack);

            ItemEstoque novoItemEstoque = null;
            if (novoItem.IsPacked)
            {
            //Parallel.ForEach(itemsToPack.Where(i => i.IsPacked).ToList(), i =>
            //{
                if(ItemEstoqueId == null)
                {
                    var itemEstoque = new ItemEstoque()
                    {
                        ItemBaseId = novoItem.ItemBaseId,
                        PrateleiraId = novoItem.ContainerId,
                        PackX = Convert.ToDouble(novoItem.CoordX),
                        PackY = Convert.ToDouble(novoItem.CoordY),
                        PackZ = Convert.ToDouble(novoItem.CoordZ),
                        TipoItemEstoqueId = novoItem.TipoDeItemId,
                        DataHora = DateTime.Now,
                        UsuarioId = null,
                        Ativo = true
                    };

                    itemEstoqueCore.Inserir(itemEstoque);

                    novoItemEstoque = itemEstoque;
                } 
                else
                {
                    var itemEstoque = itemEstoqueCore.RetornarPorId(ItemEstoqueId ?? -1);

                    itemEstoque.ItemBaseId = novoItem.ItemBaseId;
                    itemEstoque.PrateleiraId = novoItem.ContainerId;
                    itemEstoque.PackX = Convert.ToDouble(novoItem.CoordX);
                    itemEstoque.PackY = Convert.ToDouble(novoItem.CoordY);
                    itemEstoque.PackZ = Convert.ToDouble(novoItem.CoordZ);
                    itemEstoque.TipoItemEstoqueId = novoItem.TipoDeItemId;
                    itemEstoque.DataHora = DateTime.Now;
                    itemEstoque.UsuarioId = null;
                    itemEstoque.Ativo = true;

                    itemEstoqueCore.Alterar(itemEstoque);

                    novoItemEstoque = itemEstoque;
                }
            //});
            }

            var response = new
            {
                NovoItemId = novoItemEstoque == null ? -1 : novoItemEstoque.Id,
                PrateleiraId = novoItemEstoque == null ? -1 : novoItemEstoque.PrateleiraId
            };

            return Json(response);
        }

        public JsonResult RetirarItem(int TipoItemEstoqueId)
        {
            var itemEstoqueCore = new ItemEstoqueCore();

            var itemEstoque = itemEstoqueCore.RetornarUltimoAssociadoPorTipoId(TipoItemEstoqueId);
            var prateleiraId = itemEstoque.PrateleiraId;

            //itemEstoque.PrateleiraId = null;
            //itemEstoqueCore.Alterar(itemEstoque);

            itemEstoqueCore.Deletar(itemEstoque);

            var response = new
            {
                ItemId = itemEstoque.Id,
                PrateleiraId = prateleiraId ?? -1
            };

            return Json(response);
        }
    }
}