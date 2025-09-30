using ECommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.DTO;
using OrderAPI.Application.DTO.Conversions;
using OrderAPI.Application.Interface;
using OrderAPI.Application.Services;

namespace OrderAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IOrder orderInterface, IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
            {
                return NotFound("No order found.");
            }

            var (_, list) = OrderConversion.FromEntity(null!, orders);
            return list!.Any() ? Ok(list) : NotFound("No order found.");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid argument passed.");
            }

            var order = await orderInterface.GetByIdAsync(id);
            if (order is null)
            {
                return NotFound("Order not found.");
            }

            var (_order, _) = OrderConversion.FromEntity(order, null!);
            return _order is not null ? Ok(_order) : NotFound("Order not found.");
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetClientOrders(int clientId)
        {
            if (clientId < 0)
            {
                return BadRequest("Invalid argument passed.");
            }
            var clientOrders = await orderService.GetAsyncOrdersByClientId(clientId);
            return clientOrders.Any() ? Ok(clientOrders) : NotFound($"No order for client {clientId} found.");
        }

        [HttpGet("details/{id:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int id)
        {
            if (id < 0)
            {
                return BadRequest("Invalid argument passed.");
            }
            var orderDetails = await orderService.GetAsyncOrderDetails(id);
            return orderDetails is not null ? Ok(orderDetails) : NotFound("Order details not found.");
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.CreateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.UpdateAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        public async Task<ActionResult<Response>> DeleteOrder(OrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = OrderConversion.ToEntity(orderDTO);
            var response = await orderInterface.DeleteAsync(order);
            return response.Flag ? Ok(response) : BadRequest(response);
        }
    }
}
