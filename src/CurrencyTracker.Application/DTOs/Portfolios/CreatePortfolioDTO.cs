
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CurrencyTracker.Application.DTOs.Portfolios;

public class CreatePortfolioDTO
{
   
   public string Name { get; set; } = default!;
   
   [JsonIgnore]
   public Guid UserId { get; set; }
   

}
