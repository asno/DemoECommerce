using ProductAPI.Domain.Entities;

namespace ProductAPI.Application.DTO.Conversions
{
    public static class ProductConversion
    {
        public static Product ToEntity(ProductDTO productDTO) => new()
        {
            Id = productDTO.Id,
            Name = productDTO.Name,
            Quantity = productDTO.Quantity,
            Price = productDTO.Price
        };

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            if (product != null && products == null)
            {
                var singleProduct = new ProductDTO
                    (
                        Id: product.Id,
                        Name: product.Name!,
                        Quantity: product.Quantity,
                        Price: product.Price
                    );
                return (singleProduct, null);
            }
            if (product == null && products != null)
            {
                var listOfProducts = products.Select(p => new ProductDTO(p.Id, p.Name!, p.Quantity, p.Price)).ToList();
                return (null, listOfProducts);
            }

            return (null, null);
        }
    }
}
