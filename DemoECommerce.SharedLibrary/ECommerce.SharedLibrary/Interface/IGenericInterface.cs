using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ECommerce.SharedLibrary.Responses;


namespace ECommerce.SharedLibrary.Interface
{
    public interface IGenericInterface<T> where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    }
}
