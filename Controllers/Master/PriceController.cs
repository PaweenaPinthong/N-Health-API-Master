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
    [Route("Price")]
    [SwaggerTag("ระบบจัดการข้อมูล Price")]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _iPrice;
        private readonly IAccessTokenService _auth;

        public PriceController(IPriceService iPrice, IAccessTokenService _auth)
        {
            this._auth = _auth;
            this._iPrice = iPrice;
        }

        [SwaggerOperation(Tags = new[] { "Price" }, Summary = "เพิ่มข้อมูล Price", Description = "เพิ่มข้อมูล Price")]
        [HttpPost("Add")]
        public async Task<MessageResponseModel> Add([FromBody] PriceModel model)
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

                var result = await _iPrice.AddService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Price" }, Summary = "แก้ไขข้อมูล Price", Description = "แก้ไขข้อมูล Price")]
        [HttpPost("Edit")]
        public async Task<MessageResponseModel> Edit([FromBody] PriceModel model)
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

                var result = await _iPrice.EditService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Price" }, Summary = "เปลี่ยนสถานะข้อมูล", Description = "เปลี่ยนสถานะข้อมูล Active(true) กับ Inactive(false)")]
        [HttpPost("ChangeActive")]
        public async Task<MessageResponseModel> ChangeActive([FromBody] PriceRequest request)
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

                var result = await _iPrice.ChangeActiveService(request.Price_Id, request.Active, user_code);
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


        [SwaggerOperation(Tags = new[] { "Price" }, Summary = "ดึงข้อมูลราย Record", Description = "ดึงข้อมูลราย Record")]
        [HttpPost("GetById")]
        public async Task<MessageResponseModel> GetById([FromBody] PriceRequest request)
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

                var result = await _iPrice.GetByIdService(request.Price_Id);
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

        [SwaggerOperation(Tags = new[] { "Price" }, Summary = "ค้นหาข้อมูล", Description = "ค้นหาข้อมูลหน้าหลัก")]
        [HttpPost("Search")]
        public async Task<MessageResponseModel> Search([FromBody] SearchPriceModel data)
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

                var result = await _iPrice.SearchService(data);
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
