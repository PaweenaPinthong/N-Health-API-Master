using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IPermissionData
    {
        Task<bool> Add(PermissionDataModel? data, string? userCode);
        Task<bool> Edit(PermissionDataModel? data, string? userCode);
        Task<DataTable> GetPermissionById(int id);
        Task<DataTable> GetMenuByPermissionId(int id);
        Task<DataTable> GetAllMenuName();
        //Task<DataTable> Search(SearchPermissionModel? data);
        Task<(DataTable, long)> Search(SearchPermissionModel? data);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<MessageResponseModel> CheckDupDataPermission(PermissionModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
        //Task<int> CountData();
    }
}
