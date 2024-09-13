using System.Text.Json.Serialization;
using N_Health_API.Models;

namespace BusinessIdeaMasterAPIs.Models
{
    public class ImportOrderRES : MasterModel
    {
        [JsonPropertyName("CFNo")]
        public string? CFNo { get; set; }
    }
}
