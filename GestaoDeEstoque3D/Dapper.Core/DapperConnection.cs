using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public static class DapperConnection
    {
        public static string ConnectionStringDapper { get; } = System.Configuration.ConfigurationManager.AppSettings["ConnectionStringPostgres"];

        public static NpgsqlConnection Create()
        {
            return new NpgsqlConnection(ConnectionStringDapper);
        }
    }
}