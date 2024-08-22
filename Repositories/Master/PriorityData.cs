using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System.Data;
using System.Text;
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
                List<string> arrSql = new List<string>();
                if (priorityModel != null)
                {
                    string typeStr = "PY";
                    var lastId = "select priority_id from priority where modified_datetime is not null order by modified_datetime desc limit 1";
                    priorityModel.Priority_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);
                }
                // query add Priority
                var query = "INSERT INTO priority " +
                "(priority_id, priority_code, priority_name, priority_color, active, created_by, created_datetime, modified_by, modified_datetime) " +
                "VALUES(@id, @code, @priority_name, @priority_color, @active, @created_by, @created_datetime, @modified_by, @modified_datetime);";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id", Value = priorityModel?.Priority_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "code", Value = priorityModel?.Priority_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_name", Value = priorityModel?.Priority_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_color", Value = priorityModel?.Priority_Color, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = priorityModel?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                arrSql.Add(query);

                result = await DBSQLPostgre.SQLPostgresExecutionAddData(arrSql, parameters);
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
                // query Edit Priority
                var query = "UPDATE priority SET" +
                "  priority_name = @priority_name " +
                ", priority_color = @priority_color " +
                ", active = @active " +
                ", modified_by = @modified_by " +
                ", modified_datetime = @modified_datetime" +
                "  where priority_id = @priority_id ;";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "priority_id", Value = priorityModel?.Priority_Id, Type = NpgsqlDbType.Integer });
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
                StringBuilder condition = new StringBuilder();

                // check status ถ้า null คือโชว์ทั้งหมด ไม่ใส่ลงไปในเงื่อนไข
                if (dataSearch?.Active != null)
                {
                    condition.AppendFormat("p.active = {0}", dataSearch.Active);
                }

                // เงื่อนไข priority code
                if (dataSearch?.Priority_Code != null)
                {
                    if (condition.Length > 0) condition.Append(" and ");
                    condition.AppendFormat("p.priority_code LIKE '%{0}%'", dataSearch.Priority_Code);
                }

                // เงื่อนไข priority name
                if (dataSearch?.Priority_Name != null)
                {
                    if (condition.Length > 0) condition.Append(" and ");
                    condition.AppendFormat("p.priority_name LIKE '%{0}%'", dataSearch.Priority_Name);
                }
                string qField = "select "+
                    " p.priority_id , "+
                    " p.priority_code , " +
                    " p.priority_name , " +
                    " p.priority_color , " +
                    " p.active , " +
                    " concat(uc.\"name\",' ',uc.lastname ) as created_by , " +
                    " p.created_dateTime , " +
                    " concat(um.\"name\",' ',um.lastname ) as modified_by , " +
                    " p.modified_dateTime " +
                    " from priority p" +
                    " left join userinfo uc on p.created_by = uc.user_code" +
                    " left join userinfo um on p.modified_by = um.user_code";
                string query = qField + (condition.Length == 0 ? "" : $" where {condition}") +
                    $" ORDER BY modified_datetime OFFSET (({dataSearch?.PageNumber}-1)*{dataSearch?.PageSize}) ROWS FETCH NEXT {dataSearch?.PageSize} ROWS ONLY;\r\n";
                var totalRows = "select count(p.priority_id) as count_rows from priority p" +
                    (condition.Length == 0 ? "" : $" where {condition}");

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
