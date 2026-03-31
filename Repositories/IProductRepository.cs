using Shop.Api.DTO.Common;
using Shop.Api.DTO.Product;
using Shop.Api.Models;

namespace Shop.Api.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product> GetById(Guid Id);
        Task Create(Product product);
        Task Update(Product Product);
        Task Delete(Guid Id);
        Task<List<Product>> GetFiltered(ProductQueryParameters query);
    }
}
