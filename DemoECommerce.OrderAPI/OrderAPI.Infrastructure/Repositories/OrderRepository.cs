using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Application.Interface;
using OrderAPI.Domain.Entities;
using OrderAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace OrderAPI.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
                var order = await GetAsyncOrders(o => o.Id == entity.Id);
                if (order is not null)
                {
                    return new Response(false, $"{order} already exist.");
                }
                var currentEntity = context.Orders.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Id > 0)
                {
                    return new Response(false, $"Unexpected error while creating order.");
                }
                return new Response(true, $"Order id {currentEntity!.Id} added successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while placing an order.");
            }
        }

        public async Task<Response> DeleteAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.Id);
                if (order is null)
                {
                    return new Response(false, $"Error: {order} not found.");
                }
                context.Remove(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"Order id {order.Id} was deleted successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured while placing an order.");
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occured when attempting to find all orders.");
            }
        }

        public async Task<Order> GetAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.FirstOrDefaultAsync(predicate);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occured when attempting to find the order.");
            }
        }

        public async Task<IEnumerable<Order>> GetAsyncOrders(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var orders = await context.Orders.Where(predicate).ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occured when attempting to find orders.");
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await GetByIdAsync(id);
                return order is not null ? order : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occured when attempting to find the order.");
            }
        }

        public async Task<Response> UpdateAsync(Order entity)
        {
            try
            {
                var order = await GetByIdAsync(entity.Id);
                if (order is null)
                {
                    return new Response(false, $"Error: {order} not found.");
                }
                context.Entry(order).State = EntityState.Detached;
                context.Orders.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"Order id {order.Id} was updated successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured when attempting to update an order.");
            }
        }
    }
}
