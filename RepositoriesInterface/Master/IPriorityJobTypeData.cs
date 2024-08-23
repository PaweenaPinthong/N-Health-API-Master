using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IPriorityJobTypeData
    {
        Task<bool> Add(PriorityJobtypeDataModel data, string? userCode);
        Task<bool> Edit(PriorityJobtypeDataModel data, string? userCode);
        Task<DataTable> GetById(int id);
        Task<DataTable> GetListJobTypeById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchPriorityJobtypeModel? dataSearch);
        Task<MessageResponseModel> CheckDupData(PriorityJobtypeDataModel data);

    }
}
