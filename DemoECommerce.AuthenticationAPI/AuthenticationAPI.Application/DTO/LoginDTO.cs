using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Application.DTO
{
    public record LoginDTO([Required] string Email, [Required] string Password);
}
