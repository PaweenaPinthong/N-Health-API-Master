using Microsoft.AspNetCore.Mvc;
using N_Health_API.Helper;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using N_Health_API.ServicesInterfece.Master;
using OfficeOpenXml;
using System.Drawing;

namespace N_Health_API.Services.Master
{
    public class ProductTypeService : IProductTypeService
    {
        private IConfiguration _config;
        private IProductTypeData _repo;

        public ProductTypeService(IConfiguration config, IProductTypeData repo)
        {
            _config = config;
            _repo = repo;
        }

        public async Task<MessageResponseModel> AddService(ProductTypeModel? data, string? userCode)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                var checkDup = await _repo.CheckDupData(data);
                if(Convert.ToBoolean(checkDup?.Data) == false)
                {
                    var result = await _repo.Add(data, userCode,false);
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

        public async  Task<MessageResponseModel> ChangeActiveService(int? id, bool? isActive, string? userCode)
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

        public async Task<MessageResponseModel> EditService(ProductTypeModel? data, string? userCode)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                var checkDup = await _repo.CheckDupData(data);
                if(Convert.ToBoolean(checkDup?.Data) == false)
                {
                    var result = await _repo.Edit(data, userCode);
                    if(result != false)
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
                ProductTypeModel? data = Util.ConvertDataTableToList<ProductTypeModel>(res).FirstOrDefault();
                meg_res.Success = true;
                meg_res.Message = ReturnMessage.SUCCESS;
                meg_res.Code = ReturnCode.SUCCESS;
                meg_res.Data = data;
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName = " - " + ex.Message + " - " + ex.StackTrace;
            }
            return meg_res;
        }

        public async Task<MessageResponseModel> SearchService(SearchProductTypeModel? data)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                var res = await _repo.Search(data);
                var recordData = Util.ConvertDataTableToList<ProductTypeSearch>(res.Item1);
                var countRows = res.Item2;
                var response = new PaginatedListModel<ProductTypeSearch>(recordData, (int)countRows, data.PageNumber, data.PageSize);

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
        
