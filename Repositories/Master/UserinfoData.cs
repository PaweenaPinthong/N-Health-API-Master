using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using N_Health_API.ServicesInterfece;
using NpgsqlTypes;
using System.Data;

namespace N_Health_API.Repositories.Master
{
    public class UserinfoData : IUserinfoData
    {
        public async Task<bool> Add(UserinfoModel data, string userName)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var qUser = "INSERT INTO userinfo " +
                    "(user_id, user_code, \"password\", user_name, employee_id" +
                    ", prefix_name, \"name\", lastname" +
                    ", user_type, user_level, team" +
                    ", telephone_no, email" +
                    ", location_id, department_id, permission_id" +
                    ", active, created_by, created_datetime, modified_by, modified_datetime) " +
                    "VALUES(@user_id ,@user_code ,@password ,@user_name ,@employee_id " +
                    ",@prefix_name ,@name ,@lastname " +
                    ",@user_type ,@user_level ,@team " +
                    ",@telephone_no ,@email " +
                    ",@location_id ,@department_id ,@permission_id " +
                    ",@active ,@created_by ,@created_datetime ,@modified_by ,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "user_id", Value = data?.User_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "user_code", Value = data?.User_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "password", Value = data?.Password, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "user_name", Value = data?.User_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "employee_id", Value = data?.Employee_Id, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "prefix_name", Value = data?.Prefix_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "name", Value = data?.Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "lastname", Value = data?.Lastname, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "user_type", Value = data?.User_Type, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "user_level", Value = data?.User_Level, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "telephone_no", Value = data?.Telephone_No, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "email", Value = data?.Email, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "department_id", Value = data?.Department_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "permission_id", Value = data?.Permission_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userName, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userName, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                                
                result = DBSQLPostgre.SQLPostgresExecutionCommand(qUser, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ChangeActive(int id, bool isActive, string userName)
        {
            try
            {
                var query = "  UPDATE userinfo  " +
                            "   SET active = @active  " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where user_id = @user_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userName, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "user_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(UserinfoModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string? msg = string.Empty;
                var query = "select u.*" +
                            " from userinfo u " +
                            " where u.user_name = '{0}'";

                query = string.Format(query, data?.User_Name);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                if (result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = msg+ string.Format("(Usename : {0})", data?.User_Name);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserinfoModel> CheckUserByUserName(string userName)
        {
            UserinfoModel result = new UserinfoModel();

            try
            {
                var query = "SELECT user_id, user_code, \"password\", user_name, employee_id, prefix_name, \"name\", lastname " +
                        " FROM userinfo " +
                        " where user_name = '{0}'";

                query = string.Format(query, userName);

                var res = DBSQLPostgre.SQLPostgresSelectCommand(query);

                result.User_Id = Convert.ToInt32(res?.Rows[0]["user_id"]);
                result.User_Code = res?.Rows[0]["user_code"].ToString();
                result.Password = res?.Rows[0]["password"].ToString();
                result.Employee_Id = res?.Rows[0]["employee_id"].ToString();
                result.Prefix_Name = res?.Rows[0]["prefix_name"].ToString();
                result.Name = res?.Rows[0]["name"].ToString();
                result.Lastname = res?.Rows[0]["lastname"].ToString();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> CountData()
        {
            try
            {
                var query = "select  count(user_id) as count_rows from userinfo; ";
                var result = DBSQLPostgre.SQLPostgresExecuteScalar(query);
                return (int)result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Edit(UserinfoModel? data, string userName)
        {

            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                var qUser = "UPDATE userinfo " +
                    "SET user_code=@user_code, user_name=@user_name, employee_id=@employee_id" +
                    ", prefix_name=@prefix_name, \"name\"=@name, lastname=@lastname" +
                    ", user_type=@user_type, user_level=@user_level, team=@team, telephone_no=@telephone_no, email=@email" +
                    ", location_id=@location_id, department_id=@department_id, permission_id=@permission_id, active=@active" +
                    ", modified_by=@modified_by, modified_datetime=@modified_datetime WHERE user_id=@user_id;\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "user_id", Value = data?.User_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "user_code", Value = data?.User_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "user_name", Value = data?.User_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "employee_id", Value = data?.Employee_Id, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "prefix_name", Value = data?.Prefix_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "name", Value = data?.Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "lastname", Value = data?.Lastname, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "user_type", Value = data?.User_Type, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "user_level", Value = data?.User_Level, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "telephone_no", Value = data?.Telephone_No, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "email", Value = data?.Email, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "department_id", Value = data?.Department_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "permission_id", Value = data?.Permission_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userName, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });

                result = DBSQLPostgre.SQLPostgresExecutionCommand(qUser, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> GetById(int id)
        {

            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                var query = "select u.*" +
                            " from userinfo u " +
                            " where user_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<(DataTable, long)> Search(SearchUserInfoModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                var query = "SELECT u.* " +
                    ",concat(un.\"name\",' ',un.lastname ) as Full_Name" +
                    " FROM public.userinfo u " +
                    " inner join userinfo un on u.user_code = un.user_code ";
                string condition = string.Empty;

                if (data?.Active != null)//status
                {
                    condition = string.Format(condition + " u.active = {0}", data.Active);
                }

                if (!string.IsNullOrEmpty(data?.User_Name))//Username
                {
                    if (!string.IsNullOrEmpty(condition))
                    {
                        condition = condition + " and ";
                    }
                    condition = string.Format(condition + " u.user_name = '%{0}%'", data.User_Name);
                }
                if (!string.IsNullOrEmpty(data?.Employee_Id))//รหัสพนักงาน
                {
                    if (!string.IsNullOrEmpty(condition))
                    {
                        condition = condition + " and ";
                    }
                    condition = string.Format(condition + " u.employee_id like '%{0}%'", data.Employee_Id);
                }

                if (!string.IsNullOrEmpty(condition))
                {
                    query = query + " where " + condition;
                }
                query = query + $" ORDER BY u.user_name OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";

                ////query = query + "select  count(permission_id) as count_rows from permission; ";
                //var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                List<string> arrSql = new List<string>();
                arrSql.Add(query);
                arrSql.Add("select  count(user_id) as count_rows from userinfo; ");
                var result = await DBSQLPostgre.SQLPostgresSelectSearch(arrSql);


                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
