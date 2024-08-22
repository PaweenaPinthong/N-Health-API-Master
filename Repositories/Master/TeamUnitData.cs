using N_Health_API.Core;
using N_Health_API.Helper;
using N_Health_API.Models;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Data;

namespace N_Health_API.Repositories.Master
{
    public class TeamUnitData : ITeamUnitData
    {
        public async Task<bool> Add(TeamUnitModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {
                List<string> arrSql = new List<string>();
                if (data != null)
                {
                    string typeStr = "VT";
                    var lastId = "select team_unit_id from team_unit where created_datetime is not null order by created_datetime desc limit 1";
                    data.Team_Unit_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                    arrSql.Add(lastId);
                }
                var query = "INSERT INTO team_unit " +
                    "(team_unit_id" +
                    ",team_unite_code" +
                    ",team_unit_name " +
                    ",location_id ,team " +
                    ", active, created_by, created_datetime, modified_by, modified_datetime) " +
                    "VALUES(" +
                    "@id " +
                    ",@code " +
                    ",@team_unit_name" +
                    ",@location_id ,@team " +
                    ",@active ,@created_by ,@created_datetime ,@modified_by ,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id", Value = data?.Team_Unit_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "code", Value = data?.Team_Unit_Code, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "team_unit_name", Value = data?.Team_Unit_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Team, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp });

                arrSql.Add(query);
                result = await DBSQLPostgre.SQLPostgresExecutionAddData(arrSql, parameters);
                //result = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
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
                var query = "  UPDATE \"team_unit\"     " +
                            "   SET active = @active  " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where team_unit_id = @team_unit_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active", Value = isActive, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "team_unit_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch
            {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(TeamUnitModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string? msg = string.Empty;
                var query = "select tu.*" +
                            " from team_unit tu " +
                            " where tu.team = '{0}' " +
                            " and tu.location_id = {1} " +
                            " and tu.team_unit_name = '{2}'" +
                            "" + (data?.Team_Unit_Id is not null ? " and team_unit_id = " + data?.Team_Unit_Id : "");

                query = string.Format(query, data?.Location_Id, data?.Team, data?.Team_Unit_Name);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);
                if (result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = msg + string.Format("(Location Name : {0})\r\n " +
                                                ",(Team : {1})\r\n" +
                                                ",(Unit : {2})\r\n" +
                                                "", data?.Location_Name, data?.Team,data?.Team_Unit_Name);
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

        public async Task<bool> Edit(TeamUnitModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();
            try
            {

                var query = "UPDATE team_unit SET" +
                    " team_unit_name = @team_unit_name" +
                    ",location_id = @location_id" +
                    ",team = @team" +
                    ",active = @active" +
                    ",modified_by = @modified_by " +
                    ",modified_datetime =@modified_datetime  " +
                    "where team_unit_id = @team_unit_id ;\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "team_unit_id", Value = data?.Team_Unit_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team_unit_name", Value = data?.Team_Unit_Name, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "location_id", Value = data?.Location_Id, Type = NpgsqlDbType.Integer });
                parameters.Add(new DBParameter { Name = "team", Value = data?.Team, Type = NpgsqlDbType.Varchar });
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
                            "tu.team_unit_id" +
                            ",tu.team_unit_code" +
                            ",tu.team_unit_name " +
                            ",tu.team" +
                            ",tu.created_datetime" +
                            ",tu.modified_datetime" +
                            ",tu.active" +
                            ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                            ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                            " from team_unit tu " +
                            " left join userinfo uc on tu.created_by = uc.user_code " +
                            " left join userinfo um on tu.modified_by = um.user_code "+
                            " where team_unit_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable,long)> Search(SearchTeamUnitModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string condition = string.Empty;

                //status
                condition = data?.Active != null ? string.Format(condition + " tu.active = {0}", data.Active) : " tu.active in (true,false)";
                //Team
                condition = condition + (!string.IsNullOrEmpty(data?.Team) ? string.Format(" and tu.team = '{0}'", data?.Team) : "");
                //Unit
                condition = condition + (!string.IsNullOrEmpty(data?.Team_Unit_Name) ? string.Format(" and tu.team_unit_name = '%{0}%'", data?.Team_Unit_Name) : "");

                string query = string.Empty;
                string qField = "select " +
                               "tu.team_unit_id" +
                               ",tu.team_unit_code" +
                               ",tu.team_unit_name " +
                               ",tu.team" +
                               ",tu.created_datetime" +
                               ",tu.modified_datetime" +
                               ",concat(uc.\"name\",' ',uc.lastname ) as created_by" +
                               ",concat(um.\"name\",' ',um.lastname ) as modified_by" +
                               ",lc.location_name";

                string qFromJoin = " from team_unit tu " +
                                 " left join location lc on tu.location_id = lc.location_id" +
                                 " left join userinfo uc on tu.created_by = uc.user_code " +
                                 " left join userinfo um on tu.modified_by = um.user_code ";

                query = qField + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}")
                        + $" ORDER BY tu.modified_datetime desc OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";

                var totalRows = "select  count(tu.team_unit_id) as count_rows " + qFromJoin + (string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

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
