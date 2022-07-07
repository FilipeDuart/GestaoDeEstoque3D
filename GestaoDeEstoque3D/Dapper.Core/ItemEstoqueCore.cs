using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public class ItemEstoqueCore : TabelaBaseCore<ItemEstoque>
    {
        public ItemEstoque RetornarUltimoAssociadoPorTipoId(int TipoItemEstoqueId)
        {
            ItemEstoque ItemsEstoque = null;
            using (var connection = DapperConnection.Create())
            {
                ItemsEstoque = connection.Query<ItemEstoque, TipoItemEstoque, ItemEstoque>(
                    @"select * from tbl_item_estoque ite
                      inner join tbl_tipo_item_estoque tie on tie_id = ite_tie_id
                      where ite_tie_id = @TipoItemEstoqueId
                      and ite_pra_id is not null 
                      order by ite_id desc
                      limit 1",
                    (ITE, TIE) =>
                    {
                        ITE.TipoItemEstoque = TIE;

                        return ITE;
                    },
                    param: new { TipoItemEstoqueId },
                    splitOn: "ite_id, tie_id"
                ).FirstOrDefault();
            }

            return ItemsEstoque;
        }

        public List<ItemEstoque> RetornarTodos()
        {
            List<ItemEstoque> ItemEstoque;
            using (var connection = DapperConnection.Create())
            {
                ItemEstoque = connection.Query<ItemEstoque, TipoItemEstoque, ItemEstoque>(
                    @"select * from tbl_item_estoque ite
                      inner join tbl_tipo_item_estoque tie on tie_id = ite_tie_id
                      where ite_ativo is true",
                    (ITE, TIE) =>
                    {
                        ITE.TipoItemEstoque = TIE;

                        return ITE;
                    },
                    splitOn: "ite_id, tie_id"
                ).ToList();
            }

            return ItemEstoque;
        }
        
        public List<ItemEstoque> RetornarNaoEstocados()
        {
            List<ItemEstoque> ItemEstoque;
            using (var connection = DapperConnection.Create())
            {
                ItemEstoque = connection.Query<ItemEstoque, TipoItemEstoque, ItemEstoque>(
                    @"select * from tbl_item_estoque ite
                      inner join tbl_tipo_item_estoque tie on tie_id = ite_tie_id
                      where ite_ativo is true and ite_pra_id is null",
                    (ITE, TIE) =>
                    {
                        ITE.TipoItemEstoque = TIE;

                        return ITE;
                    },
                    splitOn: "ite_id, tie_id"
                ).ToList();
            }

            return ItemEstoque;
        }

        public List<ItemEstoque> RetornarPorPrateleiraId(int prateleiraId)
        {
            var ItemsEstoque = new List<ItemEstoque>();
            using (var connection = DapperConnection.Create())
            {
                ItemsEstoque = connection.Query<ItemEstoque, TipoItemEstoque, ItemEstoque>(
                    @"select * from tbl_item_estoque ite
                      inner join tbl_tipo_item_estoque tie on tie_id = ite_tie_id
                      where ite_pra_id = @prateleiraId",
                    (ITE, TIE) =>
                    {
                        ITE.TipoItemEstoque = TIE;

                        return ITE;
                    },
                    param: new { prateleiraId },
                    splitOn: "ite_id, tie_id"
                ).ToList();
            }

            return ItemsEstoque;
        }

        public override ItemEstoque RetornarPorId(int id)
        {
            ItemEstoque ItemEstoque;
            using (var connection = DapperConnection.Create())
            {
                ItemEstoque = connection.Query<ItemEstoque, TipoItemEstoque, ItemEstoque>(
                    @"select * from tbl_item_estoque ite
                      inner join tbl_tipo_item_estoque tie on tie_id = ite_tie_id
                      where ite_id = @id
                      limit 1",
                    (ITE, TIE) =>
                    {
                        ITE.TipoItemEstoque = TIE;

                        return ITE;
                    },
                    param: new { id },
                    splitOn: "ite_id, tie_id"
                ).FirstOrDefault();
            }

            return ItemEstoque;
        }
    }
}