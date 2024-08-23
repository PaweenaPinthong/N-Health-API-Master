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
        public JobTypeController(IJobTypeService iJobTypeService)
        {
            this._iJobTypeService = iJobTypeService;

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
    }
}
