using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IVehicleTypeData
    {
        Task<bool> Add(VehicleTypeModel? data, string? userCode);
        Task<bool> Edit(VehicleTypeModel? data, string? userCode);
        Task<DataTable> GetById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchVehicleTypeModel? data);
        Task<MessageResponseModel> CheckDupData(VehicleTypeModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
    }
}

