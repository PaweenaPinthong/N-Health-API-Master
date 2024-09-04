using N_Health_API.Helper;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using N_Health_API.ServicesInterfece.Master;

namespace N_Health_API.Services.Master
{
    public class ReasonService : IReasonService
    {
        private IConfiguration _config;
        private IReasonData _repo;

        public ReasonService(IConfiguration config, IReasonData repo)
        {
            _config = config;
            _repo = repo;
        }

        public async Task<MessageResponseModel> AddService(ReasonModel? data, string? userCode)
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
                    var result = await _repo.Add(data, userCode);
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

        public async Task<MessageResponseModel> EditService(ReasonModel? data, string? userCode)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                var checkDup = await _repo.CheckDupData(data);
                if (Convert.ToBoolean(checkDup?.Data) == false)
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
                    meg_res.Message = string.Format(ReturnMessage.DUPLICATE_DATA,checkDup.Message);
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
                ReasonModel? data = Util.ConvertDataTableToList<ReasonModel>(res).FirstOrDefault();
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

        public async Task<MessageResponseModel> SearchService(SearchReasonModel? data)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;

            try
            {
                var res = await _repo.Search(data);
                var recordData = Util.ConvertDataTableToList<ReasonSearch>(res.Item1);
                var countRows = res.Item2;
                var response = new PaginatedListModel<ReasonSearch>(recordData, (int)countRows, data.PageNumber, data.PageSize);

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
    }
}
