using Shop.Api.DTO.Product;

namespace Shop.Api.Services
{
    public interface IProductService
    {
        public Task<List<ProductResponseDTO>> GetAll();
        public Task<ProductResponseDTO> GetById(Guid id);
        public Task<ProductResponseDTO> Create(ProductCreateDTO productCreateDTO);
        public Task<ProductResponseDTO> Update(Guid id, ProductUpdateDTO productUpdateDTO);
        public Task Delete(Guid id);
    }
}
