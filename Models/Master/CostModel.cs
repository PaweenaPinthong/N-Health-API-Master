using N_Health_API.Models.Shared;

namespace N_Health_API.Models.Master
{
    public class CostModel : MasterModel
    {
        public int Cost_Id { get; set; }
        public string Cost_Code { get; set; } = string.Empty;
        public string Cost_Name { get; set; } = string.Empty;
        public int Location_Id { get; set; }
        public string Location__Name { get; set; } = string.Empty; 
        public string Team { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
    public class CostVehicleTypeModel : MasterModel
    {
        public int Cost_Id { get; set; }
        public int Vehicle_Type_Id { get; set; }
        public decimal Cost_Value { get; set; }
        public string Cost_Per_Unit_Type { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
    }

    public class CostDataModel
    {
        public CostModel Cost { get; set; } = new CostModel();
        public List<CostVehicleTypeModel>? CostVehicleType { get; set; } = new List<CostVehicleTypeModel>();
    }

    public class SearchCostModel : RequestParameterModel
    {        
        public string? Location_Name { get; set; } = string.Empty;
        public string? Cost_Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public class CostRequest
    {
        public int Cost_Id { get; set; }
        public bool Active { get; set; }
    }
}
