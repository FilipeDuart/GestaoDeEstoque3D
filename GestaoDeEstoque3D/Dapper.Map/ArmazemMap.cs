using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class ArmazemMap : EntityMap<Armazem>
    {
        public ArmazemMap()
        {
            Map(p => p.Id)
                .ToColumn("arm_id");

            Map(p => p.Nome)
                .ToColumn("arm_nome");

            Map(p => p.PoligonoId)
                .ToColumn("arm_pol_id");

            Map(p => p.Ativo)
                .ToColumn("arm_ativo");
        }
    }
}