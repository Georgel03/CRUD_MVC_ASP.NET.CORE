namespace CrudMvc.Application.DTO
{
    public class PaginatedProductDTO
    {
        public List<ProductDTO> Products { get; set; }
        public int TotalPages { get; set; }
        public ProductQueryDTO Query { get; set; }
    }
}
