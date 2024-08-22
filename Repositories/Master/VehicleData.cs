using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System.Data;
using N_Health_API.Models;


namespace N_Health_API.Repositories.Master
{
    public class VehicleData : IVehicleData
    {
        public async Task<bool> Add(VehicleModel? data, string? userCode)
        {
            bool result = false;


            try
            {
                List<string> arrSql = new List<string>();
                DateTime dateTime = new DateTimeUtils().NowDateTime();
                if (data != null)
                {
                    string typeStr = "VC";
                    var lastId = "select vehicle_id from vehicle where modified_datetime is not null order by modified_datetime desc limit 1";
                    data.Vehicle_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);
                }

                var qInst = "INSERT INTO vehicle " +
                    "(vehicle_type_id " +
                    ",vehicle_type_code" +
                    ", location_id" +
                    ", team" +
                    ", vehicle_type_id" +
                    ", plate_no" +
                    ", contract_start" +
                    ", contract_end" +
                    ", contract_type" +
                    ", vendor_name" +
                    ", tax_end" +
                    ", compulsory_insurance_end" +
                    ", insurance_end" +
                    ", calibration_end" +
                    ", gps_end" +
                    ", chassis_no" +
                    ", fuel_rate" +
                    ",active, created_by, created_datetime, modified_by, modified_datetime) " +
                    "VALUES(" +
                    "@id " +
                    ",@code " +
                    ",@location_id" +
                    ",@team" +
                    ",@vehicle_type_id" +
                    ",@plate_no" +
                    ",@contract_start" +
                    ",@contract_end" +
                    ",@contract_type" +
                    ",@vendor_name" +
                    ",@tax_end" +
                    ",@compulsory_insurance_end" +
                    ",@insurance_end" +
                    ",@calibration_end" +
                    ",@gps_end" +
                    ",@chassis_no" +
                    ",@fuel_rate" +
                    ",@active ,@created_by ,@created_datetime ,@modified_by ,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id", Value = data?.Vehicle_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "code", Value = data?.Vehicle_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "@location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@team", Value = data?.Team, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@vehicle_type_id", Value = data?.Vehicle_Type_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@plate_no", Value = data?.Plate_No, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "@contract_start", Value = data?.Contract_Start, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@contract_end", Value = data?.Contract_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@contract_type", Value = data?.Contract_Type, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@vendor_name", Value = data?.Vendor_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "@tax_end", Value = data?.Tax_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@compulsory_insurance_end", Value = data?.Compulsory_Insurance_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@insurance_end", Value = data?.Insurance_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@calibration_end", Value = data?.Calibration_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@gps_end", Value = data?.Gps_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@chassis_no", Value = data?.Chassis_No, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "@fuel_rate", Value = data?.Fuel_Rate, Type = NpgsqlDbType.Numeric });
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
                var query = "  UPDATE \"vehicle\"     " +
                            "   SET active = @active  " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where vehicle_id = @vehicle_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "vehicle_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(VehicleModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string? msg = string.Empty;
                var query = "select v.*" +
                            " from vehicle v " +
                            " where v.location_id = '{0}' and v.plate_no = '{1}'" + (data?.Location_Id is null || (data?.Location_Id <= 0) ? "" : " and v.vehicle_id = " + data?.Vehicle_Id);

