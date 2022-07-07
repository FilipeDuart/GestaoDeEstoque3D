using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public class EstanteCore : TabelaBaseCore<Estante>
    {
        public List<Estante> RetornarTodos(int ArmazemId = 1)
        {
            List<Estante> Estante;
            using (var connection = DapperConnection.Create())
            {
                Estante = connection.Query<Estante, int?, Estante>(
                    @"select est.*, pol_id from tbl_estante est
                      left join tbl_poligono pol on est_pol_id = pol_id and pol_ativo is false
                      where est_ativo is true and est_arm_id = @ArmazemId
                      order by est_id desc",
                    (EST, Associado) =>
                    {
                        EST.Associado = Associado == null;

                        return EST;
                    },
                    new { ArmazemId },
                    splitOn: "est_id, pol_id"
                ).ToList();
            }

            return Estante;
        }

        public Estante RetornarComItens(int ArmazemId = 1, int EstanteId = -1)
        {
            List<Estante> Estante = new List<Estante>();
            using (var connection = DapperConnection.Create())
            {
                connection.Query<Estante, Prateleira, Estante>(
                    @"select * from tbl_estante est
                      inner join tbl_prateleira pra on pra_est_id = est_id
                      where est_ativo is true and est_arm_id = @ArmazemId and est_id = @EstanteId",
                    (EST, PRA) =>
                    {
                        var estante = Estante.Where(est => est.Id == EST.Id).FirstOrDefault() ?? EST;

                        if (estante.Prateleiras == null)
                            estante.Prateleiras = new List<Prateleira>();

                        PRA.Estante = estante;

                        estante.Prateleiras.Add(PRA);

                        if (!Estante.Exists(est => est.Id == EST.Id))
                            Estante.Add(EST);

                        return EST;
                    },
                    new { ArmazemId, EstanteId },
                    splitOn: "est_id, pra_id"
                ).ToList();
            }

            var itemEstoqueCore = new ItemEstoqueCore();
            Parallel.ForEach(Estante, estante =>
            {
                Parallel.ForEach(estante.Prateleiras, prateleira =>
                {
                    prateleira.ItemsEstoque = itemEstoqueCore.RetornarPorPrateleiraId(prateleira.Id);
                });
            });

            return Estante.FirstOrDefault();
        }

        public List<Estante> RetornarTodosComItens(int ArmazemId = 1)
        {
            List<Estante> Estante = new List<Estante>();
            using (var connection = DapperConnection.Create())
            {
                connection.Query<Estante, Prateleira, Estante>(
                    @"select * from tbl_estante est
                      inner join tbl_prateleira pra on pra_est_id = est_id
                      where est_ativo is true and est_arm_id = @ArmazemId",
                    (EST, PRA) =>
                    {
                        var estante = Estante.Where(est => est.Id == EST.Id).FirstOrDefault() ?? EST;

                        if (estante.Prateleiras == null)
                            estante.Prateleiras = new List<Prateleira>();

                        PRA.Estante = estante;

                        estante.Prateleiras.Add(PRA);

                        if (!Estante.Exists(est => est.Id == EST.Id))
                            Estante.Add(EST);

                        return EST;
                    },
                    new { ArmazemId },
                    splitOn: "est_id, pra_id"
                ).ToList();
            }

            var itemEstoqueCore = new ItemEstoqueCore();
            Parallel.ForEach(Estante, estante =>
            {
                Parallel.ForEach(estante.Prateleiras, prateleira =>
                {
                    prateleira.ItemsEstoque = itemEstoqueCore.RetornarPorPrateleiraId(prateleira.Id);
                });
            });

            return Estante;
        }

        public List<Estante> RetornarEstantesNaoPresentesNoMapa(int ArmazemId = 1)
        {
            List<Estante> Estante;
            using (var connection = DapperConnection.Create())
            {
                Estante = connection.Query<Estante>(
                    @"select * from tbl_estante est
                      inner join tbl_poligono pol on est_pol_id = pol_id and pol_ativo is false
                      where est_ativo is true and est_arm_id = @ArmazemId",
                    new { ArmazemId }
                ).ToList();
            }

            return Estante;
        }

        public List<Estante> RetornarEstantesAssociadas()
        {
            List<Estante> Estante = new List<Estante>();
            using (var connection = DapperConnection.Create())
            {
                connection.Query<Estante, Prateleira, Estante>(
                    @"select * from tbl_estante est
                      inner join tbl_prateleira pra on pra_est_id = est_id
                      where est_ativo is true and est_pol_id is not null",
                    (EST, PRA) =>
                    {
                        var estante = Estante.Where(est => est.Id == EST.Id).FirstOrDefault() ?? EST;

                        if (estante.Prateleiras == null)
                            estante.Prateleiras = new List<Prateleira>();

                        PRA.Estante = estante;

                        estante.Prateleiras.Add(PRA);

                        if (!Estante.Exists(est => est.Id == EST.Id))
                            Estante.Add(EST);

                        return EST;
                    },
                    null,
                    splitOn: "est_id, pra_id"
                ).ToList();
            }

            return Estante;
        }

        public override Estante RetornarPorId(int id)
        {
            Estante Estante;
            using (var connection = DapperConnection.Create())
            {
                Estante = connection.Query<Estante>(
                    @"select * from tbl_estante est
                      where est_id = @id
                      limit 1",
                    param: new { id }
                ).FirstOrDefault();
            }

            return Estante;
        }
    }
}