using System.Text.Json.Serialization;

namespace JwtDecode.Models;
internal class Transaction
{
    public string? Id { get; set; }
    public double Amount { get; set; }
    public string? Currency { get; set; }
    public Meta? Meta { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TransactionStatus Status { get; set; }
}
