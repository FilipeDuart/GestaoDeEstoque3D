using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Model
{
    [Table("tbl_armazem")]
    public class Armazem : IClasseBase
    {
        public IEntityMap Mappings { get; set; } = new ArmazemMap();
        public int Id { get; set; }
        public string Nome { get; set; }
        public int? PoligonoId { get; set; }
        public bool Ativo { get; set; }
    }
}