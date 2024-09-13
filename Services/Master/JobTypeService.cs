using BusinessIdeaMasterAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using N_Health_API.Helper;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using N_Health_API.ServicesInterfece.Master;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace N_Health_API.Services.Master
{

    public class JobTypeService : IJobTypeService
    {

        private IConfiguration _config;
        private IJobTypeData _repo;
        public JobTypeService(IConfiguration config, IJobTypeData repo)
        {
            _config = config;
            _repo = repo;
        }


        public async Task<MessageResponseModel> AddService(JobtypeDataReasone? data, string? userCode)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            string methodName = Util.GetMethodName();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;
            try
            {
                var checkDup = await _repo.CheckDupData(data);
                if (!Convert.ToBoolean(checkDup?.Data))
                {
                    var result = await _repo.Add(data, userCode, false);
                    if (result != false)
                    {
                        meg_res.Success = true;
                        meg_res.Message = ReturnMessage.SUCCESS;
                        meg_res.Code = ReturnCode.SUCCESS;
                        meg_res.Data = result;
                    }
                }
                else
                {
                    meg_res.Success = false;
                    meg_res.Message = string.Format(ReturnMessage.DUPLICATE_DATA, checkDup.Message);
                    meg_res.Code = ReturnCode.DUPLICATE_DATA;
                }

            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
            }
            return meg_res;
        }

        public async Task<MessageResponseModel> EditService(JobtypeDataReasone? data, string? userCode)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            string methodName = Util.GetMethodName();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;
            try
            {
                var checkDup = await _repo.CheckDupData(data);
                if (!Convert.ToBoolean(checkDup?.Data))
                {
                    var result = await _repo.Edit(data, userCode);
                    if (result != false)
                    {
                        meg_res.Success = true;
                        meg_res.Message = ReturnMessage.SUCCESS;
                        meg_res.Code = ReturnCode.SUCCESS;
                        meg_res.Data = result;
                    }
                }
                else
                {
                    meg_res.Success = false;
                    meg_res.Message = string.Format(ReturnMessage.DUPLICATE_DATA, checkDup.Message);
                    meg_res.Code = ReturnCode.DUPLICATE_DATA;
                }

            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
            }
            return meg_res;
        }

        public async Task<MessageResponseModel> SearchService(SearchJobtypeModel? data)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;
            try
            {
                var res = await _repo.SearchJobData(data);
                var recordData = Util.ConvertDataTableToList<JobtypeModel>(res.Item1);
                var countRows = res.Item2;
                var response = new PaginatedListModel<JobtypeModel>(recordData, (int)countRows, data.PageNumber, data.PageSize);

                meg_res.Success = true;
                meg_res.Message = ReturnMessage.SUCCESS;
                meg_res.Code = ReturnCode.SUCCESS;
                meg_res.Data = response;
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
            }

            return meg_res;
        }

        public async Task<MessageResponseModel> GetByIdService(int? id)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                var res = await _repo.GetById(id);
                JobtypeModelGetBy? data = Util.ConvertDataTableToList<JobtypeModelGetBy>(res).FirstOrDefault();
                // string jsonArray = data.Reason_List;
                string jsonArray = "[" + string.Join(",", data.Reason_List) + "]";

                // Deserialize JSON array to List<JobtypeReasonsList>
                List<JobtypeReasonsList> reasonsList = JsonConvert.DeserializeObject<List<JobtypeReasonsList>>(jsonArray);

                data.Reason_List_Value = reasonsList;
                meg_res.Success = true;
                meg_res.Message = ReturnMessage.SUCCESS;
                meg_res.Code = ReturnCode.SUCCESS;
                meg_res.Data = data;
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
            }
            return meg_res;
        }

        public async Task<MessageResponseModel> ChangeActiveService(int? id, bool? isActive, string? userCode)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                var result = await _repo.ChangeActive(id, isActive, userCode);

                meg_res.Success = true;
                meg_res.Message = ReturnMessage.SUCCESS;
                meg_res.Code = ReturnCode.SUCCESS;
                meg_res.Data = result;

            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
            }
            return meg_res;
        }

        public async Task<MessageResponseModel> ImportOrderService([FromForm] IFormFile fileExcel, string userCode)
        {

            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            JobtypeDataReasone data = new JobtypeDataReasone();
            List<JobtypeDataReasone> dataList = [];
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                using (MemoryStream ms = new MemoryStream())
                {
                    await fileExcel.CopyToAsync(ms);
                    using (ExcelPackage package = new ExcelPackage(ms))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // เลือก Sheet แรก
                        ExcelWorksheet worksheet2 = package.Workbook.Worksheets[1]; // เลือก Sheet สอง
                        int rowCount = worksheet.Dimension.Rows; // จำนวนแถวทั้งหมด
                        int rowCount2 = worksheet2.Dimension.Rows; // จำนวนแถวทั้งหมด

                        // insert value from import to model JobtypDataReasone

                        for (int row = 2; row <= rowCount; row++) // เริ่มจากแถวที่ 2
                        {
                            var uCode = worksheet?.Cells[row, 1]?.Value.ToString()?.Trim();
                            var uCode2 = worksheet2?.Cells[row, 1]?.Value.ToString()?.Trim();
                            if (data.jobtypeModel != null)
                            {
                                data.jobtypeModel.Jobtype_Code = worksheet?.Cells[row, 1]?.Value.ToString()?.Trim();
                                data.jobtypeModel.Jobtype_Name = worksheet?.Cells[row, 2]?.Value.ToString()?.Trim();
                                data.jobtypeModel.Jobtype_Desc = worksheet?.Cells[row, 3]?.Value.ToString()?.Trim();
                                data.jobtypeModel.Location_Id = int.Parse(worksheet?.Cells[row, 4]?.Value.ToString()?.Trim() ?? "1");
                                data.jobtypeModel.Team = worksheet?.Cells[row, 5]?.Value?.ToString()?.Trim();
                                data.jobtypeModel.Active = worksheet?.Cells[row, 6]?.Value?.ToString()?.Trim().Equals("TRUE", StringComparison.OrdinalIgnoreCase);
                                if (uCode == uCode2)
                                {
                                    for (int row2 = 2; row2 <= rowCount2; row2++) // เริ่มจากแถวที่ 2
                                    {
                                        data.jobtypeReasons?.Add(new JobtypeReasonModel { Reason_Id = int.Parse(worksheet2?.Cells[row2, 2]?.Value.ToString()?.Trim() ?? "1") });
                                    }
                                }

                                dataList.Add(data);
                                data = new JobtypeDataReasone();
                            }
                        };

                        /// loop for Check Duplicate Data      
                        foreach (var jobtype in dataList)
                        {
                            /// Check Duplicate Data                       
                            var checkDup = await _repo.CheckDupData(jobtype);
                            if (!Convert.ToBoolean(checkDup?.Data))
                            {
                                var result = await _repo.Add(jobtype, userCode, true);
                                meg_res.Success = true;
                                meg_res.Message = ReturnMessage.SUCCESS;
                                meg_res.Code = ReturnCode.SUCCESS;
                                meg_res.Data = result;
                            }
                            else
                            {
                                meg_res.Success = false;
                                meg_res.Message = string.Format(ReturnMessage.DUPLICATE_DATA, checkDup.Message);
                                meg_res.Code = ReturnCode.DUPLICATE_DATA;
                                return meg_res;
                            }
                        }
                    }
                }
                return meg_res;
            }
            catch (Exception ex)
            {
                return meg_res;
            }
        }
    }

}