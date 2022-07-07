using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class EstanteMap : EntityMap<Estante>
    {
        public EstanteMap()
        {
            Map(p => p.Id)
                .ToColumn("est_id");

            Map(p => p.QuantidadePrateleiras)
                .ToColumn("est_quantidade_prateleiras");

            Map(p => p.LarguraPrateleiras)
                .ToColumn("est_prateleira_largura");

            Map(p => p.AlturaPrateleiras)
                .ToColumn("est_prateleira_altura");

            Map(p => p.ProfundidadePrateleiras)
                .ToColumn("est_prateleira_profundidade");

            Map(p => p.PesoMaximoPrateleiras)
                .ToColumn("est_prateleira_peso_maximo");

            Map(p => p.PoligonoId)
                .ToColumn("est_pol_id");

            Map(p => p.UsuarioId)
                .ToColumn("est_usu_id");

            Map(p => p.Ativo)
                .ToColumn("est_ativo");

            Map(p => p.ArmazemId)
                .ToColumn("est_arm_id");

            Map(p => p.AncoragemLat)
                .ToColumn("est_ancoragem_lat");

            Map(p => p.AncoragemLng)
                .ToColumn("est_ancoragem_lng");
        }
    }
}