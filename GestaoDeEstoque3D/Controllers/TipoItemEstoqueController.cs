using GestaoDeEstoque3D.Dapper.Core;
using GestaoDeEstoque3D.Dapper.Model;
using GestaoDeEstoque3D.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestaoDeEstoque3D.Controllers
{
    public class TipoItemEstoqueController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RetornarTiposItemEstoque(bool ApenasAssociados = false)
        {
            List<TipoItemEstoque> tiposItemEstoque;

            if(ApenasAssociados)
                tiposItemEstoque = new TipoItemEstoqueCore().RetornarTodosAssociados();
            else
                tiposItemEstoque = new TipoItemEstoqueCore().RetornarTodos();

            var response = tiposItemEstoque.Select(tpi => new
            {
                Id = tpi.Id,
                Nome = tpi.Nome,
                Descricao = tpi.Descricao,
                Largura = tpi.Largura,
                Altura = tpi.Altura,
                Profundidade = tpi.Profundidade,
                Peso = tpi.Peso,
                PesoMaximoEmpilhamento = tpi.PesoMaximoEmpilhamento,
                CodigoDeBarras = tpi.CodigoDeBarras,
                PossuiAssociacao = tpi.PossuiAssociacao,
            });

            var return_json = Json(response, JsonRequestBehavior.AllowGet);
            return_json.MaxJsonLength = int.MaxValue;
            return return_json;
        }

        public JsonResult CadastrarTipoItemEstoque(string JsonTipoItemEstoque)
        {
            var definition = new
            {
                Nome = "",
                Descricao = "",
                Largura = "",
                Altura = "",
                Profundidade = "",
                Peso = "",
                PesoMaximoEmpilhamento = "",
                CodigoDeBarras = "",
            };

            var jsonTipoItemEstoque = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(JsonTipoItemEstoque, definition);

            var tipoItemEstoque = new TipoItemEstoque()
            {
                Nome = jsonTipoItemEstoque.Nome,
                Descricao = jsonTipoItemEstoque.Descricao,
                Largura = Convert.ToDouble(jsonTipoItemEstoque.Largura.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US")),
                Altura = Convert.ToDouble(jsonTipoItemEstoque.Altura.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US")),
                Profundidade = Convert.ToDouble(jsonTipoItemEstoque.Profundidade.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US")),
                Peso = Convert.ToDouble(jsonTipoItemEstoque.Peso.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US")),
                PesoMaximoEmpilhamento = Convert.ToDouble(jsonTipoItemEstoque.PesoMaximoEmpilhamento.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US")),
                CodigoDeBarras = jsonTipoItemEstoque.CodigoDeBarras,
                UsuarioId = null,
                DataHora = DateTime.Now,
                Ativo = true,
            };

            new TipoItemEstoqueCore().Inserir(tipoItemEstoque);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditarTipoItemEstoque(string JsonTipoItemEstoque)
        {
            var definition = new
            {
                Id = new int(),
                Nome = "",
                Descricao = "",
                Largura = "",
                Altura = "",
                Profundidade = "",
                Peso = "",
                PesoMaximoEmpilhamento = "",
                CodigoDeBarras = "",
            };

            var jsonTipoItemEstoque = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(JsonTipoItemEstoque, definition);

            var core = new TipoItemEstoqueCore();

            var TipoItemEstoque = core.RetornarPorId(jsonTipoItemEstoque.Id);

            TipoItemEstoque.Nome = jsonTipoItemEstoque.Nome;
            TipoItemEstoque.Descricao = jsonTipoItemEstoque.Descricao;
            TipoItemEstoque.Largura = Convert.ToDouble(jsonTipoItemEstoque.Largura.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
            TipoItemEstoque.Altura = Convert.ToDouble(jsonTipoItemEstoque.Altura.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
            TipoItemEstoque.Profundidade = Convert.ToDouble(jsonTipoItemEstoque.Profundidade.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
            TipoItemEstoque.Peso = Convert.ToDouble(jsonTipoItemEstoque.Peso.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
            TipoItemEstoque.PesoMaximoEmpilhamento = Convert.ToDouble(jsonTipoItemEstoque.PesoMaximoEmpilhamento.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US"));
            TipoItemEstoque.CodigoDeBarras = jsonTipoItemEstoque.CodigoDeBarras;
            TipoItemEstoque.UsuarioId = null;
            TipoItemEstoque.DataHora = DateTime.Now;
            TipoItemEstoque.Ativo = true;

            core.Alterar(TipoItemEstoque);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletarTipoItemEstoque(int TipoItemEstoqueId)
        {
            var core = new TipoItemEstoqueCore();

            var TipoItemEstoque = core.RetornarPorId(TipoItemEstoqueId);

            TipoItemEstoque.Ativo = false;

            core.Alterar(TipoItemEstoque);
            
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletarItem(int ItemEstoqueId)
        {
            var core = new ItemEstoqueCore();

            var ItemEstoque = core.RetornarPorId(ItemEstoqueId);

            core.Deletar(ItemEstoque);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetornarNaoEstocados()
        {
            var itensDeEstoque = new ItemEstoqueCore().RetornarNaoEstocados();

            var response = itensDeEstoque.GroupBy(ite => ite.TipoItemEstoqueId).Select(ite => ite.First()).Select(ite => new {
                Id = ite.Id,
                TipoItemEstoqueId = ite.TipoItemEstoqueId,
                Nome = ite.TipoItemEstoque.Nome,
                Descricao = ite.TipoItemEstoque.Descricao,
                Largura = ite.TipoItemEstoque.Largura,
                Altura = ite.TipoItemEstoque.Altura,
                Profundidade = ite.TipoItemEstoque.Profundidade,
                Peso = ite.TipoItemEstoque.Peso,
                PesoMaximoEmpilhamento = ite.TipoItemEstoque.PesoMaximoEmpilhamento,
                CodigoDeBarras = ite.TipoItemEstoque.CodigoDeBarras,
                Quantidade = itensDeEstoque.Where(ite2 => ite2.TipoItemEstoqueId == ite.TipoItemEstoqueId).Count(),
            });

            var return_json = Json(response, JsonRequestBehavior.AllowGet);
            return_json.MaxJsonLength = int.MaxValue;
            return return_json;
        }
    }
}