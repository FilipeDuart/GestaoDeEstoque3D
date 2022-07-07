using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class SistemaConfiguracaoMap : EntityMap<SistemaConfiguracao>
    {
        public SistemaConfiguracaoMap()
        {
            Map(p => p.Id)
                .ToColumn("sic_id");

            Map(p => p.Nome)
                .ToColumn("sic_nome");

            Map(p => p.Valor)
                .ToColumn("sic_valor");

            Map(p => p.Ativo)
                .ToColumn("sic_ativo");
        }
    }
}