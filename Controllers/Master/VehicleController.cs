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
    [Route("Vehicle")]
    [SwaggerTag("ระบบจัดการข้อมูล Vehicle")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _iVehicle;
        private readonly IAccessTokenService _auth;

        public VehicleController(IVehicleService iVehicle, IAccessTokenService auth)
        {
            this._auth = auth;
            this._iVehicle = iVehicle;
        }

        [SwaggerOperation(Tags = new[] { "Vehicle" }, Summary = "เพิ่มข้อมูล Vehicle", Description = "เพิ่มข้อมูล Vehicle")]
        [HttpPost("Add")]
        public async Task<MessageResponseModel> Add([FromBody] VehicleModel model)
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

                var result = await _iVehicle.AddService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Vehicle" }, Summary = "แก้ไขข้อมูล Vehicle", Description = "แก้ไขข้อมูล Vehicle")]
        [HttpPost("Edit")]
        public async Task<MessageResponseModel> Edit([FromBody] VehicleModel model)
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

                var result = await _iVehicle.EditService(model, user_code);
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

        [SwaggerOperation(Tags = new[] { "Vehicle" }, Summary = "เปลี่ยนสถานะข้อมูล", Description = "เปลี่ยนสถานะข้อมูล Active(true) กับ Inactive(false)")]
        [HttpPost("ChangeActive")]
        public async Task<MessageResponseModel> ChangeActive([FromBody] VehicleRequest request)
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

                var result = await _iVehicle.ChangeActiveService(request.Vehicle_Id, request.Active, user_code);
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


        [SwaggerOperation(Tags = new[] { "Vehicle" }, Summary = "ดึงข้อมูลราย Record", Description = "ดึงข้อมูลราย Record")]
        [HttpPost("GetById")]
        public async Task<MessageResponseModel> GetById([FromBody] VehicleRequest request)
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

                var result = await _iVehicle.GetByIdService(request.Vehicle_Id);
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

        [SwaggerOperation(Tags = new[] { "Vehicle" }, Summary = "ค้นหาข้อมูล", Description = "ค้นหาข้อมูลหน้าหลัก")]
        [HttpPost("Search")]
        public async Task<MessageResponseModel> Search([FromBody] SearchVehicleModel data)
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

                var result = await _iVehicle.SearchService(data);
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
