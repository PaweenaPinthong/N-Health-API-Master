using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace N_Health_API.Repositories.Master
{
    public class PriorityJobTypeData : IPriorityJobTypeData
    {
        public async Task<bool> Add(PriorityJobtypeDataModel data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();

            try
            {
                List<string> arrSql = new List<string>();
                if (data != null)
                {
                    string typeStr = "PJ";
                    var lastId = "select priority_jobtype_id from priority_jobtype where created_datetime is not null order by created_datetime desc limit 1";
                    data.PriorityJobtype.Priority_Jobtype_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);
                }
                // query add Priority Job Type
                var qInst = "INSERT INTO priority_jobtype " +
                "(priority_jobtype_id,priority_jobtype_code, priority_id, location_id,team, service_time, waiting_time, active, created_by, created_datetime, modified_by, modified_datetime) " +
                "VALUES(@id, @code, @priority_id, @location_id, @team, @service_time, @waiting_time, @active, @created_by, @created_datetime, @modified_by, @modified_datetime);";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id", Value = data?.PriorityJobtype.Priority_Jobtype_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "code", Value = data?.PriorityJobtype.Priority_Jobtype_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "priority_id", Value = data?.PriorityJobtype.Priority_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.PriorityJobtype.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.PriorityJobtype.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "service_time", Value = data?.PriorityJobtype.Service_Time, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "waiting_time", Value = data?.PriorityJobtype.Waiting_Time, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "active", Value = data?.PriorityJobtype.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });

                // insert jobtype_id to Table priority_jobtype_jobtype
                StringBuilder qJobType = new StringBuilder();
                if (data?.Jobtype != null)
                {
                    foreach (var item in data.Jobtype)
                    {
                        var query = "INSERT INTO priority_jobtype_jobtype " +
                        " (priority_jobtype_id, jobtype_id,created_by,created_datetime, modified_by, modified_datetime) " +
                        " VALUES(@id,'{0}','{1}','{2}','{3}','{4}');\r\n";
                        query = string.Format(query, item.Jobtype_Id, userCode, dateTime.ToString("yyyy-MM-dd hh:mm:ss tt"), userCode, dateTime.ToString("yyyy-MM-dd hh:mm:ss tt"));
                        qJobType.Append(query);
                    }
                }
                arrSql.Add(qInst + qJobType);


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
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var query = "   UPDATE \"priority_jobtype\" SET active = @active" +
                            ",  modified_by = @modified_by " +
                            ",  modified_datetime = @modified_datetime  " +
                            "   where priority_jobtype_id = @priority_jobtype_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "priority_jobtype_id", Value = id, Type = NpgsqlDbType.Integer });

                result = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(PriorityJobtypeDataModel data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string? msg = string.Empty;
                var query = "select pj.*" +
                            " from priority_jobtype pj " +
                            " where pj.priority_id = {0} "+
                            " and pj.location_id != {1}" +"" +
                            " and LOWER(pj.team) = LOWER('{2}')";

                query = string.Format(query, data?.PriorityJobtype.Priority_Id,data?.PriorityJobtype.Location_Id,data?.PriorityJobtype.Team);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                if (result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = "(ข้อมูล Priority Name, Location Name, Team)";
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

        public async Task<bool> Edit(PriorityJobtypeDataModel data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                // query Edit Priority Job Type
                var qUpdate = "UPDATE priority_jobtype SET" +
                "  team = @team " +
                ", priority_id = @priority_id " +
                ", location_id = @location_id " +
                ", service_time = @service_time " +
                ", waiting_time = @waiting_time " +
                ", active = @active " + 
                ", modified_by = @modified_by " +
                ", modified_datetime = @modified_datetime" +
                "  where priority_jobtype_id = @priority_jobtype_id ;";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "priority_jobtype_id", Value = data?.PriorityJobtype.Priority_Jobtype_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "priority_id", Value = data?.PriorityJobtype.Priority_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.PriorityJobtype.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.PriorityJobtype.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "service_time", Value = data?.PriorityJobtype.Service_Time, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "waiting_time", Value = data?.PriorityJobtype.Waiting_Time, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "active", Value = data?.PriorityJobtype.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });

                StringBuilder qJobType = new StringBuilder();
                var qDel = "DELETE FROM priority_jobtype_jobtype WHERE priority_jobtype_id={0};";
                qDel = string.Format(qDel, data?.PriorityJobtype.Priority_Jobtype_Id);
                qJobType.Append(qDel);
                if (data?.Jobtype != null) {
                    foreach (var item in data.Jobtype)
                    {
                        var query = "INSERT INTO priority_jobtype_jobtype " +
                       " (priority_jobtype_id, jobtype_id,created_by,created_datetime, modified_by, modified_datetime) " +
                       " VALUES('{0}','{1}','{2}','{3}','{4}','{5}');\r\n";
                        query = string.Format(query,data?.PriorityJobtype.Priority_Jobtype_Id , item.Jobtype_Id, userCode, dateTime.ToString("yyyy-MM-dd hh:mm:ss tt"), userCode, dateTime.ToString("yyyy-MM-dd hh:mm:ss tt"));

                        qJobType.Append(query);
                    }
                }

                result = DBSQLPostgre.SQLPostgresExecutionCommand(qUpdate + qJobType, parameters);
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
                var query = "select "+
                        " PJT.priority_jobtype_id ," +
                        " PJT.priority_jobtype_code ," +
                        " P.priority_id ," +
                        " P.priority_name ," +
                        " PJT.team ," +
                        " PJT.service_time ," +
                        " PJT.waiting_time ," +
                        " PJT.active"+
                        " from priority_jobtype PJT"+
                        " LEFT JOIN priority P on P.priority_id = PJT.priority_id"+
                        " where PJT.priority_jobtype_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<DataTable> GetListJobTypeById(int id)
        {
             try
            {
                var query = "select" +
                    "  PJJ.priority_jobtype_id" +
                    ", JT.jobtype_id" +
                    ", JT.jobtype_name" +
                    "  from priority_jobtype_jobtype PJJ" +
                    "  left join jobtype JT on JT.jobtype_id = PJJ.jobtype_id" +
                    "  where PJJ.priority_jobtype_id = {0}";

                query = string.Format(query, id);

                var result =  DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchPriorityJobtypeModel? dataSearch)
        {
            try
            {
                StringBuilder condition = new StringBuilder();

                // check status ถ้า null คือโชว์ทั้งหมด ไม่ใส่ลงไปในเงื่อนไข
                if (dataSearch?.Active != null)
                {
                    condition.AppendFormat("pj.active = {0}", dataSearch.Active);
                }

                // เงื่อนไข Team
                if (dataSearch?.Team != null)
                {
                    if (condition.Length > 0) condition.Append(" and ");
                    condition.AppendFormat("pj.team LIKE '%{0}%'", dataSearch.Team);
                }

                // เงื่อนไข priority name by id
                if (dataSearch?.Priority_Id != null)
                {
                    if (condition.Length > 0) condition.Append(" and ");
                    condition.AppendFormat("pj.priority_id = {0}", dataSearch.Priority_Id);
                }

                // เงื่อนไข jobtype name by id
                if (dataSearch?.Jobtype_Id != null)
                {
                    if (condition.Length > 0) condition.Append(" and ");
                    condition.AppendFormat("pjj.jobtype_id = {0}", dataSearch.Jobtype_Id);
                }
                string qField = "WITH jobtype_agg AS (" +
                    " select pjj.priority_jobtype_id, STRING_AGG(jt.jobtype_name, ', ') AS jobtype_name  " +
                    " from jobtype jt  " +
                    " inner join priority_jobtype_jobtype pjj ON jt.jobtype_id = pjj.jobtype_id  " +
                    " group by pjj.priority_jobtype_id\n" +
                    ")\n"+
                    "select " +
                    " pj.priority_jobtype_id , " +
                    " pj.priority_jobtype_code , " +
                    " pj.priority_id , " +
                    " p.priority_name , " +
                    " pj.service_time , " +
                    " pj.waiting_time , " +
                    " COALESCE(ja.jobtype_name, '') AS jobtype_name , " +
                    " pj.active , " +
                    " pj.team , " +
                    " concat(uc.\"name\",' ',uc.lastname ) as created_by , " +
                    " pj.created_dateTime , " +
                    " concat(um.\"name\",' ',um.lastname ) as modified_by , " +
                    " pj.modified_dateTime " +
                    " from priority_jobtype pj" +
                    " left join userinfo uc on pj.created_by = uc.user_code" +
                    " left join userinfo um on pj.modified_by = um.user_code" +
                    " left join priority p on p.priority_id = pj.priority_id" +
                    " left join jobtype_agg ja ON pj.priority_jobtype_id = ja.priority_jobtype_id";
                string query = qField + (condition.Length == 0 ? "" : $" where {condition}") +
                    $" ORDER BY modified_datetime OFFSET (({dataSearch?.PageNumber}-1)*{dataSearch?.PageSize}) ROWS FETCH NEXT {dataSearch?.PageSize} ROWS ONLY;\r\n";
                var totalRows = "select count(pj.priority_jobtype_id) as count_rows from priority p" +
                    (condition.Length == 0 ? "" : $" where {condition}");

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
