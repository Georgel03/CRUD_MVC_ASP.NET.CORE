namespace CrudMvc.Application.DTO
{
    public class ProductQueryDTO
    {
        public string? SearchItem { get; set; }

        public string? SortColumn { get; set; }

        public string? SortOrder { get; set; } = "asc";

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;



    }
}
