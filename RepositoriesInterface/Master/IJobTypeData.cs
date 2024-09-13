using System.Data;
using BusinessIdeaMasterAPIs.Models;
using Microsoft.AspNetCore.Mvc;
using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IJobTypeData
    {
        Task<bool> Add(JobtypeDataReasone? data, string? userCode ,bool? isImport);
        Task<bool> Edit(JobtypeDataReasone? data, string? userCode);
        Task<MessageResponseModel> CheckDupData(JobtypeDataReasone? data);
        Task<(DataTable, long)> SearchJobData(SearchJobtypeModel? data);
        Task<DataTable> GetById(int? id);
        Task<bool> ChangeActive(int? id, bool? isActive, string? userCode);
        Task<MessageResponseModel> ImportOrder(string? id, string? memberCode);

    }
}