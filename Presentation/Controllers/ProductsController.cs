using CrudMvc.Application;
using CrudMvc.Application.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CrudMvc.Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        
        public ProductsController(IProductService productService) {

            this.productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await productService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Index(
        string? search,
        string? sort,
        string order = "asc",
        int page = 1)
        {
            var query = new ProductQueryDTO
            {
                SearchItem = search,
                SortColumn = sort,
                SortOrder = order,
                Page = page,
                PageSize = 5
            };

            var model = await productService.GetFilteredAsync(query);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }
            await productService.CreateAsync(productDTO);
            return RedirectToAction("Index", "Products");
        
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await productService.GetByIdAsync(id);
            if (product == null) return RedirectToAction(nameof(Index));

            var dto = new CreateProductDTO
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description
                // ImageFile cannot be preloaded in HTML
            };

            ViewData["ProductId"] = product.Id;
            ViewData["ImageFileName"] = product.ImageUrl;
            ViewData["CreatedAt"] = DateTime.Now.ToString("MM/dd/yyyy");

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, CreateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(productDTO);
            }

            await productService.UpdateAsync(id, productDTO);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task <IActionResult> Delete(int id)
        {
            await productService.DeleteAsync(id);
            return RedirectToAction("Index", "Products");
        }

        

    }
}
