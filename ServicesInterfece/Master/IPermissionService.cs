﻿using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Threading.Tasks;

namespace N_Health_API.ServicesInterfece.Master
{
    public interface IPermissionService
    {
        Task<MessageResponseModel> GetAllMenuNameService();
        Task<MessageResponseModel> SearchService(SearchPermissionModel data);
        Task<MessageResponseModel> GetByIdService(int id);
        Task<MessageResponseModel> AddService(PermissionDataModel data,string? userCode);
        Task<MessageResponseModel> EditService(PermissionDataModel data, string? userCode);
        Task<MessageResponseModel> ChangeActiveService(int id, bool isActive, string? userCode);
        //Task<MessageResponseModel> ExportExcelService(SearchPermissionModel data);
        //Task<MessageResponseModel> ImportExcelService([FromForm] IFormFile data);
        //Task<MessageResponseModel> DownloadTemplate();

    }
}
