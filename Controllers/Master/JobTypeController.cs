using BusinessIdeaMasterAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.ServicesInterfece.Master;
using N_Health_API.ServicesInterfece.Shared;
using Swashbuckle.AspNetCore.Annotations;

namespace N_Health_API.Controllers.Master
{
    [Authorize]
    [Route("JobType")]
    public class JobTypeController : Controller
    {

        private readonly IJobTypeService _iJobTypeService;
        private readonly IAccessTokenService _auth;
        public JobTypeController(IJobTypeService iJobTypeService, IAccessTokenService auth)
        {
            this._iJobTypeService = iJobTypeService;
            this._auth = auth;

        }
        [SwaggerOperation(Tags = new[] { "JobType" }, Summary = "เพิ่มข้อมูล JobType", Description = "เพิ่มข้อมูล JobType")]
        [HttpPost("Add")]
        public async Task<MessageResponseModel> Add([FromBody] JobtypeDataReasone data)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;
            string? user_code = Request.Headers["UserCode"];


            try
            {
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);

                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }
                var result = await _iJobTypeService.AddService(data, user_code);
                msgResult.Code = result.Code;
                msgResult.Message = result.Message;
                msgResult.Success = result.Success;


            }
            catch (Exception ex)
            {
                msgResult.Message = ex.Message;
            }
            return msgResult;
        }

        [SwaggerOperation(Tags = new[] { "JobType" }, Summary = "แก้ไขข้อมูล JobType", Description = "แก้ไขข้อมูล JobType")]
        [HttpPost("Edit")]
        public async Task<MessageResponseModel> Edit([FromBody] JobtypeDataReasone data)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;
            string? user_code = Request.Headers["UserCode"];


            try
            {
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);

                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }
                var result = await _iJobTypeService.EditService(data, user_code);
                msgResult.Code = result.Code;
                msgResult.Message = result.Message;
                msgResult.Success = result.Success;
            }
            catch (Exception ex)
            {
                msgResult.Message = ex.Message;
            }
            return msgResult;
        }

        [SwaggerOperation(Tags = new[] { "JobType" }, Summary = "ค้นหาข้อมูล", Description = "ค้นหาข้อมูลหน้าหลัก")]
        [HttpPost("Search")]
        public async Task<MessageResponseModel> SearchJobData([FromBody] SearchJobtypeModel data)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;
            string? user_code = Request.Headers["UserCode"];

            try
            {
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);

                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }
                var result = await _iJobTypeService.SearchService(data);
                msgResult.Code = result.Code;
                msgResult.Message = result.Message;
                msgResult.Success = result.Success;
                msgResult.Data = result.Data;
            }
            catch (Exception ex)
            {
                msgResult.Message = ex.Message;
            }
            return msgResult;
        }

        [SwaggerOperation(Tags = new[] { "GetById" }, Summary = "Get ข้อมูล by ID", Description = "Get ข้อมูล by ID")]
        [HttpPost("GetById")]
        public async Task<MessageResponseModel> GetById([FromBody] JobtypeRequest request)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;

            try
            {
                // header 
                string? user_code = Request.Headers["UserCode"];
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }

                var result = await _iJobTypeService.GetByIdService(request.Jobtype_Id);
                msgResult.Code = result.Code;
                msgResult.Message = result.Message;
                msgResult.Success = result.Success;
                msgResult.Data = result.Data;
            }
            catch (Exception ex)
            {
                msgResult.Message = ex.Message;
            }

            return msgResult;
        }

        [SwaggerOperation(Tags = new[] { "ChangeActive" }, Summary = "เปลี่ยนสถานะข้อมูล", Description = "เปลี่ยนสถานะข้อมูล")]
        [HttpPost("ChangeActive")]
        public async Task<MessageResponseModel> ChangeActive([FromBody] JobtypeRequest request)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;

            try
            {
                // header
                string? user_code = Request.Headers["UserCode"];
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }

                var result = await _iJobTypeService.ChangeActiveService(request.Jobtype_Id, request.Active, user_code);
                msgResult.Code = result.Code;
                msgResult.Message = result.Message;
                msgResult.Success = result.Success;
                msgResult.Data = result.Data;

            }
            catch (Exception ex)
            {
                msgResult.Message = ex.Message;
            }
            return msgResult;

        }

        [SwaggerOperation(Tags = new[] { "Import Excel" }, Summary = "อัพโหลดไฟล์ excel", Description = "อัพโหลดไฟล์ excel")]
        [HttpPost("Import Excel")]
        public async Task<MessageResponseModel> ImportExcel(IFormFile FileExcel)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            string? user_code = Request.Headers["UserCode"];
            var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
            try
            {
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }
                msgResult = await _iJobTypeService.ImportOrderService(FileExcel, user_code!);

            }
            catch (Exception ex)
            {
                msgResult.Message = ex.Message;
                return msgResult;
            }
            return msgResult;

        }
    }
}
