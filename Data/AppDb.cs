using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MovieMvc.Models;

namespace MovieMvc.Data
{
    public class AppDb : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
            //
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseLoggerFactory(MyLoggerFactory)
            .EnableSensitiveDataLogging();
            


        }
        public DbSet<Movies> Movies { get; set; }
    }

   
}
