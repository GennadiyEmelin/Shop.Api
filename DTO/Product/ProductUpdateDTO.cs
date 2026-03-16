namespace Shop.Api.DTO.Product
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
    }
}
