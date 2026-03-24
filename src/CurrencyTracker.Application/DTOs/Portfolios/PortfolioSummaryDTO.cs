namespace CurrencyTracker.Application.DTOs.Portfolios;

public record PortfolioSummaryDTO
{
    public Guid Id { get; init; }

    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal TotalInvested { get; init; } // how much you spent
    public decimal CurrentValue { get; init; }  // what's worth it today
    public decimal TotalProfitLoss => CurrentValue - TotalInvested; // is it loss or profit
    public decimal ProfitLossPercentage => TotalInvested != 0
            ? TotalProfitLoss / TotalInvested * 100 // percentage of the profit or loss
            : 0;
    public bool IsProfitable => TotalProfitLoss > 0;


}
