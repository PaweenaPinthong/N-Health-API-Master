using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IProductTypeData
    {
        Task<bool> Add(ProductTypeModel? data, string? userCode,bool? isImportexcel);
        Task<bool> Edit(ProductTypeModel? data, string? userCode);
        Task<DataTable> GetById(int? id);
        Task<bool> ChangeActive(int? id, bool? isActive, string? userCode);
        Task<(DataTable, long)> Search(SearchProductTypeModel? data);
        Task<MessageResponseModel> CheckDupData(ProductTypeModel? data);
        Task<(DataTable, long)> SearchExport(SearchProductTypeModel? data);

    }
}
