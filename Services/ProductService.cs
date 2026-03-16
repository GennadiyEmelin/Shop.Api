using Shop.Api.DTO.Product;
using Shop.Api.Models;
using Shop.Api.Repositories;

namespace Shop.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ProductResponseDTO> Create(ProductCreateDTO productCreateDTO)
        {
            try
            {
                var product = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = productCreateDTO.Name,
                    Price = productCreateDTO.Price,
                    Description = productCreateDTO.Description,
                    Stock = productCreateDTO.Stock,
                    CreatedAt = DateTime.UtcNow
                };
                await _productRepository.Create(product);
                return new ProductResponseDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                };
            }
            catch
            {
                throw new Exception("Ошибка при добавлении товара");
            }
        }

        public async Task Delete(Guid id)
        {
            try
            {
                await _productRepository.Delete(id);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductResponseDTO>> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAll();
                return products.Where(p => p.IsDeleted == false)
                               .Select(p => new ProductResponseDTO
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Price = p.Price,
                                   Description = p.Description
                               })
                               .ToList();
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductResponseDTO> GetById(Guid id)
        {
            var product = await _productRepository.GetById(id);
            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };
        }

        public async Task<ProductResponseDTO> Update(Guid id, ProductUpdateDTO productUpdateDTO)
        {
            var existingProduct = await _productRepository.GetById(id);

            if (productUpdateDTO.Name == null)
            { productUpdateDTO.Name = existingProduct.Name; }
            else { existingProduct.Name = productUpdateDTO.Name; }

            if (productUpdateDTO.Description == null)
            { productUpdateDTO.Description = existingProduct.Description; }
            else { existingProduct.Description = productUpdateDTO.Description; }

            if (productUpdateDTO.Stock == null)
            { productUpdateDTO.Stock = existingProduct.Stock; }
            else { existingProduct.Stock = productUpdateDTO.Stock; }

            if (productUpdateDTO.Price == null)
            { productUpdateDTO.Price = existingProduct.Price; }
            else { existingProduct.Price = productUpdateDTO.Price; }

            await _productRepository.Update(existingProduct);
            return new ProductResponseDTO
            {
                Id = existingProduct.Id,
                Name = existingProduct.Name,
                Price = existingProduct.Price,
                Description = existingProduct.Description
            };
        }
    }
}
