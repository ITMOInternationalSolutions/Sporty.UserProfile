using System.ComponentModel.DataAnnotations;
using Sporty.UserProfile.Core.Users.Entities;

namespace Sporty.UserProfile.Web.Controllers.Dto;

public class UserDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email is incorrect")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "UserType is required")]
    public UserType UserType { get; set; }
}