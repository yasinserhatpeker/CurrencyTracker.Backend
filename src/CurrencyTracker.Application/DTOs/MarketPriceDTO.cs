namespace CurrencyTracker.Application.DTOs;

public record MarketPriceDTO
{
    public string BaseCurrency { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public DateTime LastUpdated { get; init; }
    public decimal? ChangePercentage24H { get; init; }
    public string QuoteCurrency { get; init; } = string.Empty;
    public string? Source { get; init; } = string.Empty;
    public bool? IsUp => ChangePercentage24H > 0;
}