        public async Task<MessageResponseModel> ImportProductType(IFormFile? data,string? usercode)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                if (data == null || data.Length <= 0)
                {
                    meg_res.Success = false;
                    meg_res.Message = ReturnMessage.DATA_NOT_FOUND;
                    meg_res.Code = ReturnCode.DATA_NOT_FOUND;
                }
                if (!Path.GetExtension(data!.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase) &&
                    !Path.GetExtension(data.FileName).Equals(".xls", StringComparison.OrdinalIgnoreCase)
                )
                {
                    meg_res.Success = false;
                    meg_res.Message = "ไฟล์ที่อัปโหลดต้องเป็นไฟล์ xls หรือ xlsx เท่านั้น";
                    meg_res.Code = ReturnCode.SYSTEM_ERROR;
                }
                if (data.Length >= (1024.00 * 1024.00))
                {
                    meg_res.Success = false;
                    meg_res.Message = "ไฟล์ที่อัปโหลดต้องมีขนาดไม่เกิน 1 MB เท่านั้น";
                    meg_res.Code = ReturnCode.SYSTEM_ERROR;
                }


                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (MemoryStream ms = new MemoryStream())
                {
                    await data.CopyToAsync(ms);
                    using (ExcelPackage package = new ExcelPackage(ms))
                    {
                        ExcelWorksheet ws = package.Workbook.Worksheets[0];
                        List<ProductTypeModel> productTypes = new List<ProductTypeModel>();
                        List<string> productTypesDup = new List<string>();

                        int lastrow = 0;

                        for(int i = 2;i<= ws.Dimension.End.Row; i++)
                        {
                            if (!string.IsNullOrEmpty(ws.Cells["C" + i].Text))
                            {
                                lastrow = i;
                            }
                        }
                        for(int i=2;i<= lastrow; i++)
                        {
                            var model = new ProductTypeModel();
                            model.Product_Type_Code = ws.Cells["A" + i].Text;
                            model.Product_Type_Code_Interface = ws.Cells["B" + i].Text;
                            model.Product_Type_Name = ws.Cells["C" + i].Text;

                            var flagText = ws.Cells["D" + i].Text.Trim().ToLower();

                            if(flagText == "yes" || flagText == "1")
                            {
                                model.Sub_Product_Flag = true;
                            }else if(flagText == "no" || flagText == "0")
                            {
                                model.Sub_Product_Flag = false;
                            }
                            else
                            {
                                model.Sub_Product_Flag = false;
                            }

                            productTypes.Add(model);
                        }

                        foreach (var pt in productTypes)
                        {
                            var checkDup = await _repo.CheckDupData(pt);
                            if (Convert.ToBoolean(checkDup?.Data) == true)
                            {
                                productTypesDup.Add(pt.Product_Type_Name!);
                            }
                        }

                        if (productTypesDup.Count > 0)
                        {
                            meg_res.Success = false;
                            meg_res.Message = string.Format(ReturnMessage.DUPLICATE_DATA, String.Join(',', productTypesDup));
                            meg_res.Code = ReturnCode.DUPLICATE_DATA;
                            return meg_res;
                        }

                        foreach (var pt in productTypes)
                        {
                            var result = await _repo.Add(pt, usercode,true);
                            if (result == false)
                            {
                                meg_res.Success = false;
                                meg_res.Message = ReturnMessage.SYSTEM_ERROR;
                                meg_res.Code = ReturnCode.SYSTEM_ERROR;
                                return meg_res;
                            }
                        }

                        meg_res.Success = true;
                        meg_res.Message = ReturnMessage.SUCCESS;
                        meg_res.Code = ReturnCode.SUCCESS;
                    }
                }
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
            }
            return meg_res;
        }

        public async Task<MessageResponseModel> ExportProductType(SearchProductTypeModel? data, string? userCode)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                // ตั้งค่าให้ EPPlus ใช้ License แบบ Non-Commercial
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    // ตั้งชื่อหัวตาราง excel
                    var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells[1, 1].Value = "product_type_id";
                    worksheet.Cells[1, 2].Value = "product_type_code";
                    worksheet.Cells[1, 3].Value = "Product_type_code_interface";
                    worksheet.Cells[1, 4].Value = "product_type_name";
                    worksheet.Cells[1, 5].Value = "sub_product_flag";
                    worksheet.Cells[1, 6].Value = "active";
                    worksheet.Cells[1, 7].Value = "created_by";
                    worksheet.Cells[1, 8].Value = "created_name";
                    worksheet.Cells[1, 9].Value = "created_datetime";
                    worksheet.Cells[1, 10].Value = "update_by";
                    worksheet.Cells[1, 11].Value = "update_name";
                    worksheet.Cells[1, 12].Value = "update_datetime";

                    // เรียกข้อมูลใน db
                    var res = await _repo.SearchExport(data);
                    var recordData = Util.ConvertDataTableToList<ProductTypeSearch>(res.Item1);


                    // วนลูปข้อมูลลง cell ใน excel
                    for (int i = 0; i < recordData.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = recordData[i].Product_Type_Id;
                        worksheet.Cells[i + 2, 2].Value = recordData[i].Product_Type_Code;
                        worksheet.Cells[i + 2, 3].Value = recordData[i].Product_Type_Code_Interface;
                        worksheet.Cells[i + 2, 4].Value = recordData[i].Product_Type_Name;
                        worksheet.Cells[i + 2, 5].Value = recordData[i].Sub_Product_Flag;
                        worksheet.Cells[i + 2, 6].Value = recordData[i].Active;
                        worksheet.Cells[i + 2, 7].Value = recordData[i].Created_By;
                        worksheet.Cells[i + 2, 8].Value = recordData[i].Created_Name;
                        worksheet.Cells[i + 2, 9].Value = recordData[i].Created_Date_Str;
                        worksheet.Cells[i + 2, 10].Value = recordData[i].Update_By;
                        worksheet.Cells[i + 2, 11].Value = recordData[i].Update_Name;
                        worksheet.Cells[i + 2, 12].Value = recordData[i].Update_Date_Str;
                    }

                    // บันทึกข้อมูลลงใน MemoryStream แบบ gen excel เป็น base64
                    using (var stream = new MemoryStream())
                    {
                        package.SaveAs(stream);
                        var byteArray = stream.ToArray();

                        // แปลงข้อมูลเป็น Base64 string
                        var base64String = Convert.ToBase64String(byteArray);

                        meg_res.Success = true;
                        meg_res.Message = ReturnMessage.SUCCESS;
                        meg_res.Code = ReturnCode.SUCCESS;
                        meg_res.Data = base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
            }
            return meg_res;
        }
    }
}
