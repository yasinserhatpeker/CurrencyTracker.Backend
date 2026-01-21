using System;
using System.Runtime.CompilerServices;
using CurrencyTracker.Domain.Entities;

namespace CurrencyTracker.Application.DTOs.Portfolios;

public class CreatePortfolioDTO
{
   public string Name {get;set;} = default!;
   public User User {get;set;} = default!;
}
