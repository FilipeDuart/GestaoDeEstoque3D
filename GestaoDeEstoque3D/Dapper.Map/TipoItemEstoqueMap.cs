using Dapper.FluentMap.Mapping;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Map
{
    public class TipoItemEstoqueMap : EntityMap<TipoItemEstoque>
    {
        public TipoItemEstoqueMap()
        {
            Map(p => p.Id)
                .ToColumn("tie_id");

            Map(p => p.Nome)
                .ToColumn("tie_nome");

            Map(p => p.Descricao)
                .ToColumn("tie_descricao");

            Map(p => p.Largura)
                .ToColumn("tie_largura");

            Map(p => p.Altura)
                .ToColumn("tie_altura");

            Map(p => p.Profundidade)
                .ToColumn("tie_profundidade");

            Map(p => p.Peso)
                .ToColumn("tie_peso");

            Map(p => p.PesoMaximoEmpilhamento)
                .ToColumn("tie_peso_maximo_empilhamento");

            Map(p => p.CodigoDeBarras)
                .ToColumn("tie_codigo_de_barras");

            Map(p => p.UsuarioId)
                .ToColumn("tie_usu_id");

            Map(p => p.DataHora)
                .ToColumn("tie_data_hora");

            Map(p => p.Ativo)
                .ToColumn("tie_ativo");
        }
    }
}