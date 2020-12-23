using Microsoft.EntityFrameworkCore;
using MovieMvc.Models;

namespace MovieMvc.Data
{
    public class AppDb : DbContext
    {
        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
            //
        }

        public DbSet<Movies> Movies { get; set; }
    }

   
}
