using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Application.DTO
{
    public record AppUserDTO
    (
        [Required] int Id,
        [Required] string Name,
        [Required] string PhoneNumber,
        [Required, EmailAddress] string Address,
        [Required] string Email,
        [Required] string Password,
        [Required] string Role
    );
}
