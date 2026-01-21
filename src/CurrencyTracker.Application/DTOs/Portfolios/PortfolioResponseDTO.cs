using System;

namespace CurrencyTracker.Application.DTOs.Portfolios;

public class PortfolioResponseDTO
{
   public Guid Id {get;set;}
   public string Name {get;set;} = default!;
   public Guid UserId {get;set;} 
   
}
