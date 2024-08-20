using N_Health_API.Models.Master;
using N_Health_API.Models.Shared;
using System.Data;

namespace N_Health_API.RepositoriesInterface.Master
{
    public interface IUserinfoData
    {
        Task<bool> Add(UserinfoModel data, string? userName);
        Task<bool> Edit(UserinfoModel? data, string? userName);
        Task<DataTable> GetById(int id);
        Task<bool> ChangeActive(int id, bool isActive, string? userName);
        Task<UserinfoModel> CheckUserByUserName(string user);
        //Task<DataTable> Search(SearchUserInfoModel? data);
        Task<(DataTable,long)> Search(SearchUserInfoModel? data);
        Task<MessageResponseModel> CheckDupData(UserinfoModel? data);//เช็คข้อมูล ถ้า return true ถือว่ามีข้อมูลซ้ำ
        //Task<int> CountData();
    }
}
