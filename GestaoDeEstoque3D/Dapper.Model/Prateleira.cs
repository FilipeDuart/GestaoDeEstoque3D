using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Model
{
    [Table("tbl_prateleira")]
    public class Prateleira : IClasseBase
    {
        public IEntityMap Mappings { get; set; } = new PrateleiraMap();
        public int Id { get; set; }
        public int EstanteId { get; set; }
        public int Nivel { get; set; }

        public Estante Estante { get; set; }
        public List<ItemEstoque> ItemsEstoque { get; set; }
    }
}