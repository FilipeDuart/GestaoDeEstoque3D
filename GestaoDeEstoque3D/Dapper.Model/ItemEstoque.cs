using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Model
{
    [Table("tbl_item_estoque")]
    public class ItemEstoque : IClasseBase
    {
        public IEntityMap Mappings { get; set; } = new ItemEstoqueMap();
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public int? TipoItemEstoqueId { get; set; }
        public DateTime DataHora { get; set; }
        public bool Ativo { get; set; }
        public double PackX { get; set; }
        public double PackY { get; set; }
        public double PackZ { get; set; }
        public int? PrateleiraId { get; set; }
        public int? ItemBaseId { get; set; }

        public TipoItemEstoque TipoItemEstoque { get; set; }
    }
}