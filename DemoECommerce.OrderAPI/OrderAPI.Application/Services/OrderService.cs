using OrderAPI.Application.DTO;
using OrderAPI.Application.DTO.Conversions;
using OrderAPI.Application.Interface;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderAPI.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline) : IOrderService
    {
        public async Task<ProductDTO> GetAsyncProductDTO(int productId)
        {
            var httpMessage = await httpClient.GetAsync($"/api/products/{productId}");
            if (!httpMessage.IsSuccessStatusCode)
            {
                return null!;
            }
            var productDTO = await httpMessage.Content.ReadFromJsonAsync<ProductDTO>();
            return productDTO!;
        }

        public async Task<AppUserDTO> GetAsyncAppUserDTO(int appUserId)
        {
            var httpMessage = await httpClient.GetAsync($"/api/users/{appUserId}");
            if (!httpMessage.IsSuccessStatusCode)
            {
                return null!;
            }
            var appUserDTO = await httpMessage.Content.ReadFromJsonAsync<AppUserDTO>();
            return appUserDTO!;
        }

        public async Task<OrderDetailsDTO> GetAsyncOrderDetails(int orderId)
        {
            var order = await orderInterface.GetByIdAsync(orderId);
            if (order is null || order.Id < 0)
            {
                return null!;
            }
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");
            var productDTO = retryPipeline.ExecuteAsync(async token => await GetAsyncProductDTO(order.ProductId)).Result;
            var appUserDTO = retryPipeline.ExecuteAsync(async async => await GetAsyncAppUserDTO(order.ClientId)).Result;

            return new OrderDetailsDTO(order.Id, productDTO.Id, appUserDTO.Id, appUserDTO.PhoneNumber,
                appUserDTO.Address, appUserDTO.Email, productDTO.Name, productDTO.Quantity,
                productDTO.Price, productDTO.Quantity * productDTO.Price, order.Date);
        }

        public async Task<IEnumerable<OrderDTO>> GetAsyncOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetAsyncOrders(o => o.ClientId == clientId);
            if (!orders.Any())
            {
                return null!;
            }

            var (_, ordersDTO) = OrderConversion.FromEntity(null, orders);
            return ordersDTO!;
        }
    }
}
