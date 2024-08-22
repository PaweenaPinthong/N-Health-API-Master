using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System;
using System.Data;

namespace N_Health_API.Repositories.Master
{
    public class VehicleTypeData : IVehicleTypeData
    {
        public async Task<bool> Add(VehicleTypeModel? data, string? userCode)
        {
            bool result = false;
            
            
            try
            {
                List<string> arrSql = new List<string>();
                DateTime dateTime = new DateTimeUtils().NowDateTime();
                if (data != null)
                {
                    string typeStr = "VT";
                    var lastId = "select vehicle_type_id from vehicle_type where modified_datetime is not null order by modified_datetime desc limit 1";
                    data.Vehicle_Type_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);
                }

                var qInst = "INSERT INTO vehicle_type " +
                    "(vehicle_type_id " +
                    ",vehicle_type_code" +
                    ",vehicle_type_name " +
                    ",active, created_by, created_datetime, modified_by, modified_datetime) " +
                    "VALUES(" +
                    "@id " +
                    ",@code " +
                    ",@vehicle_type_name " +
                    ",@active ,@created_by ,@created_datetime ,@modified_by ,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id", Value = data?.Vehicle_Type_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "code", Value = data?.Vehicle_Type_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "vehicle_type_name", Value = data?.Vehicle_Type_Name, Type = NpgsqlDbType.Varchar });            
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                arrSql.Add(qInst);
                result = await DBSQLPostgre.SQLPostgresExecutionAddData(arrSql, parameters);
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
                var query = "  UPDATE \"vehicle_type\"     " +
                            "   SET active = @active  " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where vehicle_type_id = @vehicle_type_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "vehicle_type_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(VehicleTypeModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string? msg = string.Empty;
                var query = "select v.*" +
                            " from vehicle_type v " +
                            " where v.vehicle_type_name = '{0}'" + (data?.Vehicle_Type_Id is null || (data?.Vehicle_Type_Id <= 0) ? "" :" and v.vehicle_type_id = " + data?.Vehicle_Type_Id);

                query = string.Format(query, data?.Vehicle_Type_Name);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                if (result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = msg + string.Format("(Vehicle Name : {0}) ", data?.Vehicle_Type_Name);
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

        public async Task<bool> Edit(VehicleTypeModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var query = "UPDATE vehicle_type SET" +
                    " vehicle_type_name = @vehicle_type_name" +
                    " ,active = @active" +
                    " ,modified_by = @modified_by " +
                    " ,modified_datetime = @modified_datetime  " +
                    "where vehicle_type_id = @vehicle_type_id ;\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "vehicle_type_id", Value = data?.Vehicle_Type_Id, Type = NpgsqlDbType.Integer });                
                parameters.Add(new DBParameter { Name = "vehicle_type_name", Value = data?.Vehicle_Type_Name, Type = NpgsqlDbType.Varchar });
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
                            "v.vehicle_type_id" +
                            ",v.vehicle_type_code" +
                            ",v.vehicle_type_name" +
                            ",v.active" +
                            ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                            ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                            ",v.created_datetime" +
                            ",v.modified_datetime " +
                            " from vehicle_type v " +
                            " left join userinfo uc on v.created_by = uc.user_code " +
                            " left join userinfo um on v.modified_by = um.user_code " +
                            " where vehicle_type_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchVehicleTypeModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string condition = string.Empty;

                //status
                condition = data?.Active != null ? string.Format(condition + " v.active = {0}", data.Active) : " v.active in (true,false)";
                //Vehicle_Type_Name
                condition = condition + (!string.IsNullOrEmpty(data?.Vehicle_Type_Name)  ? string.Format(" and v.vehicle_type_name like '%{0}%'", data?.Vehicle_Type_Name) : "");
                //Vehicle_Type_Code
                condition = condition + (!string.IsNullOrEmpty(data?.Vehicle_Type_Code) ? string.Format(" and v.vehicle_type_code = '{0}'", data?.Vehicle_Type_Code) : "");

                string query = string.Empty;
                string qField = "select " +
                               " v.vehicle_type_id" +
                               ",v.vehicle_type_code" +
                               ",v.vehicle_type_name" +
                               ",v.created_datetime" +
                               ",v.modified_datetime" +
                               ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                               ",concat(um.\"name\",' ',um.lastname ) as modified_by" ;

                string qFromJoin = " from vehicle_type v " +
                                 " left join userinfo uc on v.created_by = uc.user_code " +
                                 " left join userinfo um on v.modified_by = um.user_code ";

                query = qField + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}")
                        + $" ORDER BY v.modified_datetime desc OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";

                var totalRows = "select  count(v.vehicle_type_id) as count_rows " + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

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
