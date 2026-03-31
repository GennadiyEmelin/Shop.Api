using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.DTO.Common;
using Shop.Api.DTO.Product;
using Shop.Api.Models;

namespace Shop.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task Create(Product Product)
        {
            if(Product == null) 
                throw new ArgumentException("Ошибка добавления продукта");
            await _context.Products.AddAsync(Product);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid Id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
            if(product == null)
                throw new KeyNotFoundException($"Товар c Id {Id} не найден");
            if (product.IsDeleted == false)
            {
                product.IsDeleted = true;
            }
            else { throw new ArgumentException($"Товар c Id {Id} уже удален"); }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.Where(p => p.IsDeleted == false).ToListAsync();
        }

        public async Task<Product> GetById(Guid Id)
        {
            var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id);
            if(product == null || product.IsDeleted == true)
                throw new KeyNotFoundException($"Товар c Id {Id} не найден");
            return product;
        }

        public async Task Update(Product Product)
        {
            var existing = await _context.Products.FirstOrDefaultAsync(p => p.Id == Product.Id);

            if (existing == null)
                throw new KeyNotFoundException("Продукт не найден");

            existing.Name = Product.Name;
            existing.Price = Product.Price;
            existing.Description = Product.Description;
            existing.Stock = Product.Stock;

            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetFiltered(ProductQueryParameters query)
        {
            var products = _context.Products
                .Where(p => !p.IsDeleted)
                .AsQueryable();

            if (query.MinPrice.HasValue)
                products = products.Where(p => p.Price >= query.MinPrice.Value);

            if (query.MaxPrice.HasValue)
                products = products.Where(p => p.Price <= query.MaxPrice.Value);

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                if (query.SortBy.ToLower() == "price")
                {
                    products = query.Order == "desc"
                        ? products.OrderByDescending(p => p.Price)
                        : products.OrderBy(p => p.Price);
                }
            }

            products = products
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize);

            return await products.ToListAsync();
        }
    }
}
