using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IReasonData
    {
        Task<bool> Add(ReasonModel? data, string? userCode,bool? isImportexcel);
        Task<bool> Edit(ReasonModel? data, string? userCode);
        Task<DataTable> GetById(int? id);
        Task<bool> ChangeActive(int? id, bool? isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchReasonModel? data);
        Task<MessageResponseModel> CheckDupData(ReasonModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
        Task<(DataTable, long)> SearchExport(SearchReasonModel? data);
    }
}


