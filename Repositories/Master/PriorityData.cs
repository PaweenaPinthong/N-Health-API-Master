using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace N_Health_API.Repositories.Master
{
    public class PriorityData : IPriorityData
    {
        public async Task<bool> Add(PriorityModel? priorityModel, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try {
                // query add Priority
                var query = "INSERT INTO priority " +
                "(priority_id, priority_code, priority_name, priority_color, active, created_by, created_datetime, modified_by, modified_datetime) " +
                "VALUES(@priority_id, @priority_code, @priority_name, @priority_color, @active, @created_by, @created_datetime, @modified_by, @modified_datetime);";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "priority_id", Value = priorityModel?.Priority_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "priority_code", Value = priorityModel?.Priority_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_name", Value = priorityModel?.Priority_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_color", Value = priorityModel?.Priority_Color, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = priorityModel?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });

                result = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return result;
            } catch {
                throw;
            }
        }

        public async Task<bool> ChangeActive(int id, bool isActive, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var query = "   UPDATE \"priority\" SET active = @active" +
                            ",  modified_by = @modified_by " +
                            ",  modified_datetime = @modified_datetime  " +
                            "   where priority_id = @priority_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "priority_id", Value = id, Type = NpgsqlDbType.Integer });

                result = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(PriorityModel? priorityModel, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                // query add Priority
                var query = "UPDATE priority SET" +
                ", priority_code = @priority_code " +
                ", priority_name = @priority_name " +
                ", priority_color = @priority_color " +
                ", active = @active " +
                ", modified_by = @modified_by " +
                ", modified_datetime = @modified_datetime" +
                "  where priority_id = @priority_id ;";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "priority_id", Value = priorityModel?.Priority_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "priority_code", Value = priorityModel?.Priority_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_name", Value = priorityModel?.Priority_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_color", Value = priorityModel?.Priority_Color, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = priorityModel?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });

                result = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetById(int id)
        {
            try
            {
                var query = "select * from priority where priority_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchPriorityModel? dataSearch)
        {
            try {
                string condition = string.Empty;
                // check status ถ้า null คือโชว์ทั้งหมด ไม่ใส่ลงไปในเงื่อนไข
                if (dataSearch?.Active != null) {
                    condition = string.Format(condition + " p.active = {0}", dataSearch.Active);
                }
                // เงื่อนไข priority code
                condition = condition + (dataSearch?.Priority_Code is not null ? string.Format(" and priority_code = {0}", dataSearch?.Priority_Code) : "");
                // เงื่อนไข priority name
                condition = condition + (dataSearch?.Priority_Name is not null ? string.Format(" and priority_name = {0}", dataSearch?.Priority_Name) : "");
            
                string qField = "select * from priority";
                string query = qField + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}") +
                    $" ORDER BY modified_datetime OFFSET (({dataSearch?.PageNumber}-1)*{dataSearch?.PageSize}) ROWS FETCH NEXT {dataSearch?.PageSize} ROWS ONLY;\r\n";
                var totalRows = "select  count(p.price_id) as count_rows " + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

                List<string> arrSql = new List<string>();
                arrSql.Add(query);
                arrSql.Add(totalRows);

                var result = await DBSQLPostgre.SQLPostgresSelectSearch(arrSql);
                return result;
            }
            catch { 
                throw;
            }
        }
    }
}
