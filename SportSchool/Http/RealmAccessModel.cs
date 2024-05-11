using System.Text.Json.Serialization;

namespace SportSchool.Http
{
    public class RealmAccessModel
    {
        [JsonPropertyName("roles")]
        public IEnumerable<string> Roles { get; set; }
    }
}
