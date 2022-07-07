using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public class CamadaCore : TabelaBaseCore<Camada>
    {
        public List<Camada> RetornarTodos()
        {
            List<Camada> Camada;
            using (var connection = DapperConnection.Create())
            {
                Camada = connection.Query<Camada>(
                    @"select * from tbl_camada cam
                      where cam_ativo is true
                      order by cam_nome"
                ).ToList();
            }

            return Camada;
        }

        public Camada RetornarPorCamadaNomeArmazemId(string CamadaNome, int ArmazemId)
        {
            Camada Camada;
            using (var connection = DapperConnection.Create())
            {
                Camada = connection.Query<Camada>(
                    @"select * from tbl_camada cam
                      where cam_nome = @CamadaNome and cam_arm_id = @ArmazemId
                      limit 1",
                    param: new { CamadaNome, ArmazemId }
                ).FirstOrDefault();
            }

            return Camada;
        }

        public override Camada RetornarPorId(int id)
        {
            Camada Camada;
            using (var connection = DapperConnection.Create())
            {
                Camada = connection.Query<Camada>(
                    @"select * from tbl_camada cam
                      where cam_id = @id
                      limit 1",
                    param: new { id }
                ).FirstOrDefault();
            }

            return Camada;
        }
    }
}