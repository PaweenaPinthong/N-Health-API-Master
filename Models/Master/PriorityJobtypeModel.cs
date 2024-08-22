using N_Health_API.Models.Shared;

namespace N_Health_API.Models.Master
{
    public class PriorityJobtypeModel : MasterModel
    {
        public int Priority_Jobtype_Id { get; set; }
        public string Priority_Jobtype_Code { get; set; } = string.Empty;
        public int Priority_Id { get; set; }
        public string Team { get; set; } = string.Empty;
        public int Service_Time { get; set; }
        public int Waiting_Time { get; set; }
        public bool Active { get; set; }
    }
    public class PriorityJobtypeJobtypeModel : MasterModel
    {
        public int Priority_Jobtype_Id { get; set; }
        public int Jobtype_Id { get; set; }
    }

    public class PriorityJobtypeRequest
    {
        public int Priority_Jobtype_Id { get; set; }
        public bool Active { get; set; }
    }

    public class PriorityJobtypeDataModel
    {
        public PriorityJobtypeModel PriorityJobtype { get; set; }
        public List<PriorityJobtypeJobtypeModel> Jobtype { get; set; }
    }

    public class PriorityJobtypeDataModelById
    {
        public PriorityJobtypeModelById PriorityJobtype { get; set; }
        public List<PriorityJobtypeJobtypeModelById> Jobtype { get; set; }
    }

    public class PriorityJobtypeModelById
    {
        public int Priority_Jobtype_Id { get; set; }
        public string Priority_Jobtype_Code { get; set; } = string.Empty;
        public int Priority_Id { get; set; }
        public string Priority_Name { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public int Service_Time { get; set; }
        public int Waiting_Time { get; set; }
        public bool Active { get; set; }
    }

    public class PriorityJobtypeJobtypeModelById
    {
        public int Priority_Jobtype_Id { get; set; }
        public int Jobtype_Id { get; set; }
        public string Jobtype_name { get; set; } = string.Empty;
    }

    public class SearchPriorityJobtypeModel : RequestParameterModel
    {
        public bool? Active { get; set; }
        public int? Jobtype_Id { get; set; }
        public int? Priority_Id { get; set; }
        public int? Location_Id { get; set; }
        public string? Team { get; set; }

    }

    public class SearchPriorityJobtypeResponseModel : MasterModel
    {
        public int Priority_Jobtype_Id { get; set; }
        public string Priority_Jobtype_Code { get; set; } = string.Empty;
        public int Priority_Id { get; set; }
        public string Priority_Name { get; set; } = string.Empty;
        public string Jobtype_Name { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public int Service_Time { get; set; }
        public int Waiting_Time { get; set; }
        public bool Active { get; set; }

    }
}
