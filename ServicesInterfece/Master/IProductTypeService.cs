using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IProductTypeService
    {
        Task<MessageResponseModel> SearchService(SearchProductTypeModel? data);
        Task<MessageResponseModel> GetByIdService(int? id);
        Task<MessageResponseModel> AddService(ProductTypeModel? data, string? userCode);
        Task<MessageResponseModel> EditService(ProductTypeModel? data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int? id, bool? isActive,string? userCode);
    }
}
