using Shop.Api.DTO.Product;
using Shop.Api.Models;
using Shop.Api.Repositories;
using AutoMapper;

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
            try
            {
                var product = _mapper.Map<Product>(productCreateDTO);
                product.Id = Guid.NewGuid();
                product.CreatedAt = DateTime.UtcNow;
                await _productRepository.Create(product);
                return _mapper.Map<ProductResponseDTO>(product);
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
                return _mapper.Map<List<ProductResponseDTO>>(products);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
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
    }
}
