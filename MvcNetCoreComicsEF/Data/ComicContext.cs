using Microsoft.EntityFrameworkCore;
using MvcNetCoreComicsEF.Models;

namespace MvcNetCoreComicsEF.Data
{
    public class ComicContext : DbContext
    {
        public ComicContext(DbContextOptions<ComicContext> options)
            : base(options) { }

        public DbSet<Comic> Comics { get; set; }
    }
}
