using OrderAPI.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderAPI.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetAsyncOrdersByClientId(int clientId);
        Task<OrderDetailsDTO> GetAsyncOrderDetails(int orderId);
    }
}
