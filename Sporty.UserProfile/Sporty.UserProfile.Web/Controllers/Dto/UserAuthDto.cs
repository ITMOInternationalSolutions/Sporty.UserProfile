using System.ComponentModel.DataAnnotations;
using Sporty.UserProfile.Core.Users.Entities;

namespace Sporty.UserProfile.Web.Controllers.Dto;

public class UserAuthDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is incorrect")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "UserType is required")]
    public UserType UserType { get; set; }
}