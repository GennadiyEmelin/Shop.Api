using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.DTO.Common;
using Shop.Api.DTO.Product;
using Shop.Api.Services;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProducts()
        {
            return Ok(await _productService.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductCreateDTO productDTO)
        {
            await _productService.Create(productDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _productService.Delete(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetById(Guid id)
        {
            return Ok(await _productService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ProductUpdateDTO productUpdateDTO)
        {
            return Ok(await _productService.Update(id, productUpdateDTO));
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetFiltered([FromQuery] ProductQueryParameters query)
        {
            return Ok(await _productService.GetFiltered(query));
        }
    }
}
