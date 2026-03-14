using System.ComponentModel.DataAnnotations;
using CurrencyTracker.Domain.Entities.Enums;

namespace CurrencyTracker.Application.DTOs.Transactions;

public class CreateTransactionsDTO
{

    [Required(ErrorMessage = "PortfolioId is required")]
    public Guid PortfolioId { get; set; }
    [Required(ErrorMessage = "Transaction date is required")]
    public DateTime TransactionDate { get; set; }

    [Required(ErrorMessage = "Symbol is required")]
    [StringLength(10, MinimumLength = 2, ErrorMessage = "Symbol must be between 2 and 10 characters (e. g, BTC, ETH, USD)")]
    public string Symbol { get; set; } = default!;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.00000001, double.MaxValue, ErrorMessage = "Price must be strictly greater than zero.")]
    public decimal Price { get; set; } = default!;

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0.00000001, double.MaxValue, ErrorMessage = "Price must be strictly greater than zero.")]
    public decimal Quantity { get; set; } = default!;

    [Required(ErrorMessage = "Transaction type is required")]
    public TransactionType TransactionType { get; set; }

    [Required(ErrorMessage = "Quote currency is required")]
    [StringLength(5)]
    public string QuoteCurrency { get; set; } = "TRY";
}
