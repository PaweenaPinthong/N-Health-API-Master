using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.Text;
using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;

namespace N_Health_API.Repositories.Master
{
    public class JobTypeData : IJobTypeData
    {
        private IConfiguration _config;

        public JobTypeData(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> Add(JobtypeDataReasone? data, string? userCode)
        {
            bool result = false;
            List<string> arrSql = new List<string>();
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            string formattedDate = dateTime.ToString("yyyy-MM-dd h:mm:ss tt");

            try
            {
                if (data != null)
                {
                    string typeStr = "JT";
                    var lastId = "select jobtype_id from jobtype where created_datetime is not null order by created_datetime desc limit 1";
                    data.jobtypeModel.Jobtype_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);

                    var qInst = "  INSERT INTO \"jobtype\"  " +
                                "(jobtype_id,jobtype_code,jobtype_name,location_id,team,active,created_by,created_datetime,modified_by,modified_datetime,jobtype_desc,product_detail_flag)" +
                                "VALUES(" +
                                $"@id,@code,@jobtype_name,@location_id,@team,@active,@created_by,@created_datetime,@modified_by,@modified_datetime,@jobtype_desc,@product_detail_flag);\r\n";


                    List<DBParameter> parameters = new List<DBParameter>();
                    parameters.Add(new DBParameter { Name = "id", Value = data.jobtypeModel.Jobtype_Id, Type = NpgsqlDbType.Integer });
                    parameters.Add(new DBParameter { Name = "code", Value = data.jobtypeModel.Jobtype_Code, Type = NpgsqlDbType.Varchar });
                    parameters.Add(new DBParameter { Name = "jobtype_name", Value = data.jobtypeModel.Jobtype_Name, Type = NpgsqlDbType.Varchar });
                    parameters.Add(new DBParameter { Name = "jobtype_desc", Value = data.jobtypeModel.Jobtype_Desc, Type = NpgsqlDbType.Varchar });
                    parameters.Add(new DBParameter { Name = "location_id", Value = data.jobtypeModel.Location_Id, Type = NpgsqlDbType.Integer });
                    parameters.Add(new DBParameter { Name = "team", Value = data.jobtypeModel.Team, Type = NpgsqlDbType.Varchar });
                    parameters.Add(new DBParameter { Name = "active", Value = data.jobtypeModel.Active, Type = NpgsqlDbType.Boolean });
                    parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                    parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                    parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                    parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                    parameters.Add(new DBParameter { Name = "product_detail_flag", Value = false, Type = NpgsqlDbType.Boolean });


                    StringBuilder qCost_vt = new StringBuilder();
                    if (data.jobtypeReasons != null)
                    {
                        foreach (var item in data.jobtypeReasons)
                        {
                            var query = "INSERT INTO jobtype_reason " +
                            " ( reason_id,created_by,created_datetime, modified_by, modified_datetime ,jobtype_id) " +
                            " VALUES({0},'{1}','{2}','{3}','{4}',@id);\r\n";
                            query = string.Format(query, item.Reason_Id, userCode, formattedDate, userCode, formattedDate);
                            qCost_vt.Append(query);
                        }
                    }
                    arrSql.Add(qInst + qCost_vt);
                    result = await DBSQLPostgre.SQLPostgresExecutionAddData(arrSql, parameters);
                    return result;
                }


            }
            catch
            {
                throw;
            }
            return result;
        }

        public async Task<MessageResponseModel> CheckDupData(JobtypeDataReasone? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            string? msg = string.Empty;
            var query = "select jt.*" +
            "from jobtype jt " +
            "where jt.jobtype_name = '{0}' "
            + (data?.jobtypeModel?.Jobtype_Id is null || (data?.jobtypeModel.Jobtype_Id <= 0) ? "" : "and jobtype_id != "
            + data?.jobtypeModel.Jobtype_Id);
            var query2 = " and jt.location_id = {0} ";
            query2 = string.Format(query2, data?.jobtypeModel?.Location_Id);
            query = string.Format(query, data?.jobtypeModel?.Jobtype_Name);

            try
            {
                var result = DBSQLPostgre.SQLPostgresSelectCommand(query + query2);
                if (result != null && result?.AsEnumerable().Count() > 0)
                {
                    //data เป็น true ให้ถือว่าเจอข้อมูลซ้ำ
                    msg = msg + string.Format("(Jobtype Name : {0})", data?.jobtypeModel.Jobtype_Name);
                    meg_res.Success = true;
                    meg_res.Message = msg;
                    meg_res.Code = ReturnCode.DUPLICATE_DATA;
                    meg_res.Data = true;
                    return meg_res;
                }
                else
                {
                    //data เป็น false ให้ถือว่าข้อมูลไม่ซ้ำ
                    meg_res.Success = true;
                    meg_res.Message = ReturnMessage.SUCCESS;
                    meg_res.Code = ReturnCode.SUCCESS;
                    meg_res.Data = false;
                    return meg_res;
                }

            }
            catch
            {
                throw;
            }

        }

