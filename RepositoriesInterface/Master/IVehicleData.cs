using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IVehicleData
    {
        Task<bool> Add(VehicleModel? data, string? userCode);
        Task<bool> Edit(VehicleModel? data, string? userCode);
        Task<DataTable> GetById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchVehicleModel? data);
        Task<MessageResponseModel> CheckDupData(VehicleModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
    }
}
