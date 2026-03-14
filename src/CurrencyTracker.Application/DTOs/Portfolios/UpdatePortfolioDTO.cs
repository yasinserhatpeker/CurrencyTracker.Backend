using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Application.DTOs.Portfolios;

public class UpdatePortfolioDTO
{
   [Required(ErrorMessage = "Name is required")]
   [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
   public string Name { get; set; } = default!;
   public Guid Id { get; set; }
}
