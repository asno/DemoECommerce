using ECommerce.SharedLibrary.Interface;
using OrderAPI.Domain.Entities;
using System.Linq.Expressions;

namespace OrderAPI.Application.Interface
{
    public interface IOrder:IGenericInterface<Order>
    {
        public Task<IEnumerable<Order>> GetAsyncOrders(Expression<Func<Order, bool>> predicate);
    }
}
