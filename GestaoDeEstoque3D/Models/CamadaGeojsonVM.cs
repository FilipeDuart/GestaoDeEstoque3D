using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Models
{
    public class CamadaGeojsonVM
    {
        public int CamadaId { get; set; }
        public string CamadaNome { get; set; }
        public string CamadaGeojson { get; set; }
        public string CamadaCor { get; set; }
    }
}