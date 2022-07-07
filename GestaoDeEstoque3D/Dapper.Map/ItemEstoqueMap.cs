using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class ItemEstoqueMap : EntityMap<ItemEstoque>
    {
        public ItemEstoqueMap()
        {
            Map(p => p.Id)
                .ToColumn("ite_id");

            Map(p => p.UsuarioId)
                .ToColumn("ite_usu_id");

            Map(p => p.DataHora)
                .ToColumn("ite_data_hora");

            Map(p => p.Ativo)
                .ToColumn("ite_ativo");

            Map(p => p.TipoItemEstoqueId)
                .ToColumn("ite_tie_id");

            Map(p => p.PackX)
                .ToColumn("ite_pack_x");

            Map(p => p.PackY)
                .ToColumn("ite_pack_y");

            Map(p => p.PackZ)
                .ToColumn("ite_pack_z");

            Map(p => p.PrateleiraId)
                .ToColumn("ite_pra_id");
            
            Map(p => p.ItemBaseId)
                .ToColumn("ite_item_base_id");
        }
    }
}