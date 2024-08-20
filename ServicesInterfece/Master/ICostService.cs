using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface ICostService
    {
        Task<MessageResponseModel> SearchService(SearchCostModel data);
        Task<MessageResponseModel> GetByIdService(int id);
        Task<MessageResponseModel> AddService(CostDataModel data, string? userName);
        Task<MessageResponseModel> EditService(CostDataModel data, string? userName);
        Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userName);
    }
}
