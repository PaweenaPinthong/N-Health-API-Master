using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IPriorityData
    {
        Task<bool> Add(PriorityModel? priorityModel, string? userCode);
        Task<bool> Edit(PriorityModel? priorityModel, string? userCode);
        Task<DataTable> GetById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchPriorityModel? dataSearch);
        Task<MessageResponseModel> CheckDupData(PriorityModel? priorityModel);

    }
}
