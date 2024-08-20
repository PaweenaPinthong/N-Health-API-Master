using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System.Data;

namespace N_Health_API.Repositories.Master
{
    public class PriceData : IPriceData
    {

        public async Task<bool> Add(PriceModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var query = "INSERT INTO price " +
                    "(price_id, price_code " +
                    ",location_id ,team " +
                    ",priority_id ,department_id  " +
                    ",price_single ,price_multi " +
                    ",penalty_rate,penalty_unit " +
                    ", active, created_by, created_datetime, modified_by, modified_datetime) " +
                    "VALUES(@price_id ,@price_code " +
                    ",@location_id ,@team " +
                    ",@priority_id,@department_id  " +
                    ",@price_single ,@price_multi " +
                    ",@penalty_rate,@penalty_unit " +
                    ",@active ,@created_by ,@created_datetime ,@modified_by ,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "price_id", Value = data?.Price_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "price_code", Value = data?.Price_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_id", Value = data?.Priority_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "department_id", Value = data?.Department_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "price_single", Value = data?.Price_Single, Type = NpgsqlDbType.Numeric });
                parameters.Add(new DBParameter { Name = "price_multi", Value = data?.Price_Multi, Type = NpgsqlDbType.Numeric });
                parameters.Add(new DBParameter { Name = "penalty_rate", Value = data?.Penalty_Rate, Type = NpgsqlDbType.Numeric });
                parameters.Add(new DBParameter { Name = "penalty_unit", Value = data?.Penalty_Unit, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
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


        public async Task<bool> ChangeActive(int id, bool isActive, string? userCode)
        {
            try
            {
                var query = "  UPDATE \"price\"     " +
                            "   SET active = @active  " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where price_id = @price_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "price_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(PriceModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string? msg = string.Empty;
                var query = "select p.*" +
                            " from price p " +
                            " where p.team = '{0}' and p.location_id = {1} " + (data?.Price_Id is not null ? "and cost_id = " + data?.Price_Id : "");

                query = string.Format(query, data?.Location_Id,data?.Team);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                if (result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = msg + string.Format("(Location Name : {0}) ,(Team : {1})", data?.Location_Name, data?.Team);
                    meg_res.Success = true;
                    meg_res.Message = msg + ReturnCode.DUPLICATE_DATA;
                    meg_res.Code = ReturnCode.DUPLICATE_DATA;
                    meg_res.Data = true;//data เป็น true ให้ถือว่าเจอข้อมูลซ้ำ
                    return meg_res;
                }
                else
                {
                    meg_res.Success = true;
                    meg_res.Message = ReturnMessage.SUCCESS;
                    meg_res.Code = ReturnCode.SUCCESS;
                    meg_res.Data = false;//data เป็น false ให้ถือว่าข้อมูลไม่ซ้ำ
                    return meg_res;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> Edit(PriceModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var query = "UPDATE price SET" +
                    ",location_id = @location_id" +
                    ",team = @team" +
                    ",priority_id = @priority_id" +
                    ",department_id = @department_id " +
                    ",price_single = @price_single " +
                    ",price_multi = @price_multi" +
                    ",penalty_rate = @penalty_rate" +
                    ",penalty_unit = @penalty_unit" +
                    ",active = @active" +
                    ",modified_by = @modified_by " +
                    ",modified_datetime =@modified_datetime  " +
                    "where price_id = @price_id ;\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "price_id", Value = data?.Price_Id, Type = NpgsqlDbType.Integer });
                //parameters.Add(new DBParameter { Name = "price_code", Value = data?.Price_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_id", Value = data?.Priority_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "department_id", Value = data?.Department_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "price_single", Value = data?.Price_Single, Type = NpgsqlDbType.Numeric });
                parameters.Add(new DBParameter { Name = "price_multi", Value = data?.Price_Multi, Type = NpgsqlDbType.Numeric });
                parameters.Add(new DBParameter { Name = "penalty_rate", Value = data?.Penalty_Rate, Type = NpgsqlDbType.Numeric });
                parameters.Add(new DBParameter { Name = "penalty_unit", Value = data?.Penalty_Unit, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean });
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
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                var query = "select " +
                            " p.price_id" +
                            ",p.price_code" +
                            ",p.team" +
                            ",p.created_datetime" +
                            ",p.modified_datetime" +
                            ",p.active" +
                            ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                            ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                            " from price p " +
                            " left join userinfo uc on p.created_by = uc.user_code " +
                            " left join userinfo um on p.modified_by = um.user_code "+
                            " where price_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchPriceModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string condition = string.Empty;

                if (data?.Active != null)//status
                {
                    condition = string.Format(condition + " p.active = {0}", data.Active);
                }
                //Location_Name
                condition = condition + (data?.Location_Id is not null ? string.Format(" and p.location_id = {0}", data?.Location_Id) : "");
                //Priority
                condition = condition + (data?.Priority_Id is not null ? string.Format(" and p.priority_id = {0}", data?.Priority_Id) : "");

                string query = string.Empty;
                string qField = "select " +
                               " p.price_id" +
                               ",p.price_code" +
                               ",p.team" +
                               ",p.created_datetime" +
                               ",p.modified_datetime" +
                               ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                               ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                               ",lc.location_name" +
                               ",pt.priority_name ";

                string qFromJoin = " from price p " +
                                 " left join location lc on p.location_id = lc.location_id" +
                                 " left join userinfo uc on p.created_by = uc.user_code " +
                                 " left join userinfo um on p.modified_by = um.user_code " +
                                 " left join priority pt um on p.priority_id = pt.priority_id ";

                query = qField + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}")
                        + $" ORDER BY p.modified_datetime OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";

                var totalRows = "select  count(p.price_id) as count_rows " + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

                List<string> arrSql = new List<string>();
                arrSql.Add(query);
                arrSql.Add(totalRows);
                var result = await DBSQLPostgre.SQLPostgresSelectSearch(arrSql);
                return result;
            }
            catch
            {
                throw;
            }
        }
    }
}
