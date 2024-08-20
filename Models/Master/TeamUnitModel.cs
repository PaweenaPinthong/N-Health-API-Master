using N_Health_API.Models.Shared;

namespace N_Health_API.Models.Master
{
    public class TeamUnitModel : MasterModel
    {
        public int Team_Unit_Id { get; set; }
        public string Team_Unit_Code { get; set; } = string.Empty;
        public string Team_Unit_Name { get; set; } = string.Empty;
        public int Location_Id { get; set; }
        public string Location_Name { get; set; } = string.Empty; 
        public string Team { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public class SearchTeamUnitModel : RequestParameterModel
    {
        public string? Team { get; set; } = string.Empty;
        public string? Team_Unit_Name { get; set; } = string.Empty;//Unit
        public bool Active { get; set; }
    }

    public class TeamUnitRequest
    {
        public int Team_Unit_Id { get; set; }
        public bool Active { get; set; }
    }
}
