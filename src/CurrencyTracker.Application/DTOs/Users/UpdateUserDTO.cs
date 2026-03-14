using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CurrencyTracker.Application.DTOs.Users;

public class UpdateUserDTO
{ 
   [JsonIgnore]
   public Guid Id {get;set;}
   [Required]
   [EmailAddress]
   public string Email {get;set;} = default!;
   [Required]
   public string Username {get;set;} = default!;

}
