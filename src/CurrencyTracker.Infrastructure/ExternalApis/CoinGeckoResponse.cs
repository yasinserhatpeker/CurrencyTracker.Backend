using System.Text.Json.Serialization;

namespace CurrencyTracker.Infrastructure.ExternalAPIs;

public class CoinGeckoResponse
{  
    [JsonPropertyName("amount")]
    public decimal amount {get;set;}
}
