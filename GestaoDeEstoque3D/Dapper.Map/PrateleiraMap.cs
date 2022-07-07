using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class PrateleiraMap : EntityMap<Prateleira>
    {
        public PrateleiraMap()
        {
            Map(p => p.Id)
                .ToColumn("pra_id");

            Map(p => p.EstanteId)
                .ToColumn("pra_est_id");

            Map(p => p.Nivel)
                .ToColumn("pra_nivel");
        }
    }
}