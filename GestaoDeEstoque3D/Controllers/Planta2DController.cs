using GestaoDeEstoque3D.Dapper.Core;
using GestaoDeEstoque3D.Dapper.Model;
using GestaoDeEstoque3D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestaoDeEstoque3D.Controllers
{
    public class Planta2DController : Controller
    {
        // GET: Planta2D
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CarregarCamadas()
        {
            var camadas = new CamadaCore().RetornarTodos();
            var poligonos = new PoligonoCore().RetornarTodos();
            var estantesAssociadas = new EstanteCore().RetornarEstantesAssociadas();
            var prateleiras = new List<Prateleira>();
            estantesAssociadas.Select(e => e.Prateleiras).ToList().ForEach(e => prateleiras.AddRange(e));

            var camadasGeojson = new List<CamadaGeojsonVM>();

            foreach(var camada in camadas)
            {
                var geojson = "[" + string.Join(",", poligonos.Where(pol => pol.CamadaId == camada.Id).Select(pol => pol.Geojson).ToArray()) + "]";

                var camadaGeojson = new CamadaGeojsonVM() { CamadaId = camada.Id, CamadaNome = camada.Nome, CamadaGeojson = geojson, CamadaCor = camada.Cor };

                camadasGeojson.Add(camadaGeojson);
            }

            var response = new
            {
                camadasGeojson = camadasGeojson.Select(i => new
                {
                    i.CamadaId,
                    i.CamadaNome,
                    i.CamadaGeojson,
                    i.CamadaCor
                }),
                estantesAssociadas = estantesAssociadas.Select(est => new { 
                    est.Id,
                    est.QuantidadePrateleiras,
                    est.PoligonoId
                }),
                prateleiras = prateleiras.Select(pra => new { 
                    pra.Id,
                    pra.EstanteId,
                    pra.Nivel
                })
            };

            var return_json = Json(response, JsonRequestBehavior.AllowGet);
            return_json.MaxJsonLength = int.MaxValue;
            return return_json;
        }

        [HttpPost]
        public int SalvarPoligono(int PoligonoId, int CamadaId, string Geojson, bool Ativo = true)
        {
            if(PoligonoId == -1)
            {
                var poligono = new Poligono()
                {
                    CamadaId = CamadaId,
                    Geojson = Geojson,
                    Ativo = Ativo
                };

                new PoligonoCore().Inserir(poligono);

                poligono.Geojson = poligono.Geojson.Replace("\"PoligonoId\":\"-1\"", "\"PoligonoId\":\"" + poligono.Id + "\"");

                new PoligonoCore().Alterar(poligono);

                return poligono.Id;
            }
            else
            {
                var poligono = new PoligonoCore().RetornarPorId(PoligonoId);

                poligono.Geojson = Geojson;

                new PoligonoCore().Alterar(poligono);

                return poligono.Id;
            }
        }

        [HttpPost]
        public void DeletarPoligono(int PoligonoId)
        {
            var poligono = new PoligonoCore().RetornarPorId(PoligonoId);

            poligono.Ativo = false;

            new PoligonoCore().Alterar(poligono);
        }

        [HttpPost]
        public JsonResult AdicionarReferenciaEstante(int PoligonoId, int EstanteId)
        {
            var estante = new EstanteCore().RetornarPorId(EstanteId);
            var poligono = new PoligonoCore().RetornarPorId(PoligonoId);

            dynamic response = null;

            if (estante.ArmazemId == poligono.Camada.ArmazemId)
            {
                estante.PoligonoId = PoligonoId;

                new EstanteCore().Alterar(estante);

                response = new
                {
                    Id = EstanteId,
                    PoligonoId = PoligonoId,
                    QuantidadePrateleiras = estante.QuantidadePrateleiras
                };
            }

            var return_json = Json(response, JsonRequestBehavior.AllowGet);
            return_json.MaxJsonLength = int.MaxValue;
            return return_json;
        }

        [HttpPost]
        public void RemoverReferenciaEstante(int EstanteId)
        {
            var estante = new EstanteCore().RetornarPorId(EstanteId);

            estante.PoligonoId = null;

            new EstanteCore().Alterar(estante);
        }
    }
}