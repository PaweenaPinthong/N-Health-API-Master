using System.Data;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IJobTypeService
    {
        Task<MessageResponseModel> AddService(JobtypeDataReasone? data, string? userCode);
        Task<MessageResponseModel> EditService(JobtypeDataReasone? data, string? userCode);
        Task<MessageResponseModel> SearchService(SearchJobtypeModel? data);
        Task<MessageResponseModel> GetByIdService(int? id);
        Task<MessageResponseModel> ChangeActiveService(int? id, bool? isActive, string? userCode);
    }
}