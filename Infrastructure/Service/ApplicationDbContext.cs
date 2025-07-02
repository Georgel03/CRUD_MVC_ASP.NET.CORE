using CrudMvc.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudMvc.Infrastructure.Service
{
    public class ApplicationDbContext : DbContext
    {
       public DbSet<Product> Products { get; set; }
       public ApplicationDbContext(DbContextOptions options) : base(options)
       {
           
       }  
    }
}
