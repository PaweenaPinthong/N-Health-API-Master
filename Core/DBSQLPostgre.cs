using N_Health_API.Models;
using Npgsql;
using System.Data;

namespace N_Health_API.Core
{
    class DBSQLPostgre
    {
        //private static string connString = "Server=localhost;Database=roof_truss_boq_service;Port=8888;User Id=roof_truss_boq_service;Password=LEGKDryAV8p8QQhY";
        private static string connString = "Server=pg-25b7556e-paweenapinthong-fe13.i.aivencloud.com;Database=nhealth_api;Port=10460;User Id=avnadmin;";
        public DBSQLPostgre()
        {
            LoadParam();
        }
        private static void LoadParam() 
        {
            ParameterConfig paramCon = new ParameterConfig();
            paramCon.LoadParam();

            connString = "Server="+ paramCon.AWS_DB_HOST+ "; Database=" + paramCon.AWS_DB_NAME + ";User Id=" + paramCon.AWS_DB_USER + ";Password=" + paramCon.AWS_DB_PASS + "";
        }

        private static string GenerateId(string id)
        {
            string substringID = id.Substring(1, 10);
            var lastAddedId = Convert.ToInt64(substringID);
            string roofid = Convert.ToString(lastAddedId + 1).PadLeft(10, '0');
            return roofid;
        }

        public static DataTable SQLPostgresSelectCommand(string tableName,string column = "", string where = "",string order = "",string limit = "")
        {
            DataTable result = new DataTable();
            try
            {
                string sql;
                string selectColumn;
                string prefixTable = " from public." + tableName;

                selectColumn = (column == string.Empty) ? ("select *") : ("select " + column);
                where = (where != string.Empty) ? " where " + where : "";
                order = (order != string.Empty) ? " order by " + order : "";
                limit = (limit != string.Empty) ? " limit " + limit : "";

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    sql = selectColumn + prefixTable + where + order + limit;

                    using (var command = new NpgsqlCommand(sql, conn))
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        result.Load(reader);
                        reader.Close();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in {0}: {1}", Helper.GetCurrentMethod(), ex.Message);
            }
            return result;
        }

        public static DataTable SQLPostgresSelectCommand(string sql)
        {
            DataTable result = new DataTable();
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (var command = new NpgsqlCommand(sql, conn))
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        result.Load(reader);
                        reader.Close();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in {0}: {1}", Helper.GetCurrentMethod(), ex.Message);
            }
            return result;
        }

        public static long SQLPostgresExecuteScalar(string tableName, string column = "", string where = "", string order = "", string limit = "")
        {
            long result = 0;
            try
            {
                string sql;
                string selectColumn;
                string prefixTable = " from public." + tableName;

                selectColumn = (column == string.Empty) ? ("select *") : ("select " + column);
                where = (where != string.Empty) ? " where " + where : "";
                order = (order != string.Empty) ? " order by " + order : "";
                limit = (limit != string.Empty) ? " limit " + limit : "";

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    sql = selectColumn + prefixTable + where + order + limit;

                    using (var command = new NpgsqlCommand(sql, conn))
                    {
                        result = (long)command.ExecuteScalar();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in {0}: {1}", Helper.GetCurrentMethod(), ex.Message);
            }
            return result;
        }

        public static long SQLPostgresExecuteScalar(string sql)
        {
            long result = 0;
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    using (var command = new NpgsqlCommand(sql, conn))
                    {
                        result = (long)command.ExecuteScalar();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in {0}: {1}", Helper.GetCurrentMethod(), ex.Message);
            }
            return result;
        }

        public static bool SQLPostgresExecutionCommand(string sql, List<DBParameter> parameters)
        {
            bool result = false;
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(sql, conn);

                    foreach (var parameter in parameters) 
                    {
                        cmd.Parameters.AddWithValue(parameter.Name, parameter.Type, parameter.Value);
                    }

                    cmd.Prepare();

                    //cmd.ExecuteNonQuery();
                    var res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        result = true;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in {0}: {1}", Helper.GetCurrentMethod(), ex.Message);
                throw;
            }
            return result;
        }

        public async static Task<(DataTable, long)> SQLPostgresSelectSearch(List<string> sql)
        {
            DataTable result = new DataTable();
            long rowCount = 0;
            try
            {
                await using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var command = new NpgsqlCommand(sql[0], conn))
                    {
                        NpgsqlDataReader reader = command.ExecuteReader();
                        result.Load(reader);
                        reader.Close();
                    }
                    using (var command = new NpgsqlCommand(sql[1], conn))
                    {
                        var row = command.ExecuteScalar();
                        rowCount = row is null ? 0 : (long)row;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in {0}: {1}", Helper.GetCurrentMethod(), ex.Message);
            }
            return (result, rowCount);
        }

        public async static Task<bool> SQLPostgresExecutionAddData(List<string> sql, List<DBParameter> parameters)
        {
            bool result = false;
            long currentId = 0;
            try
            {
                await using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();                    
                    using (var command = new NpgsqlCommand(sql[0], conn))
                    {
                        var row = command.ExecuteScalar();
                        currentId = row is null ? 0 : Convert.ToInt64(row);
                        currentId = currentId+1;
                    }
                    using (var cmd = new NpgsqlCommand(sql[1], conn))
                    {
                        foreach (var parameter in parameters)
                        {
                            if (parameter.Name == "id") 
                            {
                                parameter.Value = currentId;
                            }
                            else if (parameter.Name == "code")
                            {
                                parameter.Value = parameter.Value + currentId.ToString("000");
                            }
                            cmd.Parameters.AddWithValue(parameter.Name, parameter.Type, parameter.Value);
                        }

                        cmd.Prepare();
                        var res = cmd.ExecuteNonQuery();
                        if (res > 0) 
                        {
                            result = true;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in {0}: {1}", Helper.GetCurrentMethod(), ex.Message);
                throw;
            }
            return result;
        }
    }
}