        public async Task<(DataTable, long)> SearchJobData(SearchJobtypeModel? data)
        {
            string condition = string.Empty;
            MessageResponseModel meg_res = new MessageResponseModel();

            try
            {
                if (data?.Status != null)
                    condition = string.Format(condition + " jt.active = {0} ", data.Status);

                if (!string.IsNullOrEmpty(data?.Jobtype_Name))
                {
                    if (!string.IsNullOrEmpty(condition))
                        condition = condition + " and ";
                    condition = string.Format(condition + " jt.jobtype_name like '%{0}%'", data?.Jobtype_Name);
                }


                if (!string.IsNullOrEmpty(data?.Short_Location_Name))
                {
                    if (!string.IsNullOrEmpty(condition))
                        condition = condition + " and ";
                    condition = string.Format(condition + " l.location_name like '%{0}%'", data?.Short_Location_Name);
                }

                if (!string.IsNullOrEmpty(data?.Team))
                {
                    if (!string.IsNullOrEmpty(condition))
                        condition = condition + " and ";
                    condition = string.Format(condition + " jt.team like '%{0}%'", data?.Team);
                }




                string query = string.Empty;
                string qField = "select " +
                "jt.jobtype_name " +
                ",jt.active " +
                ",jt.team " +
                ",jt.jobtype_id" +
                ",jt.jobtype_code" +
                ",jt.location_id" +
                ",jt.product_Detail_Flag" +
                ",jt.created_by" +
                ",jt.modified_by" +
                ",MAX(r.reason_name) AS reason_name " +
                ",l.location_name" +
                ",lc.\"name\"";
                string qFromJoin = " from jobtype jt " +
                " left join userinfo lc on jt.created_by = lc.user_code" +
                " left join \"location\" l on jt.location_id =l.location_id " +
                " inner join jobtype_reason jr on jr.jobtype_id = jt.jobtype_id" +
                " left join reason r on r.reason_id = jr.reason_id ";

                query = qField + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}")
                + " GROUP by jt.jobtype_id , l.location_id , lc.\"name\"" 
                + $" ORDER BY jt.modified_datetime DESC OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";
                var totalRows = "select  count(jt.jobtype_id) as count_rows " + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

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

        public async Task<bool> Edit(JobtypeDataReasone data, string? usercode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            string formattedDate = dateTime.ToString("yyyy-MM-dd h:mm:ss tt");

            try
            {
                var qJobtype = "  UPDATE \"jobtype\" " +
                "SET jobtype_name = @jobtype_name" +
                ", modified_by = @modified_by" +
                ", jobtype_desc = @jobtype_desc" +
                ", location_id = @location_id" +
                ", team = @team" +
                ", active = @active" +
                ", modified_datetime = @modified_datetime" +
                " where jobtype_id = @jobtype_id;\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "jobtype_id", Value = data.jobtypeModel.Jobtype_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "jobtype_name", Value = data.jobtypeModel.Jobtype_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "jobtype_desc", Value = data.jobtypeModel.Jobtype_Desc, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data.jobtypeModel.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data.jobtypeModel.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = data.jobtypeModel.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = usercode, Type = NpgsqlDbType.Varchar });

                StringBuilder qJobtype_vt = new StringBuilder();
                var queryD = "DELETE from jobtype_reason " +
                "WHERE jobtype_id = @jobtype_id;\r\n";
                queryD = string.Format(queryD, data.jobtypeModel.Jobtype_Id);

                foreach (var item in data.jobtypeReasons)
                {
                    var query = "INSERT INTO jobtype_reason " +
                            " ( reason_id,created_by,created_datetime, modified_by, modified_datetime ,jobtype_id) " +
                            " VALUES({0},'{1}','{2}','{3}','{4}',@jobtype_id);\r\n";
                    query = string.Format(query, item.Reason_Id, usercode, formattedDate, usercode, formattedDate);
                    qJobtype_vt.Append(query);

                }
                result = DBSQLPostgre.SQLPostgresExecutionCommand(qJobtype + queryD + qJobtype_vt, parameters);
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
                var query = "select j.* "
                + ",r.reason_name "
                + "from jobtype j "
                + "inner join jobtype_reason jr on jr.jobtype_id = j.jobtype_id "
                + "left join reason r on r.reason_id = jr.reason_id "
                + "where j.jobtype_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

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
                var query = " UPDATE \"jobtype\"  " +
                            " SET active = @active " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where jobtype_id = @jobtype_id";

                query = String.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "jobtype_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch
            {
                throw;
            }
        }
    }
}