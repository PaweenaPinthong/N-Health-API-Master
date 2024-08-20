using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IPriceData
    {
        Task<bool> Add(PriceModel? data, string? userCode);
        Task<bool> Edit(PriceModel? data, string? userCode);
        Task<DataTable> GetById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchPriceModel? data);
        Task<MessageResponseModel> CheckDupData(PriceModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
    }
}
