﻿using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Interface;
using ProductAPI.Domain.Entities;
using ProductAPI.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductAPI.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var product = await GetAsync(_ => _.Name!.Equals(entity.Name));
                if (product is not null && !string.IsNullOrEmpty(product.Name))
                {
                    return new Response(false, $"{product} already exist.");
                }
                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity is not null && currentEntity.Id > 0)
                {
                    return new Response(true, $"{currentEntity.Name} added successfully.");
                }
                else
                {
                    return new Response(false, $"Unexpected error occured when trying to add {currentEntity}.");
                }
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured when attempting to create a new product.");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await GetByIdAsync(entity.Id);
                if(product is null)
                {
                    return new Response(false, $"Error: {product} not found.");
                }
                context.Products.Remove(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{product.Name} was deleted successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured when attempting to delete a product.");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occured when attempting to find all products.");
            }
        }

        public async Task<Product> GetAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.FirstOrDefaultAsync(predicate);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occured when attempting to find the product.");
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new InvalidOperationException("Error occured when attempting to find the product.");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await GetByIdAsync(entity.Id);
                if (product is null)
                {
                    return new Response(false, $"Error: {product} not found.");
                }
                context.Entry(product).State = EntityState.Detached;
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{product.Name} was updated successfully.");

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occured when attempting to update a product.");
            }
        }
    }
}
