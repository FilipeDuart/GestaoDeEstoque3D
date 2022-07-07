using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class CamadaMap : EntityMap<Camada>
    {
        public CamadaMap()
        {
            Map(p => p.Id)
                .ToColumn("cam_id");

            Map(p => p.Nome)
                .ToColumn("cam_nome");

            Map(p => p.Cor)
                .ToColumn("cam_cor");

            Map(p => p.Ativo)
                .ToColumn("cam_ativo");

            Map(p => p.ArmazemId)
                .ToColumn("cam_arm_id");
        }
    }
}