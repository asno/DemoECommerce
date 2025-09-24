using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Application.DTO
{
    public record AppUser
    (
        int Id,
        [Required] string Name,
        [Required] string PhoneNumber,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required] string Password,
        [Required] string Role
    );
}
