using AutoMapper;
using CrudMvc.Application;
using CrudMvc.Application.DTO;
using CrudMvc.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata.Ecma335;
namespace CrudMvc.Infrastructure.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        private readonly IWebHostEnvironment environment;

        private readonly IMapper mapper;

        public ProductService(ApplicationDbContext context, IWebHostEnvironment environment, IMapper mapper) 
        { 
            this.context = context; 
            this.environment = environment;
            this.mapper = mapper;
        }
        public async Task CreateAsync(CreateProductDTO dto)
        {
            var product = mapper.Map<Product>(dto);
            product.CreatedAt = DateTime.Now;
            product.ImageFileName = dto.ImageFile != null && dto.ImageFile.Length > 0
                ? await SaveImageAsync(dto.ImageFile)
                : null;

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync(); 
        }

        public async Task DeleteAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Not found");
            }
            var oldImagePath = Path.Combine(environment.WebRootPath, "images", product.ImageFileName);
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        public async Task<List<ProductDTO>> GetAllAsync()
        {
            var products = await context.Products.AsNoTracking().ToListAsync();
            return mapper.Map<List<ProductDTO>>(products);

        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            return mapper.Map<ProductDTO>(product);

        }

        public async Task UpdateAsync(int id, CreateProductDTO dto)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Not found");
            }

            //if (!ModelState.IsValid)
            //{
            //    ViewData["ProductId"] = product.Id;
            //    ViewData["ImageFileName"] = product.ImageUrl;
            //    ViewData["CreatedAt"] = product.CreatedAt.ToString("MM/dd/yyyy");

            //    return View(productDTO);
            //}
            

            product.Name = dto.Name;
            product.Brand = dto.Brand;
            product.Category = dto.Category;
            product.Price = dto.Price;
            product.Description = dto.Description;
            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                var oldImagePath = Path.Combine(environment.WebRootPath, "images", product.ImageFileName);
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
                product.ImageFileName = await SaveImageAsync(dto.ImageFile);
            }
            product.CreatedAt = DateTime.Now;
            await context.SaveChangesAsync();
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid image file");
            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var imageFullPath = Path.Combine(environment.WebRootPath, "images");
            if (!Directory.Exists(imageFullPath))
            {
                Directory.CreateDirectory(imageFullPath);
            }
            var fullPath = Path.Combine(imageFullPath, newFileName);
            await using var stream = File.Create(fullPath);
            await file.CopyToAsync(stream);

            return newFileName;

        }

        public async Task<PaginatedProductDTO> GetFilteredAsync(ProductQueryDTO query)
        {
            //Filtration
            var productsQuery = context.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.SearchItem))
            {
                productsQuery = productsQuery
                    .Where(p => p.Name.ToLower().Contains(query.SearchItem.ToLower()) ||
                            p.Brand.ToLower().Contains(query.SearchItem.ToLower()) ||
                            p.Category.ToLower().Contains(query.SearchItem.ToLower()));

            }

            //Sorting
            productsQuery = query.SortColumn?.ToLower() switch
            {
                "name" => query.SortOrder.Equals("desc") ? productsQuery.OrderByDescending(p => p.Name) : productsQuery.OrderBy(p => p.Name),
                "price" => query.SortOrder.Equals("desc") ? productsQuery.OrderByDescending(p => p.Price) : productsQuery.OrderBy(p => p.Price),
                "createdat" => query.SortOrder.Equals("desc") ? productsQuery.OrderByDescending(p => p.CreatedAt) : productsQuery.OrderBy(p => p.CreatedAt),
                "id" => query.SortOrder.Equals("desc") ? productsQuery.OrderByDescending(p => p.Id) : productsQuery.OrderBy(p => p.Id),
                _ => productsQuery.OrderBy(p => p.Id)
            };

            //Pagination

            var total = await productsQuery.CountAsync();
            var skip = (query.Page - 1) * query.PageSize;
            productsQuery = productsQuery.Skip(skip).Take(query.PageSize);

            var products = await productsQuery.AsNoTracking().ToListAsync();
            var dtoItems = mapper.Map<List<ProductDTO>>(products);

            // Calculează total pagini
            var totalPages = (int)Math.Ceiling(total / (double)query.PageSize);

            return new PaginatedProductDTO
            {
                Products = dtoItems,
                Query = query,
                TotalPages = totalPages
            };
        }


    }
}
