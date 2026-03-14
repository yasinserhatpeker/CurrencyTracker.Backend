using System.ComponentModel.DataAnnotations;

namespace CurrencyTracker.Application.DTOs;

public class CreateUserDTO
{
  [Required(ErrorMessage = "Username is required")]
  public string Username { get; set; } = default!;

  [Required(ErrorMessage = "Email is required")]
  [EmailAddress(ErrorMessage = "Invalid email address")]
  public string Email { get; set; } = default!;
  
  [Required]
  [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
  public string Password { get; set; } = default!;
}
