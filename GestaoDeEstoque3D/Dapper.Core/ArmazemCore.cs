using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public class CArmazemCore : TabelaBaseCore<Armazem>
    {
        public List<Armazem> RetornarTodos()
        {
            List<Armazem> Armazem;
            using (var connection = DapperConnection.Create())
            {
                Armazem = connection.Query<Armazem>(
                    @"select * from tbl_armazem arm
                      where arm_ativo is true
                      order by arm_nome"
                ).ToList();
            }

            return Armazem;
        }

        public override Armazem RetornarPorId(int id)
        {
            Armazem Armazem;
            using (var connection = DapperConnection.Create())
            {
                Armazem = connection.Query<Armazem>(
                    @"select * from tbl_armazem arm
                      where arm_id = @id
                      limit 1",
                    param: new { id }
                ).FirstOrDefault();
            }

            return Armazem;
        }
    }
}