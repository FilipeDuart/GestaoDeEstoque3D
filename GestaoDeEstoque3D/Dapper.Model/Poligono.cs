using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Model
{
    [Table("tbl_poligono")]
    public class Poligono : IClasseBase
    {
        public IEntityMap Mappings { get; set; } = new PoligonoMap();
        public int Id { get; set; }
        public int CamadaId { get; set; }
        public string Geometria_READONLY { get; set; }
        public string Geojson { get; set; }
        public bool Ativo { get; set; }

        public Camada Camada { get; set; }
    }
}