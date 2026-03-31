using AutoMapper;
using Shop.Api.DTO.Common;
using Shop.Api.DTO.Product;
using Shop.Api.Models;
using Shop.Api.Repositories;

namespace Shop.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ProductResponseDTO> Create(ProductCreateDTO productCreateDTO)
        {
            var product = _mapper.Map<Product>(productCreateDTO);
            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;
            await _productRepository.Create(product);
            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task Delete(Guid id)
        {
            await _productRepository.Delete(id);
        }

        public async Task<List<ProductResponseDTO>> GetAll()
        {
            var products = await _productRepository.GetAll();
            return _mapper.Map<List<ProductResponseDTO>>(products);
        }

        public async Task<ProductResponseDTO> GetById(Guid id)
        {
            var product = await _productRepository.GetById(id);
            return _mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> Update(Guid id, ProductUpdateDTO productUpdateDTO)
        {
            var existingProduct = await _productRepository.GetById(id);

            _mapper.Map(productUpdateDTO, existingProduct);

            await _productRepository.Update(existingProduct);

            return _mapper.Map<ProductResponseDTO>(existingProduct);
        }

        public async Task<List<ProductResponseDTO>> GetFiltered(ProductQueryParameters query)
        {
            var products = await _productRepository.GetFiltered(query);
            return _mapper.Map<List<ProductResponseDTO>>(products);
        } 
    }
}
