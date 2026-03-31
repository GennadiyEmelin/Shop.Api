namespace Shop.Api.DTO.Common
{
    public class ProductQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; }
        public string? Order { get; set; } = "asc";
    }
}
