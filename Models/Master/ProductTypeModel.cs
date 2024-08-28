using N_Health_API.Models.Shared;

namespace N_Health_API.Models.Master
{
    public class ProductTypeModel : MasterModel
    {
        public int Product_Type_Id { get; set; }
        public string Product_Type_Code { get; set; } = string.Empty;
        public string Product_Type_Code_Interface { get; set; } = string.Empty;
        public string Product_Type_Name { get; set; } = string.Empty;
        public bool Sub_Product_Flag { get; set; }
        public bool Active { get; set; } = true;
    }

    public class SearchProductTypeModel : RequestParameterModel
    {
        public string? Product_Type_Code { get; set; } = string.Empty;
        public string? Product_Type_Name { get; set; } = string.Empty;
        public bool? Active { get; set; }
    }

    public class ProductTypeRequest
    {
        public int Product_Type_Id { get; set; }
        public bool Active { get; set; }
    }

    public class ProductTypeSearch
    {
        public int Product_Type_Id { get; set; }
        public string Product_Type_Code { get; set; } = string.Empty;
        public string Product_Type_Name { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime Created_DateTime { get; set; } = DateTime.Now;
        public string? Created_Date_Str { get; set; }
        public string? Created_By { get; set; }
        public string? Created_Name { get; set; }
        public DateTime Update_DateTime { get; set; } = DateTime.Now;
        public string? Update_Date_Str { get; set; }
        public string? Update_By { get; set; }
        public string? Update_Name { get; set; }
    }
}
