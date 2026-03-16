using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
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
            try
            {
                await _context.Products.AddAsync(Product);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Ошибка добавления товара");
            }
        }

        public async Task Delete(Guid Id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
                if(product.IsDeleted == false)
                {
                    product.IsDeleted = true;
                }
                else { throw new Exception($"Товар c Id {Id} уже удален"); }
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception($"Товар c Id {Id} не найден");
            }
        }

        public async Task<List<Product>> GetAll()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch
            {
                throw new Exception("Ошибка получения товаров");
            }
        }

        public async Task<Product> GetById(Guid Id)
        {
            try
            {
                return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == Id);
            }
            catch
            {
                throw new Exception($"Товар c Id {Id} не найден");
            }
        }

        public async Task Update(Product Product)
        {
            try
            {
                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == Product.Id);
                await _context.SaveChangesAsync();
            }
            catch 
            {
                throw new Exception("Ошибка обновления продукта");
            }
        }
    }
}
