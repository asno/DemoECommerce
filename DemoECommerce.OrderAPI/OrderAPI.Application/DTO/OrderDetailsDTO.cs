using System.ComponentModel.DataAnnotations;

namespace OrderAPI.Application.DTO
{
    public record OrderDetailsDTO
    (
        [Required] int OrderId,
        [Required] int ProductId,
        [Required] int ClientId,
        [Required] string PhoneNumber,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required] string ProductName,
        [Required] int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderDate
    );
}
