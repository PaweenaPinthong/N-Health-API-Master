using N_Health_API.Models.Shared;

namespace N_Health_API.Models.Master
{
    public class ReasonModel : MasterModel
    {
        public int Reason_Id { get; set; }
        public string Reason_Code { get; set; } = string.Empty;
        public string Reason_Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
    public class SearchReasonModel : RequestParameterModel
    {
        public string? Reason_Name { get; set; } = string.Empty;
        public DateTime? Datetime { get; set; }
        public bool? Active { get; set; }
    }
    public class ReasonRequest
    {
        public int Reason_Id { get; set; }
        public bool Active { get; set; }
    }
    public class ReasonSearch
    {
        public int Reason_Id { get; set; }
        public string Reason_Code { get; set; } = string.Empty;
        public string Reason_Name { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime Created_DateTime { get; set; } = DateTime.Now;
        public string? Created_Date_Str { get; set; }
        public string? Created_By { get; set; }
        public DateTime Update_DateTime { get; set; } = DateTime.Now;
        public string? Update_Date_Str { get; set; }
        public string? Update_By { get; set; }
    }

}
