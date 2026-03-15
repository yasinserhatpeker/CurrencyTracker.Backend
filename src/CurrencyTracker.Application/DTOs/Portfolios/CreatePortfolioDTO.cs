
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CurrencyTracker.Application.DTOs.Portfolios;

public class CreatePortfolioDTO
{
   [Required(ErrorMessage = "Name is required")]
   [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
   public string Name { get; set; } = default!;
   
   [JsonIgnore]
   public Guid UserId { get; set; }
   

}
