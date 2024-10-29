using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_lkelly3.Models;

namespace Fall2024_Assignment3_lkelly3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fall2024_Assignment3_lkelly3.Models.Actors> Actors { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_lkelly3.Models.Movies> Movies { get; set; } = default!;
        public DbSet<Fall2024_Assignment3_lkelly3.Models.MovieActor> MovieActor { get; set; } = default!;
    }
}
