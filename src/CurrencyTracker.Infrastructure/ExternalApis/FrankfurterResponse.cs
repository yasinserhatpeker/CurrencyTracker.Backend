using System;
using System.Text.Json.Serialization;

namespace CurrencyTracker.Infrastructure.ExternalApis;

public class FrankfurterResponse
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; } = string.Empty;

    [JsonPropertyName("date")]
    public DateTime Date { get; set; }

    [JsonPropertyName("rates")]
    public Dictionary<string,decimal> Rates {get;set;} = new(); // key = currency, value = rate
}
