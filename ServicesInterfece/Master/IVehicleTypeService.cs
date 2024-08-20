using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IVehicleTypeService
    {
        Task<MessageResponseModel> SearchService(SearchVehicleTypeModel data);
        Task<MessageResponseModel> GetByIdService(int id);
        Task<MessageResponseModel> AddService(VehicleTypeModel data, string? userCode);
        Task<MessageResponseModel> EditService(VehicleTypeModel data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userCode);
    }
}
