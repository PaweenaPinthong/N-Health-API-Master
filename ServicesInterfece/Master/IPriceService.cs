using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IPriceService
    {
        Task<MessageResponseModel> SearchService(SearchPriceModel data);
        Task<MessageResponseModel> GetByIdService(int id);
        Task<MessageResponseModel> AddService(PriceModel data, string? userCode);
        Task<MessageResponseModel> EditService(PriceModel data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userCode);
    }
}
