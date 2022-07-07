using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class PoligonoMap : EntityMap<Poligono>
    {
        public PoligonoMap()
        {
            Map(p => p.Id)
                .ToColumn("pol_id");

            Map(p => p.CamadaId)
                .ToColumn("pol_cam_id");

            //Map(p => p.Geometria_READONLY)
            //    .ToColumn("pol_geometria");

            Map(p => p.Geojson)
                .ToColumn("pol_geojson");

            Map(p => p.Ativo)
                .ToColumn("pol_ativo");
        }
    }
}