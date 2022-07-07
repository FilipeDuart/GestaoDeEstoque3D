using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public class SistemaConfiguracaoCore : TabelaBaseCore<SistemaConfiguracao>
    {
        public List<SistemaConfiguracao> RetornarTodos()
        {
            List<SistemaConfiguracao> SistemaConfiguracao;
            using (var connection = DapperConnection.Create())
            {
                SistemaConfiguracao = connection.Query<SistemaConfiguracao>(
                    @"select * from tbl_sistema_configuracao sic
                      where sic_ativo = true
                      order by sic_nome"
                ).ToList();
            }

            return SistemaConfiguracao;
        }

        public SistemaConfiguracao RetornarPorNome(string nome)
        {
            SistemaConfiguracao SistemaConfiguracao;
            using (var connection = DapperConnection.Create())
            {
                SistemaConfiguracao = connection.Query<SistemaConfiguracao>(
                    @"select * from tbl_sistema_configuracao sic
                      where lower(sic_nome) = @nome and sic_ativo = true
                      limit 1",
                    param: new { nome = nome.ToLower() }
                ).FirstOrDefault();
            }

            return SistemaConfiguracao;
        }

        public override SistemaConfiguracao RetornarPorId(int id)
        {
            SistemaConfiguracao SistemaConfiguracao;
            using (var connection = DapperConnection.Create())
            {
                SistemaConfiguracao = connection.Query<SistemaConfiguracao>(
                    @"select * from tbl_sistema_configuracao sic
                      where sic_id = @id
                      limit 1",
                    param: new { id }
                ).FirstOrDefault();
            }

            return SistemaConfiguracao;
        }
    }
}