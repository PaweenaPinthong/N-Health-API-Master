using Microsoft.IdentityModel.Tokens;
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
    public class ReasonData : IReasonData
    {
        public async Task<bool> Add(ReasonModel? data, string? userCode)
        {
            bool result = false;

            try {
                List<String> arrSql = new List<string>();
                DateTime dateTime = new DateTimeUtils().NowDateTime();
                if(data != null)
                {
                    string typeStr = "RE";
                    var lastId = "select reason_id from reason where created_datetime is not null order by created_datetime desc limit 1";
                    data.Reason_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);
                }

                var qInst = "INSERT INTO reason" +
                    "(reason_id" +
                    ",reason_code" +
                    ",reason_name" +
                    ",active, created_by, created_datetime, modified_by, modified_datetime)" + 
                    "VALUES(" +
                    "@id " +
                    ",@code " +
                    ",@reason_name " +
                    ",@active ,@created_by ,@created_datetime ,@modified_by ,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id", Value = data?.Reason_Id, Type = NpgsqlDbType.Integer});
                parameters.Add(new DBParameter { Name = "code", Value = data?.Reason_Code, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "reason_name", Value = data?.Reason_Name, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean});
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp});
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp});

                arrSql.Add(qInst);
                result = await DBSQLPostgre.SQLPostgresExecutionAddData(arrSql,parameters);

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
                var query = " UPDATE \"reason\"  " +
                            " SET active = @active " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where reason_id = @reason_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active" , Value = isActive , Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode , Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "reason_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query,parameters);
                return res;
            }
            catch 
            {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(ReasonModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                String? msg = string.Empty;
                var query = "select r.*" +
                            " from reason r " +
                            " where r.reason_name = '{0}'" + (data?.Reason_Id is null || (data?.Reason_Id <= 0) ? "" : " and r.reason_id != " + data?.Reason_Id);

                query = string.Format(query, data?.Reason_Name);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                if(result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = msg + string.Format("(Reason Name : {0})",data?.Reason_Name);
                    meg_res.Success = true;
                    meg_res.Message = msg;
                    meg_res.Code = ReturnCode.DUPLICATE_DATA;
                    meg_res.Data = true;
                    return meg_res;
                }
                else
                {
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

        public async Task<bool> Edit(ReasonModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();

            try
            {
                var query = " UPDATE reason " +
                        " SET reason_name = @reason_name " +
                        " ,active = @active " +
                        " ,modified_by = @modified_by " +
                        " ,modified_datetime = @modified_datetime " +
                        "where reason_id = @reason_id";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "reason_id", Value = data?.Reason_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "reason_name", Value = data?.Reason_Name, Type = NpgsqlDbType.Varchar });
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
                var query = "select * from reason where reason_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchReasonModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string condition = string.Empty;
                string query = string.Empty;

                if (data?.Active != null)//status
                {
                    condition = string.Format(condition + " r.active = {0}", data.Active);
                }

                if (!string.IsNullOrEmpty(data?.Reason_Name))
                {
                    condition += (condition.Length > 0 ? " and " : "") + $"r.reason_name like '%{data.Reason_Name}%'";
                }

                if (data?.Datetime != null)
                {
                   var date = Util.ConvertDateTHToString(data?.Datetime);
                    if (condition.Length > 0)
                    {
                        condition = condition + string.Format(" and DATE(r.created_datetime) = '{0}' ", date);
                    }
                    else
                    {
                        condition = condition + string.Format(" DATE(r.created_datetime) = '{0}' ", date);
                    }

                }

                string qField = "select " +
                              " r.reason_id " +
                              " ,r.reason_code " +
                              " ,r.reason_name " +
                              " ,r.active " +
                              " ,r.created_datetime " +
                              " ,TO_CHAR(r.created_datetime::timestamp, 'DD/MM/YYYY') AS Created_Date_Str " +
                              " ,r.created_by as Created_By " +
                              " ,CONCAT(uc.\"name\",' ',uc.lastname) as Created_Name " +
                              " ,r.modified_datetime " +
                              " ,TO_CHAR(r.modified_datetime ::timestamp, 'DD/MM/YYYY') AS Update_Date_Str " +
                              " ,r.modified_by as Update_By " +
                              " ,CONCAT(um.\"name\",' ',um.lastname) as Update_Name ";
                
                string qJoin = " from reason r left join userinfo uc on r.created_by = uc.user_code " +
                               " left join userinfo um on r.modified_by = um.user_code ";

                query = qField + qJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}") +
                        $" ORDER BY r.modified_datetime DESC OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";

                var totalRows = "select count(r.reason_id) as count_rows " + qJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

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
