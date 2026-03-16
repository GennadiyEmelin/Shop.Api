using System.ComponentModel.DataAnnotations;

namespace Shop.Api.DTO.Product
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Введите наименование товара!")]
        [MinLength(3, ErrorMessage = "Наименование товара должно быть не меньше 3 символов")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Введите стоимость товара!")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Стоимость товара должна быть больше 0!")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Введите описание товара!")]
        [MinLength(5, ErrorMessage = "Описание товара должно быть не меньше 5 символов")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Введите количество товара на складе!")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество товара на складе должно быть не меньше 0!")]
        public int Stock { get; set; }
    }
}
