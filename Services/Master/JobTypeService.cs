using N_Health_API.Helper;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using N_Health_API.RepositoriesInterface.Master;
using N_Health_API.ServicesInterfece.Master;

namespace N_Health_API.Services.Master
{

    public class JobTypeService : IJobTypeService
    {

        private IConfiguration _config;
        private IJobTypeData _repo;
        public JobTypeService(IConfiguration config, IJobTypeData repo)
        {
            _config = config;
            _repo = repo;
        }


        public async Task<MessageResponseModel> AddService(JobtypeDataReasone data, string? userCode)
        {
            MessageResponseModel meg_res = new MessageResponseModel();
            string methodName = Util.GetMethodName();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;
            try
            {
                var checkDup = await _repo.CheckDupData(data);
                if (!Convert.ToBoolean(checkDup?.Data))
                {
                    var result = await _repo.Add(data, userCode);
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

        public async Task<MessageResponseModel> SearchService(SearchJobtypeModel data)
        {
            string methodName = Util.GetMethodName();
            MessageResponseModel meg_res = new MessageResponseModel();
            meg_res.Message = ReturnMessage.SYSTEM_ERROR;
            meg_res.Code = ReturnCode.SYSTEM_ERROR;
            meg_res.Success = false;
            try
            {
                var res = await _repo.SearchJobData(data);
                var recordData = Util.ConvertDataTableToList<JobtypeModel>(res.Item1);
                var countRows = res.Item2;
                var response = new PaginatedListModel<JobtypeModel>(recordData, (int)countRows, data.PageNumber, data.PageSize);

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