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
    [Route("Cost")]
    [SwaggerTag("ระบบจัดการข้อมูล Cost")]
    public class CostController : ControllerBase
    {
        private readonly ICostService _iCost;
        private readonly IAccessTokenService _auth;

        public CostController(ICostService iCost, IAccessTokenService auth)
        {
            this._auth = auth;
            this._iCost = iCost;
        }

        [SwaggerOperation(Tags = new[] { "Cost" }, Summary = "เพิ่มข้อมูล Cost", Description = "เพิ่มข้อมูล Cost")]
        [HttpPost("Add")]
        public async Task<MessageResponseModel> Add([FromBody] CostDataModel model)
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

                var result = await _iCost.AddService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Cost" }, Summary = "แก้ไขข้อมูล Cost", Description = "แก้ไขข้อมูล Cost")]
        [HttpPost("Edit")]
        public async Task<MessageResponseModel> Edit([FromBody] CostDataModel model)
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

                var result = await _iCost.EditService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Cost" }, Summary = "เปลี่ยนสถานะข้อมูล", Description = "เปลี่ยนสถานะข้อมูล Active(true) กับ Inactive(false)")]
        [HttpPost("ChangeActive")]
        public async Task<MessageResponseModel> ChangeActive([FromBody] CostRequest request)
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

                var result = await _iCost.ChangeActiveService(request.Cost_Id, request.Active, user_code);
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


        [SwaggerOperation(Tags = new[] { "Cost" }, Summary = "ดึงข้อมูลราย Record", Description = "ดึงข้อมูลราย Record")]
        [HttpPost("GetById")]
        public async Task<MessageResponseModel> GetById([FromBody] CostRequest request)
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

                var result = await _iCost.GetByIdService(request.Cost_Id);
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

        [SwaggerOperation(Tags = new[] { "Cost" }, Summary = "ค้นหาข้อมูล", Description = "ค้นหาข้อมูลหน้าหลัก")]
        [HttpPost("Search")]
        public async Task<MessageResponseModel> Search([FromBody] SearchCostModel data)
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

                var result = await _iCost.SearchService(data);
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