                query = string.Format(query, data?.Vehicle_Type_Id,data?.Plate_No);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                if (result != null && result?.AsEnumerable().Count() > 0)//เช็คซ้ำ Location กับ ทะเบียนรถ
                {
                    msg = msg + string.Format("(Location Name : {0} และ ทะเบียนรถ : {1})", data?.Location_Name, data?.Plate_No);
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

        public async Task<bool> Edit(VehicleModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var query = "UPDATE vehicle SET" +
                    "location_id  = @location_id" +
                    " ,team  = @team" +
                    " ,vehicle_type_id  = @vehicle_type_id" +
                    " ,plate_no  = @plate_no" +
                    " ,contract_start  = @contract_start" +
                    " ,contract_end  = @contract_end" +
                    " ,contract_type  = @contract_type" +
                    " ,vendor_name  = @vendor_name" +
                    " ,tax_end  = @tax_end" +
                    " ,compulsory_insurance_end  = @compulsory_insurance_end" +
                    " ,insurance_end  = @insurance_end" +
                    " ,calibration_end  = @calibration_end" +
                    " ,gps_end  = @gps_end" +
                    " ,chassis_no  = @chassis_no" +
                    " ,fuel_rate  = @fuel_rate" +
                    " ,active = @active" +
                    " ,modified_by = @modified_by " +
                    " ,modified_datetime = @modified_datetime  " +
                    "where vehicle_id = @vehicle_id ;\r\n";


                List<DBParameter> parameters = new List<DBParameter>();
                
                parameters.Add(new DBParameter { Name = "@location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@team", Value = data?.Team, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@vehicle_type_id", Value = data?.Vehicle_Type_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@plate_no", Value = data?.Plate_No, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "@contract_start", Value = data?.Contract_Start, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@contract_end", Value = data?.Contract_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@contract_type", Value = data?.Contract_Type, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "@vendor_name", Value = data?.Vendor_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "@tax_end", Value = data?.Tax_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@compulsory_insurance_end", Value = data?.Compulsory_Insurance_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@insurance_end", Value = data?.Insurance_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@calibration_end", Value = data?.Calibration_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@gps_end", Value = data?.Gps_End, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "@chassis_no", Value = data?.Chassis_No, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "@fuel_rate", Value = data?.Fuel_Rate, Type = NpgsqlDbType.Numeric });
                parameters.Add(new DBParameter { Name = "vehicle_id", Value = data?.Vehicle_Id, Type = NpgsqlDbType.Integer });
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
                            "v.vehicle_id" +
                            ",v.vehicle_code" +
                            ",v.location_id" +
                            ",v.team" +
                            ",v.vehicle_type_id" +
                            ",v.plate_no" +
                            ",v.contract_start" +
                            ",v.contract_end" +
                            ",v.contract_type" +
                            ",v.vendor_name" +
                            ",v.tax_end" +
                            ",v.compulsory_insurance_end" +
                            ",v.insurance_end" +
                            ",v.calibration_end" +
                            ",v.gps_end" +
                            ",v.chassis_no" +
                            ",v.fuel_rate" +
                            ",v.active" +
                            ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                            ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                            ",v.created_datetime" +
                            ",v.modified_datetime " +
                            " from vehicle v " +
                            " left join userinfo uc on v.created_by = uc.user_code " +
                            " left join userinfo um on v.modified_by = um.user_code " +
                            " where vehicle_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchVehicleModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string condition = string.Empty;

                //status
                condition = data?.Active != null? string.Format(condition + " v.active = {0}", data.Active) : " v.active in (true,false)";

                //Vehicle_Type_Name
                condition = condition + (!string.IsNullOrEmpty(data?.Vehicle_Type_Name) ? string.Format(" and vt.vehicle_type_name like '%{0}%'", data?.Vehicle_Type_Name) : "");
                //ทะเบียนรถ
                condition = condition + (!string.IsNullOrEmpty(data?.Plate_No) ? string.Format(" and v.plate_no = '{0}'", data?.Plate_No) : "");

                string query = string.Empty;
                string qField = "select " +
                               " v.vehicle_id" +
                               ",v.vehicle_code" +
                               ",lc.location_name" +
                               ",vt.vehicle_type_name" +
                               ",v.created_datetime" +
                               ",v.modified_datetime" +
                               ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                               ",concat(um.\"name\",' ',um.lastname ) as modified_by";

                string qFromJoin = " from vehicle v " +
                                 " left join location lc on v.location_id = lc.location_id" +
                                 " left join vehicle_type vt on v.vehicle_type_id = vt.vehicle_type_id" +
                                 " left join userinfo uc on v.created_by = uc.user_code " +
                                 " left join userinfo um on v.modified_by = um.user_code ";

                query = qField + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}")
                        + $" ORDER BY v.modified_by desc OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";

                var totalRows = "select  count(v.vehicle_id) as count_rows " + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

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
