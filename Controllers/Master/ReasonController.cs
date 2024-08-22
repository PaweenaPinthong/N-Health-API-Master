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
    [Route("Reason")]
    [SwaggerTag("ระบบจัดการข้อมูล Reason")]
    public class ReasonController : ControllerBase
    {
        private readonly IReasonService _iReason;
        private readonly IAccessTokenService _auth;

        public ReasonController(IReasonService ireason , IAccessTokenService auth)
        {
            this._iReason = ireason;
            this._auth = auth;
        }

        [SwaggerOperation(Tags = new[] { "Reason" }, Summary = "เพิ่มข้อมูล Reason",Description = "เพิ่มข้อมูล Reason")]
        [HttpPost("Add")]
        public async Task<MessageResponseModel> Add([FromBody] ReasonModel model)
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

                var result = await _iReason.AddService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Reason" }, Summary = "เปลี่ยนสถานะข้อมูล", Description = "เปลี่ยนสถานะข้อมูล")]
        [HttpPost("ChangeActive")]
        public async Task<MessageResponseModel> ChangeActive([FromBody] ReasonRequest request)
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

                var result = await _iReason.ChangeActiveService(request.Reason_Id,request.Active,user_code);
                msgResult.Code = result.Code;
                msgResult.Message = result.Message;
                msgResult.Success = result.Success;
                msgResult.Data = result.Data;

            }catch (Exception ex)
            {
                msgResult.Message = ex.Message;
            }
            return msgResult;

        }

        [SwaggerOperation(Tags = new[] { "Reason" }, Summary = "แก้ไขข้อมูล Reason", Description = "แก้ไขข้อมูล Reason")]
        [HttpPost("Edit")]
        public async Task<MessageResponseModel> Edit([FromBody] ReasonModel model)
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

                var result = await _iReason.EditService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Reason" }, Summary = "Get ข้อมูล by ID", Description = "Get ข้อมูล by ID")]
        [HttpGet("GetById")]
        public async Task<MessageResponseModel> GetById([FromQuery] ReasonRequest request)
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

                var result = await _iReason.GetByIdService(request.Reason_Id);
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

        [SwaggerOperation(Tags = new[] { "Reason" }, Summary = "Search Reason", Description = "Search Reason")]
        [HttpPost("Search")]
        public async Task<MessageResponseModel> Search([FromBody] SearchReasonModel data)
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

                var result = await _iReason.SearchService(data);
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
