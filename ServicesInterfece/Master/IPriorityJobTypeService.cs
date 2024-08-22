using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IPriorityJobTypeService
    {
        Task<MessageResponseModel> SearchService(SearchPriorityJobtypeModel? dataSearch);
        Task<MessageResponseModel> GetByIdService(int id);
        Task<MessageResponseModel> AddService(PriorityJobtypeDataModel data, string? userCode);
        Task<MessageResponseModel> EditService(PriorityJobtypeDataModel data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userCode);
    }
}
