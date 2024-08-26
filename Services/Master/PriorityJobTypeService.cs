using N_Health_API.Helper;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using N_Health_API.ServicesInterfece.Master;

namespace N_Health_API.Services.Master
{
    public class PriorityJobTypeService : IPriorityJobTypeService
    {
        private IConfiguration _config;
        private IPriorityJobTypeData _repo;

        public PriorityJobTypeService(IConfiguration config, IPriorityJobTypeData repo)
        {
            _config = config;
            _repo = repo;
        }


        public async Task<MessageResponseModel> AddService(PriorityJobtypeDataModel data, string? userCode)
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
                    var result = await _repo.Add(data, userCode);
                    if (result != false)
                    {
                        meg_res.Success = true;
                        meg_res.Message = ReturnMessage.SUCCESS;
                        meg_res.Code = ReturnCode.SUCCESS;
                        meg_res.Data = result;
                    }
                    return meg_res;
                }
                else {
                    meg_res.Success = false;
                    meg_res.Message = string.Format(ReturnMessage.DUPLICATE_DATA, checkDup.Message);
                    meg_res.Code = ReturnCode.DUPLICATE_DATA;
                    return meg_res;
                }
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
                return meg_res;
            }
        }

        public async Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userCode)
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
                return meg_res;
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
                return meg_res;
            }
        }

        public async Task<MessageResponseModel> EditService(PriorityJobtypeDataModel data, string? userCode)
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
                    if (result != false)
                    {
                        meg_res.Success = true;
                        meg_res.Message = ReturnMessage.SUCCESS;
                        meg_res.Code = ReturnCode.SUCCESS;
                        meg_res.Data = result;
                    }
                    return meg_res;
                }
                else {
                    meg_res.Success = false;
                    meg_res.Message = string.Format(ReturnMessage.DUPLICATE_DATA, checkDup.Message);
                    meg_res.Code = ReturnCode.DUPLICATE_DATA;
                    return meg_res;
                }
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
                return meg_res;
            }
        }

        public async Task<MessageResponseModel> GetByIdService(int id)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;
            try
            {
                var res = await _repo.GetById(id);
                var resJobType = await _repo.GetListJobTypeById(id);
                PriorityJobtypeDataModelById data = new PriorityJobtypeDataModelById();
                PriorityJobtypeModelById? pJobType = Util.ConvertDataTableToList<PriorityJobtypeModelById>(res).FirstOrDefault();
                List<PriorityJobtypeJobtypeModelById> pJobTypelist = Util.ConvertDataTableToList<PriorityJobtypeJobtypeModelById>(resJobType);
                data.PriorityJobtype = pJobType;
                data.Jobtype = pJobTypelist;
                meg_res.Success = true;
                meg_res.Message = ReturnMessage.SUCCESS;
                meg_res.Code = ReturnCode.SUCCESS;
                meg_res.Data = data;
                return meg_res;
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
                return meg_res;
            }
        }

        public async Task<MessageResponseModel> SearchService(SearchPriorityJobtypeModel? dataSearch)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;
            try
            {
                var res = await _repo.Search(dataSearch);
                var recordData = Util.ConvertDataTableToList<SearchPriorityJobtypeResponseModel>(res.Item1);
                var countRows = res.Item2;
                var response = new PaginatedListModel<SearchPriorityJobtypeResponseModel>(recordData, (int)countRows, dataSearch.PageNumber, dataSearch.PageSize);

                meg_res.Success = true;
                meg_res.Message = ReturnMessage.SUCCESS;
                meg_res.Code = ReturnCode.SUCCESS;
                meg_res.Data = response;
                return meg_res;
            }
            catch (Exception ex)
            {
                meg_res.Message = methodName + " - " + ex.Message + " - " + ex.StackTrace;
                return meg_res;
            }
        }
    }
}
