using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IVehicleService
    {
        Task<MessageResponseModel> SearchService(SearchVehicleModel data);
        Task<MessageResponseModel> GetByIdService(int id);
        Task<MessageResponseModel> AddService(VehicleModel data, string? userCode);
        Task<MessageResponseModel> EditService(VehicleModel data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userCode);
    }
}
