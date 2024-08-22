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
    [Route("PriorityJobType")]
    [SwaggerTag("ระบบจัดการข้อมูล Priority Job Type")]
    public class PriorityJobTypeController : ControllerBase
    {
        private readonly IPriorityJobTypeService _iPriorityJobType;
        private readonly IAccessTokenService _auth;

        public PriorityJobTypeController(IPriorityJobTypeService iPriorityJobType, IAccessTokenService _auth)
        {
            this._auth = _auth;
            this._iPriorityJobType = iPriorityJobType;
        }


        [SwaggerOperation(Tags = new[] { "Priority Job Type" }, Summary = "เพิ่มข้อมูล Priority Job Type", Description = "เพิ่มข้อมูล Priority Job Type")]
        [HttpPost("Add")]
        public async Task<MessageResponseModel> Add([FromBody] PriorityJobtypeDataModel model)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;

            try
            {
                //----------------validation header--------------------------
                string? user_code = Request.Headers["UserCode"];
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }

                var result = await _iPriorityJobType.AddService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Priority Job Type" }, Summary = "แก้ไขข้อมูล Priority Job Type", Description = "แก้ไขข้อมูล Priority Job Type")]
        [HttpPost("Edit")]
        public async Task<MessageResponseModel> Edit([FromBody] PriorityJobtypeDataModel model)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;

            try
            {
                //----------------validation header--------------------------
                string? user_code = Request.Headers["UserCode"];
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }

                var result = await _iPriorityJobType.EditService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Priority Job Type" }, Summary = "เปลี่ยนสถานะข้อมูล", Description = "เปลี่ยนสถานะข้อมูล Active(true) กับ Inactive(false)")]
        [HttpPost("ChangeActive")]
        public async Task<MessageResponseModel> ChangeActive([FromBody] PriorityJobtypeRequest request)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;

            try
            {

                //----------------validation header--------------------------
                string? user_code = Request.Headers["UserCode"];
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }

                var result = await _iPriorityJobType.ChangeActiveService(request.Priority_Jobtype_Id, request.Active, user_code);
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

        [SwaggerOperation(Tags = new[] { "Priority Job Type" }, Summary = "ดึงข้อมูลราย Record", Description = "ดึงข้อมูลราย Record")]
        [HttpGet("GetById")]
        public async Task<MessageResponseModel> GetById([FromQuery] int id)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;

            try
            {
                //----------------validation header--------------------------
                string? user_code = Request.Headers["UserCode"];
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }

                var result = await _iPriorityJobType.GetByIdService(id);
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

        [SwaggerOperation(Tags = new[] { "Priority Job Type" }, Summary = "ค้นหาข้อมูล", Description = "ค้นหาข้อมูลหน้าหลัก")]
        [HttpPost("Search")]
        public async Task<MessageResponseModel> Search([FromBody] SearchPriorityJobtypeModel data)
        {
            MessageResponseModel msgResult = new MessageResponseModel();
            msgResult.Code = ReturnCode.SYSTEM_ERROR;
            msgResult.Message = ReturnMessage.SYSTEM_ERROR;
            msgResult.Success = false;

            try
            {
                //----------------validation header--------------------------
                string? user_code = Request.Headers["UserCode"];
                var checkAuth_RES = await _auth.CheckAuthService(Request.Headers[HeaderNames.Authorization], user_code);
                if (!checkAuth_RES.Success)
                {
                    return checkAuth_RES;
                }

                var result = await _iPriorityJobType.SearchService(data);
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
