using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public class PrateleiraCore : TabelaBaseCore<Prateleira>
    {
        public override Prateleira RetornarPorId(int id)
        {
            Prateleira Prateleira;
            using (var connection = DapperConnection.Create())
            {
                Prateleira = connection.Query<Prateleira>(
                    @"select * from tbl_prateleira pra
                      where pra_id = @id
                      limit 1",
                    param: new { id }
                ).FirstOrDefault();
            }

            return Prateleira;
        }

        public List<Prateleira> RetornarPorEstanteId(int estanteId)
        {
            List<Prateleira> Prateleiras = new List<Prateleira>();
            using (var connection = DapperConnection.Create())
            {
                Prateleiras = connection.Query<Prateleira>(
                    @"select * from tbl_prateleira pra
                      where pra_est_id = @estanteId",
                    param: new { estanteId }
                ).ToList();
            }

            return Prateleiras;
        }
    }
}