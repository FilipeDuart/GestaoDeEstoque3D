using Dapper.FluentMap;
using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public static class RegisterMappings
    {
        public static void Register()
        {
            FluentMapper.Initialize(config =>
            {
                //Maps Postgres <-> Dapper
                config.AddMap(new CamadaMap());
                config.AddMap(new EstanteMap());
                config.AddMap(new PoligonoMap());
                config.AddMap(new PrateleiraMap());
                config.AddMap(new SistemaConfiguracaoMap());
                config.AddMap(new TipoItemEstoqueMap());
                config.AddMap(new ItemEstoqueMap());
            });
        }
    }
}