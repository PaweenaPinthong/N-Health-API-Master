using N_Health_API.Models.Shared;

namespace N_Health_API.Models.Master
{
    public class JobtypeModel : MasterModel
    {
        public int? Jobtype_Id { get; set; }
        public string? Jobtype_Code { get; set; } = string.Empty;
        public string? Jobtype_Name { get; set; } = string.Empty;
        public string? Jobtype_Desc { get; set; } = string.Empty;
        public int? Location_Id { get; set; }
        public bool? Product_Detail_Flag { get; set; }
        public string? Team { get; set; } = string.Empty;
        public bool? Active { get; set; }
        public string? Reason_Name { get; set; } = string.Empty;
    }

    public class JobtypeDataReasone
    {
        public JobtypeModel? jobtypeModel { get; set; } = new JobtypeModel();
        public List<JobtypeReasonModel>? jobtypeReasons { get; set; } = new List<JobtypeReasonModel>();
    }
    public class JobtypeReasonModel : MasterModel
    {
        public int? Jobtype_Id { get; set; }
        public int? Reason_Id { get; set; }
    }

    public class SearchJobtypeModel : RequestParameterModel
    {
        public bool? Status { get; set; }
        public string? Team { get; set; }
        public string? Jobtype_Name { get; set; }
        public string? Short_Location_Name { get; set; }
    }

    public class JobtypeRequest
    {
        public int Jobtype_Id { get; set; }
        public bool Active { get; set; }
    }
}
