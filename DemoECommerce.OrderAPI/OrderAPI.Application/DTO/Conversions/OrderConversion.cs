using OrderAPI.Domain.Entities;

namespace OrderAPI.Application.DTO.Conversions
{
    public static class OrderConversion
    {
        public static Order ToEntity(OrderDTO orderDTO) => new()
        {
            Id = orderDTO.Id,
            ClientId = orderDTO.ClientId,
            ProductId = orderDTO.ProductId,
            PurchaseQuantity = orderDTO.PurchaseQuantity,
            Date = orderDTO.Date
        };

        public static (OrderDTO?, IEnumerable<OrderDTO>?) FromEntity(Order? order, IEnumerable<Order>? orders)
        {
            if (order is not null && orders is null)
            {
                var singleOrder = new OrderDTO(
                    order!.Id,
                    order!.ProductId,
                    order!.ClientId,
                    order!.PurchaseQuantity,
                    order!.Date
                    );
                return (singleOrder, null);
            }

            if (order is null && orders is not null)
            {
                var _orders = orders!.Select(o => new OrderDTO
                    (
                        o.Id,
                        o.ProductId,
                        o.ClientId,
                        o.PurchaseQuantity,
                        o.Date
                    ));
                return (null, _orders);
            }
            return (null, null);
        }
    }
}
