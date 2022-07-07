using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Model
{
    [Table("tbl_estante")]
    public class Estante : IClasseBase
    {
        public IEntityMap Mappings { get; set; } = new EstanteMap();
        public int Id { get; set; }
        public int? QuantidadePrateleiras { get; set; }
        public double? LarguraPrateleiras { get; set; }
        public double? AlturaPrateleiras { get; set; }
        public double? ProfundidadePrateleiras { get; set; }
        public double? PesoMaximoPrateleiras { get; set; }
        public int? PoligonoId { get; set; }
        public int? UsuarioId { get; set; }
        public bool Ativo { get; set; }
        public int? ArmazemId { get; set; }
        public double? AncoragemLat { get; set; }
        public double? AncoragemLng { get; set; }

        public bool Associado { get; set; }
        public List<Prateleira> Prateleiras { get; set; }
    }
}