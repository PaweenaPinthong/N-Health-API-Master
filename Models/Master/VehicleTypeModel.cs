﻿using N_Health_API.Models.Shared;

namespace N_Health_API.Models.Master
{
    public class VehicleTypeModel : MasterModel
    {
        public int Vehicle_Type_Id { get; set; }
        public string Vehicle_Type_Code { get; set; } = string.Empty;
        public string Vehicle_Type_Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }
    public class SearchVehicleTypeModel : RequestParameterModel
    {
        public string Vehicle_Type_Code { get; set; } = string.Empty;
        public string Vehicle_Type_Name { get; set; } = string.Empty;
        public bool Active { get; set; }
    }

    public class VehicleTypeRequest
    {
        public int Vehicle_Type_Id { get; set; }
        public bool Active { get; set; }
    }
}
