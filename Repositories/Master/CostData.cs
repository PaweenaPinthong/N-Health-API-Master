using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System.Data;
using N_Health_API.Models;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace N_Health_API.Repositories.Master
{
    public class CostData : ICostData
    {
        private IConfiguration _config;

        public CostData(IConfiguration config)
        {
            _config = config;
        }
        public async Task<bool> Add(CostDataModel? data, string? userCode)
        {
            bool result = false;
            try
            {
                List<string> arrSql = new List<string>();
                DateTime dateTime = new DateTimeUtils().NowDateTime();
                if (data != null)
                {
                    string typeStr = "CT";
                    var lastId = "select cost_id from cost where modified_datetime is not null order by modified_datetime desc limit 1";
                    data.Cost.Cost_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);
                }

                var qInst = "  INSERT INTO \"cost\"  " +
                            "(cost_id,cost_code,cost_name,location_id,team,active,created_by,created_datetime,modified_by,modified_datetime)" +
                            "VALUES(" +
                            "@id,@code,@cost_name,@location_id,@team,@active,@created_by,@created_datetime,@modified_by,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id", Value = data?.Cost.Cost_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "code", Value = data?.Cost.Cost_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "cost_name", Value = data?.Cost.Cost_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Cost.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Cost.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Cost.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                               

                //string qCost_vt = string.Empty;
                StringBuilder qCost_vt = new StringBuilder();
                if (data?.CostVehicleType != null) 
                {
                    foreach (var item in data.CostVehicleType)
                    {
                        var query = "INSERT INTO cost_vehicle_type " +
                        " (cost_value, cost_per_unit_Type,created_by,created_datetime, modified_by, modified_datetime ,vehicle_type_id,active,cost_id) " +
                        " VALUES({0},'{1}','{2}','{3}','{4}','{5}',{6},{7},@id);\r\n";
                        query = string.Format(query, item.Cost_Value, item.Cost_Per_Unit_Type, userCode, dateTime, userCode, dateTime, item.Vehicle_Type_Id,item.Active);
                        qCost_vt.Append(query);
                    }
                }
                arrSql.Add(qInst + qCost_vt);
                //arrSql.Add(qInst);
                result = await DBSQLPostgre.SQLPostgresExecutionAddData(arrSql, parameters);
                return result;
                
                //result = DBSQLPostgre.SQLPostgresExecutionCommand(qCost + qCost_vt, parameters);
                //return result;
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
                var query = "  UPDATE \"cost\"     " +
                            "   SET active = @active  " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where cost_id = @cost_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "cost_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(CostDataModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string? msg = string.Empty;
                var query = "select c.*" +
                " from cost c " +
                            " where c.cost_name = '{0}' "+ (data?.Cost.Cost_Id is null || (data?.Cost.Cost_Id <= 0) ? "": " and cost_id = " + data?.Cost.Cost_Id);
                query = string.Format(query, data?.Cost.Cost_Name);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                if (result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = msg + string.Format("(Cost_Name : {0})", data?.Cost.Cost_Name);
                    meg_res.Success = true;
                    meg_res.Message = msg;
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

        public async Task<bool> Edit(CostDataModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {


                var qCost = "  UPDATE \"cost\"  " +
                            "SET  cost_name = @cost_name" +
                            ", location_id = @location_id" +
                            ", team = @team " +
                            ", active = @active " +
                            ", modified_by = @modified_by, modified_datetime = @modified_datetime  " +
                            " where cost_id = @cost_id;\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "cost_name", Value = data?.Cost.Cost_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Cost?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Cost?.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Cost?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "cost_id", Value = data?.Cost?.Cost_Id, Type = NpgsqlDbType.Integer });

                StringBuilder qCost_vt = new StringBuilder();
                foreach (var item in data.CostVehicleType)
                {
                    var query = "UPDATE cost_vehicle_type " +
                    "SET cost_value = {0}" +
                    ", cost_per_unit_Type = '{1}'" +
                    ", modified_by = '{2}'" +
                    ", modified_datetime = '{3}'" +
                    "WHERE cost_id = {4}  and vehicle_type_id = {5};\r\n";
                    query = string.Format(query,item.Cost_Value, item.Cost_Per_Unit_Type, userCode, dateTime, data?.Cost?.Cost_Id, item.Vehicle_Type_Id);

                    qCost_vt.Append(query);
                }
                result = DBSQLPostgre.SQLPostgresExecutionCommand(qCost + qCost_vt, parameters);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<CostDataModel> GetById(int id)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                CostDataModel costDataModel = new CostDataModel();
                var query = "select " +
                            " c.cost_id"+
                            ", c.cost_code"+
                            ", c.cost_name"+
                            ", c.location_id"+
                            ", c.team"+
                            ", c.active"+
                            ", c.created_datetime"+
                            ", c.modified_datetime " +
                            ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                            ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                            " from cost c " +
                            " left join userinfo uc on c.created_by = uc.user_code " +
                            " left join userinfo um on c.modified_by = um.user_code " +
                            " where cost_id = {0}";
                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                CostModel cost = Util.ConvertDataTableToList<CostModel>(result).First();

                var qCostVT = "select " +
                            "  cvt.vehicle_type_id" +
                            ", cvt.cost_value" +
                            ", cvt.cost_per_unit_type" +
                            ", cvt.active" +
                            ", cvt.created_datetime" +
                            ", cvt.modified_datetime " +
                            ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                            ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                            " from cost_vehicle_type cvt " +                           
                            " left join userinfo uc on cvt.created_by = uc.user_code " +
                            " left join userinfo um on cvt.modified_by = um.user_code " +
                            " where cvt.cost_id = {0}";
                qCostVT = string.Format(qCostVT, id);

                var costVT = DBSQLPostgre.SQLPostgresSelectCommand(qCostVT);
                List<CostVehicleTypeModel> dCostVT = Util.ConvertDataTableToList<CostVehicleTypeModel>(costVT).ToList();

                //Mapping Model
                costDataModel.Cost = cost;
                costDataModel.CostVehicleType = dCostVT;

                return costDataModel;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchCostModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {           
                string condition = string.Empty;

                if (data?.Active != null)//status
                {
                    condition = string.Format(condition + " c.active = {0}", data.Active);
                }

                if (!string.IsNullOrEmpty(data?.Cost_Name))
                {
                    if (!string.IsNullOrEmpty(condition))
                    {
                        condition = condition + " and ";
                    }
                    condition = string.Format(condition + " c.cost_name = '%{0}%'", data.Cost_Name);
                }
                if (!string.IsNullOrEmpty(data?.Location_Name))
                {
                    if (!string.IsNullOrEmpty(condition))
                    {
                        condition = condition + " and ";
                    }
                    condition = string.Format(condition + " lc.location_name '%{0}%'", data.Location_Name);
                }

                string query = string.Empty;
                string qField = "select " +
                               "c.cost_code" +
                               ",c.cost_name" +
                               ",c.active" +
                               ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                               ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                               ",c.created_datetime" +
                               ",c.modified_datetime " +
                               ",lc.location_name";

                string qFromJoin =" from cost c " +
                                 " left join location lc on c.location_id = lc.location_id" +
                                 " left join userinfo uc on c.created_by = uc.user_code " +
                                 " left join userinfo um on c.modified_by = um.user_code " ;

                query = qField + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}")
                        + $" ORDER BY c.modified_by OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";
 
                var totalRows = "select  count(cost_id) as count_rows "+ qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");
                
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
