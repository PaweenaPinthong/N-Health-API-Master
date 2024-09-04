using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IReasonService
    {
        Task<MessageResponseModel> SearchService(SearchReasonModel? data);
        Task<MessageResponseModel> GetByIdService(int? id);
        Task<MessageResponseModel> AddService(ReasonModel? data, string? userCode);
        Task<MessageResponseModel> EditService(ReasonModel? data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int? id, bool? isActive, string? userCode);
    }
}
