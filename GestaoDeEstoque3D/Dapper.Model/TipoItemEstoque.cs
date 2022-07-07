using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Model
{
    [Table("tbl_tipo_item_estoque")]
    public class TipoItemEstoque : IClasseBase
    {
        public IEntityMap Mappings { get; set; } = new TipoItemEstoqueMap();
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public double? Largura { get; set; }
        public double? Altura { get; set; }
        public double? Profundidade { get; set; }
        public double? Peso { get; set; }
        public double? PesoMaximoEmpilhamento { get; set; }
        public string CodigoDeBarras { get; set; }
        public int? UsuarioId { get; set; }
        public DateTime DataHora { get; set; }
        public bool Ativo { get; set; }


        public bool PossuiAssociacao { get; set; }
    }
}