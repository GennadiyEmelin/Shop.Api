using Shop.Api.DTO.Common;
using Shop.Api.DTO.Product;

namespace Shop.Api.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAll();
        Task<ProductResponseDTO> GetById(Guid id);
        Task<ProductResponseDTO> Create(ProductCreateDTO productCreateDTO);
        Task<ProductResponseDTO> Update(Guid id, ProductUpdateDTO productUpdateDTO);
        Task Delete(Guid id);
        Task<List<ProductResponseDTO>> GetFiltered(ProductQueryParameters query);
    }
}
