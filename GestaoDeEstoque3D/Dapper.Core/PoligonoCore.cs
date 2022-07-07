using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public class PoligonoCore : TabelaBaseCore<Poligono>
    {
        public List<Poligono> RetornarTodos()
        {
            List<Poligono> Poligonos;
            using (var connection = DapperConnection.Create())
            {
                Poligonos = connection.Query<Poligono, Camada, Poligono>(
                    @"select * from tbl_poligono pol
                      inner join tbl_camada cam on cam_id = pol_cam_id and cam_ativo is true
                      where pol_ativo = true
                      order by cam_nome",
                    (POL, CAM) =>
                    {
                        POL.Camada = CAM;

                        return POL;
                    }, 
                    null,
                    splitOn: "pol_id, cam_id"
                ).ToList();
            }

            return Poligonos;
        }

        public List<Poligono> RetornarPorCamadaId(int CamadaId)
        {
            List<Poligono> Poligonos;
            using (var connection = DapperConnection.Create())
            {
                Poligonos = connection.Query<Poligono, Camada, Poligono>(
                    @"select * from tbl_poligono pol
                      inner join tbl_camada cam on cam_id = pol_cam_id
                      where pol_cam_id = @CamadaId and pol_ativo is true",
                    (POL, CAM) =>
                    {
                        POL.Camada = CAM;

                        return POL;
                    },
                    param: new { CamadaId },
                    splitOn: "pol_id, cam_id"
                ).ToList();
            }

            return Poligonos;
        }

        public override Poligono RetornarPorId(int id)
        {
            Poligono Poligono;
            using (var connection = DapperConnection.Create())
            {
                Poligono = connection.Query<Poligono, Camada, Poligono>(
                    @"select * from tbl_poligono pol
                      inner join tbl_camada cam on cam_id = pol_cam_id
                      where pol_id = @id
                      limit 1",
                    (POL, CAM) =>
                    {
                        POL.Camada = CAM;

                        return POL;
                    },
                    param: new { id },
                    splitOn: "pol_id, cam_id"
                ).FirstOrDefault();
            }

            return Poligono;
        }
    }
}