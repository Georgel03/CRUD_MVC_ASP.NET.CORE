using CrudMvc.Application.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CrudMvc.Application
{
    public interface IProductService
    {
        public Task<List<ProductDTO>> GetAllAsync();

        public Task<ProductDTO> GetByIdAsync(int id);

        public Task CreateAsync(CreateProductDTO dto);
        public Task UpdateAsync(int id, CreateProductDTO dto);

        public Task DeleteAsync(int id);

        public Task<PaginatedProductDTO> GetFilteredAsync(ProductQueryDTO query);
    }
}
