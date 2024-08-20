using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface ITeamUnitService
    {
        Task<MessageResponseModel> SearchService(SearchTeamUnitModel data);
        Task<MessageResponseModel> GetByIdService(int id);
        Task<MessageResponseModel> AddService(TeamUnitModel data, string? userCode);
        Task<MessageResponseModel> EditService(TeamUnitModel data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userCode);
    }
}
