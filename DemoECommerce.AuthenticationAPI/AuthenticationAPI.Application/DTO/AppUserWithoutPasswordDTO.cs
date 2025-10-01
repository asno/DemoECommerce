using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Application.DTO
{
    public record AppUserWithoutPasswordDTO
    (
        [Required] int Id,
        [Required] string Name,
        [Required] string PhoneNumber,
        [Required, EmailAddress] string Address,
        [Required] string Email,
        [Required] string Role
    );
}
