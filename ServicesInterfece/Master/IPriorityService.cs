using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IPriorityService
    {
        Task<MessageResponseModel> SearchService(SearchPriorityModel? dataSearch);
        Task<MessageResponseModel> GetByIdService(int? id);
        Task<MessageResponseModel> AddService(PriorityModel? priorityModel,string? userCode);
        Task<MessageResponseModel> EditService(PriorityModel? priorityModel, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int? id,bool? isActive, string? userCode);
    }
}
