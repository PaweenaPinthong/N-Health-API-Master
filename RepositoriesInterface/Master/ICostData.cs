using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface ICostData
    {
        Task<bool> Add(CostDataModel? data, string? userCode);
        Task<bool> Edit(CostDataModel? data, string? userCode);
        Task<CostDataModel> GetById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchCostModel? data);
        Task<MessageResponseModel> CheckDupData(CostDataModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
    }
}
