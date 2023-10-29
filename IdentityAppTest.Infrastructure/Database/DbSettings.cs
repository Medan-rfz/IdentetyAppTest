
using System.Text.Json.Serialization;

namespace IdentityAppTest.Infrastructure.Database;

public class DbSettings
{
    [JsonPropertyName("dbConnectionString")]
    public string ConnectionString { get; set; }
}
