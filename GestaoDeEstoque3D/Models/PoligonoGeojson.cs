using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Models
{
    public class PoligonoGeojson
    {
        public string type { get; set; }
        public PoligonoGeojsonProperties properties { get; set; }
        public PoligonoGeojsonGeometry geometry { get; set; }
    }

    public class PoligonoGeojsonProperties
    {
        public string PoligonoId { get; set; }
        public string CamadaId { get; set; }
        public string CamadaNome { get; set; }
    }

    public class PoligonoGeojsonGeometry
    {
        public string type { get; set; }
        public List<List<List<double>>> coordinates { get; set; }
    }



}