using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface ITeamUnitData
    {
        Task<bool> Add(TeamUnitModel data, string? userCode);
        Task<bool> Edit(TeamUnitModel? data, string? userCode);
        Task<DataTable> GetById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<(DataTable,long)> Search(SearchTeamUnitModel? data);
        Task<MessageResponseModel> CheckDupData(TeamUnitModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
    }
}
