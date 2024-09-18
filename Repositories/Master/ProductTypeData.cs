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
    public class ProductTypeData : IProductTypeData
    {
        public async Task<bool> Add(ProductTypeModel? data, string? userCode, bool? isImportexcel)
        {
            bool result = false;

            try
            {
                List<String> arrSql = new List<string>();
                DateTime dateTime = new DateTimeUtils().NowDateTime();
               
                if(data != null)
                {
                    if(isImportexcel == false)
                    {
                        string typeStr = "PT";
                        var lastId = "select product_type_id from product_type where created_datetime is not null order by created_datetime desc limit 1";
                        data.Product_Type_Code = $"{typeStr}{dateTime.ToString("MMyyyy-")}";
                        arrSql.Add(lastId);
                    }
                    else
                    {
                        var lastIm= "select product_type_id from product_type where created_datetime is not null order by created_datetime desc limit 1";
                        data.Product_Type_Code = data.Product_Type_Code;
                        arrSql.Add(lastIm);
                    }
                    
                }

                var qInst = "INSERT INTO product_type " +
                            "(product_type_id" +
                            ",product_type_code" +
                            ",product_type_code_interface" +
                            ",product_type_name" +
                            ",sub_product_flag" +
                            ",active, created_by, created_datetime, modified_by, modified_datetime)" +
                            "VALUES(" +
                            "@id " +
                            ",@code " +
                            ",@product_type_code_interface " +
                            ",@product_type_name " +
                            ",@sub_product_flag " +
                            ",@active ,@created_by ,@created_datetime ,@modified_by ,@modified_datetime);\r\n";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "id",Value = data?.Product_Type_Id, Type = NpgsqlDbType.Integer});
                parameters.Add(new DBParameter { Name = "code", Value = data?.Product_Type_Code, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "product_type_code_interface", Value = data?.Product_Type_Code_Interface, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "product_type_name", Value = data?.Product_Type_Name, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "sub_product_flag", Value = data?.Sub_Product_Flag, Type = NpgsqlDbType.Boolean });
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean});
                parameters.Add(new DBParameter { Name = "created_by", Value = userCode, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "created_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp});
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp});

                arrSql.Add(qInst);
                result = await DBSQLPostgre.SQLPostgresExecutionAddData(arrSql, parameters);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> ChangeActive(int? id, bool? isActive, string? userCode)
        {
            try
            {
                var query = " UPDATE \"product_type\" " +
                            " SET active = @active " +
                            ", modified_by = @modified_by " +
                            ", modified_datetime = @modified_datetime  " +
                            " where product_type_id = @product_type_id";

                query = string.Format(query, id);

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "active" , Value = isActive , Type = NpgsqlDbType.Boolean});
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar });
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = new DateTimeUtils().NowDateTime(), Type = NpgsqlDbType.Timestamp });
                parameters.Add(new DBParameter { Name = "product_type_id", Value = id, Type = NpgsqlDbType.Integer });

                var res = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return res;
            }
            catch {
                throw;
            }
        }

        public async Task<MessageResponseModel> CheckDupData(ProductTypeModel? data)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                string msg = string.Empty;
                var query = "select pt.*" +
                            " from product_type pt " +
                            " where replace(pt.product_type_name,' ','') =  replace('{0}',' ','') " + (data?.Product_Type_Id is null || (data?.Product_Type_Id <= 0) ? "" : " and pt.product_type_id != " + data?.Product_Type_Id);

                query = string.Format(query, data?.Product_Type_Name);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                if(result != null && result?.AsEnumerable().Count() > 0)
                {
                    msg = msg + string.Format("(Product Type Name : {0})", data?.Product_Type_Name);
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

        public async Task<bool> Edit(ProductTypeModel? data, string? userCode)
        {
            bool result = false;
            DateTime dateTime = new DateTimeUtils().NowDateTime();

            try
            {
                var query = " UPDATE product_type " +
                            " SET product_type_code_interface = @product_type_code_interface " +
                            " ,product_type_name = @product_type_name " +
                            " ,sub_product_flag = @sub_product_flag " +
                            " ,active = @active " +
                            " ,modified_by = @modified_by " +
                            " ,modified_datetime = @modified_datetime " +
                            "where product_type_id = @product_type_id";

                List<DBParameter> parameters = new List<DBParameter>();
                parameters.Add(new DBParameter { Name = "product_type_id", Value = data?.Product_Type_Id, Type = NpgsqlDbType.Integer});
                parameters.Add(new DBParameter { Name = "product_type_code_interface", Value = data?.Product_Type_Code_Interface , Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "product_type_name", Value = data?.Product_Type_Name, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "sub_product_flag", Value = data?.Sub_Product_Flag, Type = NpgsqlDbType.Boolean});
                parameters.Add(new DBParameter { Name = "active", Value = data?.Active, Type = NpgsqlDbType.Boolean});
                parameters.Add(new DBParameter { Name = "modified_by", Value = userCode, Type = NpgsqlDbType.Varchar});
                parameters.Add(new DBParameter { Name = "modified_datetime", Value = dateTime, Type = NpgsqlDbType.Timestamp});

                result = DBSQLPostgre.SQLPostgresExecutionCommand(query, parameters);
                return result;
            }
            catch
            {
                throw;
            }

        }

        public async Task<DataTable> GetById(int? id)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            try
            {
                var query = "select * from product_type where product_type_id = {0}";

                query = string.Format(query, id);

                var result = DBSQLPostgre.SQLPostgresSelectCommand(query);

                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<string>> QuerySearch(SearchProductTypeModel? data)
        {
            try
            {
                string condition = string.Empty;
                string query = string.Empty;

                List<string> querylist = new List<string>();

                if (data?.Active != null)
                {
                    condition = string.Format(condition + " p.active = {0} ", data.Active);
                }

                if (!string.IsNullOrEmpty(data?.Product_Type_Code))
                {
                    condition += (condition.Length > 0 ? " and " : "") + $"p.product_type_code like '%{data.Product_Type_Code}%'";
                }

                if (!string.IsNullOrEmpty(data?.Product_Type_Name))
                {
                    condition += (condition.Length > 0 ? " and " : "") + $"p.product_type_name like '%{data.Product_Type_Name}%'";
                }

                string qField = "select " +
                               " p.product_type_id " +
                               " ,p.product_type_code " +
                               " ,p.product_type_name " +
                               " ,p.active " +
                               " ,p.created_datetime " +
                               " ,to_char(p.created_datetime:: timestamp,'DD/MM/YYYY') as created_date_str " +
                               " ,p.created_by " +
                               " ,concat(uc.\"name\",' ',uc.lastname) as  created_name " +
                               " ,p.modified_datetime as update_date " +
                               " ,to_char(p.modified_datetime :: timestamp,'DD/MM/YYYY') as  update_date_str " +
                               " ,p.modified_by as update_by " +
                               " ,concat(um.\"name\",' ',um.lastname) as  update_name ";

                string qJoin = " from  product_type p left join userinfo uc on p.modified_by = uc.user_code " +
                               " left join userinfo um on p.modified_by = um.user_code ";

                query = qField + qJoin +(string.IsNullOrEmpty(condition) ? "" : $" where {condition}");

                querylist.Insert(0, condition);
                querylist.Insert(1, query);

                return querylist;

            }
            catch
            {
                throw;
            }
        }

        public async Task<(DataTable, long)> Search(SearchProductTypeModel? data)
        {
            MessageResponseModel mrg_res = new MessageResponseModel();
            try
            {
                string query = string.Empty;

                List<string> querySearch = new List<string>();

                querySearch = await QuerySearch(data);

                string qJoin = " from  product_type p left join userinfo uc on p.modified_by = uc.user_code " +
                               " left join userinfo um on p.modified_by = um.user_code ";

                query = querySearch[1] +
                        $" ORDER BY p.modified_datetime DESC OFFSET (({data?.PageNumber}-1)*{data?.PageSize}) ROWS FETCH NEXT {data?.PageSize} ROWS ONLY;\r\n";

                var totalRows = "select count(p.product_type_id) as count_rows " + qJoin + (string.IsNullOrEmpty(querySearch[0]) ? "" : $" where {querySearch[0]}");

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

        public async Task<(DataTable, long)> SearchExport(SearchProductTypeModel? data)
        {
            MessageResponseModel mrg_res = new MessageResponseModel();
            try
            {
                string query = string.Empty;

                List<string> querySearch = new List<string>();

                querySearch = await QuerySearch(data);

                query = querySearch[1] + $" ORDER BY p.modified_datetime DESC;\r\n";

                List<string> arrSql = new List<string>();
                arrSql.Add(query);
           
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
