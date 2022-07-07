using CsvHelper;
using GestaoDeEstoque3D.Dapper.Core;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GestaoDeEstoque3D.Controllers
{
    public class RelatorioController : Controller
    {
        public FileContentResult GerarRelatorioCompleto(bool excel = false)
        {
            var linhas = new List<object>();

            //Adiciona os itens não estocados
            var itensNaoEstocados = new ItemEstoqueCore().RetornarNaoEstocados();

            foreach(var itemNaoEstocados in itensNaoEstocados.GroupBy(ite => ite.TipoItemEstoqueId).Select(ite => ite.First()))
            {
                var item = new
                {
                    _EstanteId = "N/A",
                    _Nivel = "N/A",
                    //Id = itemNaoEstocados.Id,
                    _TipoItemEstoqueId = itemNaoEstocados.TipoItemEstoqueId,
                    Nome = itemNaoEstocados.TipoItemEstoque.Nome,
                    _Descricao = itemNaoEstocados.TipoItemEstoque.Descricao,
                    Largura = itemNaoEstocados.TipoItemEstoque.Largura,
                    Altura = itemNaoEstocados.TipoItemEstoque.Altura,
                    Profundidade = itemNaoEstocados.TipoItemEstoque.Profundidade,
                    Peso = itemNaoEstocados.TipoItemEstoque.Peso,
                    _PesoMaximoEmpilhamento = itemNaoEstocados.TipoItemEstoque.PesoMaximoEmpilhamento,
                    _CodigoDeBarras = itemNaoEstocados.TipoItemEstoque.CodigoDeBarras,
                    Quantidade = itensNaoEstocados.Where(ite2 => ite2.TipoItemEstoqueId == itemNaoEstocados.TipoItemEstoqueId).Count()
                };

                linhas.Add(item);
            }

            //Adiciona os itens estocados
            var estantes = new EstanteCore().RetornarTodosComItens();

            foreach (var estante in estantes.OrderBy(est => est.Id))
            {
                foreach (var prateleira in estante.Prateleiras.OrderBy(pra => pra.Nivel))
                {
                    foreach (var itemEstoque in prateleira.ItemsEstoque.GroupBy(ite => ite.TipoItemEstoqueId).Select(ite => ite.First()))
                    {
                        var item = new
                        {
                            _EstanteId = estante.Id.ToString(),
                            _Nivel = prateleira.Nivel.ToString(),
                            //Id = itemEstoque.Id,
                            _TipoItemEstoqueId = itemEstoque.TipoItemEstoqueId,
                            Nome = itemEstoque.TipoItemEstoque.Nome,
                            _Descricao = itemEstoque.TipoItemEstoque.Descricao,
                            Largura = itemEstoque.TipoItemEstoque.Largura,
                            Altura = itemEstoque.TipoItemEstoque.Altura,
                            Profundidade = itemEstoque.TipoItemEstoque.Profundidade,
                            Peso = itemEstoque.TipoItemEstoque.Peso,
                            _PesoMaximoEmpilhamento = itemEstoque.TipoItemEstoque.PesoMaximoEmpilhamento,
                            _CodigoDeBarras = itemEstoque.TipoItemEstoque.CodigoDeBarras,
                            Quantidade = prateleira.ItemsEstoque.Where(ite2 => ite2.TipoItemEstoqueId == itemEstoque.TipoItemEstoqueId).Count()
                        };

                        linhas.Add(item);
                    }
                }
            }

            string csvContent = "";

            if (linhas.Count > 0)
            {
                var expandos = JsonConvert.DeserializeObject<ExpandoObject[]>(SubstituirNomesDePropriedades(JsonConvert.SerializeObject(linhas)));

                using (var writer = new StringWriter())
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("pt-BR")))
                    {
                        //csv.Configuration.Delimiter = delimiter;

                        csv.WriteRecords(expandos as IEnumerable<dynamic>);
                    }

                    csvContent = writer.ToString();
                }
            }

            var data = Encoding.UTF8.GetBytes(csvContent);
            var result = Encoding.UTF8.GetPreamble().Concat(data).ToArray();

            if (excel)
            {
                result = System.IO.File.ReadAllBytes(CSV2Excel(result));
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Relatório - Armazém 3D.xlsx");
            }
            else
            {
                return File(result, "application/csv", "Relatório - Armazém 3D.csv");
            }
        }

        private string CSV2Excel(byte[] fileContents)
        {
            Directory.CreateDirectory(Server.MapPath("~/CSV2Excel"));

            string csvFileName = Server.MapPath("~/CSV2Excel/" + @"CSV2Excel_CSV_" + DateTime.Now.ToFileTimeUtc() + ".csv");

            System.IO.File.WriteAllBytes(csvFileName, fileContents);

            string excelFileName = Server.MapPath("~/CSV2Excel/" + @"CSV2Excel_Excel_" + DateTime.Now.ToFileTimeUtc() + ".xlsx");

            string worksheetsName = "Armazém 3D";

            bool firstRowIsHeader = true;

            var format = new ExcelTextFormat();
            format.Delimiter = ';';
            format.EOL = "\r"; // DEFAULT IS "\r\n";
            format.SkipLinesEnd = 1; //Remove ultima linha (essa ultima fica em branco)
            // format.TextQualifier = '"';

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                package.Save();
            }

            return excelFileName;
        }

        private string SubstituirNomesDePropriedades(string json)
        {
            json = json.Replace("_EstanteId", "Cód Estante")
                       .Replace("_Nivel", "Nível")
                       .Replace("_TipoItemEstoqueId", "Cód Tipo Item de Estoque")
                       .Replace("_Descricao", "Descrição")
                       .Replace("_PesoMaximoEmpilhamento", "Peso Máximo para Empilhamento")
                       .Replace("_CodigoDeBarras", "Código de Barras");

            return json;
        }
    }
}