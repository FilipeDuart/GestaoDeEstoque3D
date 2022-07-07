using Dapper;
using GestaoDeEstoque3D.Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestaoDeEstoque3D.Dapper.Core
{
    public abstract class TabelaBaseCore<T> where T : class, IClasseBase
    {
        public abstract T RetornarPorId(int id);

        public bool Inserir(T novoObjeto)
        {
            string tabela = "";

            try
            {
                tabela = typeof(T).GetCustomAttributesData().FirstOrDefault().ConstructorArguments.FirstOrDefault().Value.ToString();
            }
            catch (Exception)
            {
                return false;
            }

            string colunaId = "";
            string colunas = "";
            string valoresParam = "";
            var valoresDictionary = new Dictionary<string, object>();

            novoObjeto.Mappings.PropertyMaps.ToList().ForEach(pm =>
            {
                if (!pm.PropertyInfo.Name.Contains("READONLY"))
                {
                    if (pm.PropertyInfo.Name != "Id")
                    {
                        if (pm.PropertyInfo.PropertyType.Namespace == "NetTopologySuite.Geometries")
                        {
                            var geometria = pm.PropertyInfo.GetValue(novoObjeto);
                            string geometriaString = geometria == null ? null : geometria.ToString();

                            valoresDictionary.Add("@" + pm.PropertyInfo.Name, geometriaString);
                        }
                        else
                        {
                            valoresDictionary.Add("@" + pm.PropertyInfo.Name, pm.PropertyInfo.GetValue(novoObjeto));
                        }

                        colunas += pm.ColumnName + ", ";
                        valoresParam += "@" + pm.PropertyInfo.Name + ", ";
                    }
                    else
                    {
                        colunaId = pm.ColumnName;
                    }
                }
            });

            colunas = colunas.Substring(0, colunas.Length - 2);
            valoresParam = valoresParam.Substring(0, valoresParam.Length - 2);

            string sql = "insert into " + tabela + " (" + colunas + ") values (" + valoresParam + ") returning " + colunaId + ";";
            var parametros = new DynamicParameters(valoresDictionary);
            int idInserido = 0;

            using (var connection = DapperConnection.Create())
            {
                idInserido = connection.QuerySingle<int>(sql, parametros);
            }

            novoObjeto.Id = idInserido;

            return (idInserido != 0);
        }

        public bool Alterar(T objeto)
        {
            string tabela = "";

            try
            {
                tabela = typeof(T).GetCustomAttributesData().FirstOrDefault().ConstructorArguments.FirstOrDefault().Value.ToString();
            }
            catch (Exception)
            {
                return false;
            }

            string colunaId = "";
            string colunasSet = "";
            var valoresDictionary = new Dictionary<string, object>();

            objeto.Mappings.PropertyMaps.ToList().ForEach(pm =>
            {
                if (!pm.PropertyInfo.Name.Contains("READONLY"))
                {
                    if (pm.PropertyInfo.Name != "Id")
                    {
                        if (pm.PropertyInfo.PropertyType.Namespace == "NetTopologySuite.Geometries")
                        {
                            var geometria = pm.PropertyInfo.GetValue(objeto);
                            string geometriaString = geometria == null ? null : geometria.ToString();

                            valoresDictionary.Add("@" + pm.PropertyInfo.Name, geometriaString);
                        }
                        else
                        {
                            valoresDictionary.Add("@" + pm.PropertyInfo.Name, pm.PropertyInfo.GetValue(objeto));
                        }


                        colunasSet += pm.ColumnName + " = @" + pm.PropertyInfo.Name + ", ";
                    }
                    else
                    {
                        colunaId = pm.ColumnName;
                    }
                }
            });

            colunasSet = colunasSet.Substring(0, colunasSet.Length - 2);

            string sql = "update " + tabela + " set " + colunasSet + " where " + colunaId + " = " + objeto.Id + ";";
            var parametros = new DynamicParameters(valoresDictionary);
            int linhasAlteradas = 0;

            using (var connection = DapperConnection.Create())
            {
                linhasAlteradas = connection.Execute(sql, parametros);
            }

            return (linhasAlteradas > 0);
        }

        public bool Deletar(T objeto)
        {
            string tabela = "";

            try
            {
                tabela = typeof(T).GetCustomAttributesData().FirstOrDefault().ConstructorArguments.FirstOrDefault().Value.ToString();
            }
            catch (Exception)
            {
                return false;
            }

            string colunaId = "";

            objeto.Mappings.PropertyMaps.ToList().ForEach(pm =>
            {
                if (pm.PropertyInfo.Name == "Id")
                {
                    colunaId = pm.ColumnName;
                }
            });

            if (colunaId == "")
                return false;

            string sql = "delete from " + tabela + " where " + colunaId + " = " + objeto.Id + ";";
            int linhasAlteradas = 0;

            using (var connection = DapperConnection.Create())
            {
                linhasAlteradas = connection.Execute(sql);
            }

            return (linhasAlteradas > 0);
        }
    }
}