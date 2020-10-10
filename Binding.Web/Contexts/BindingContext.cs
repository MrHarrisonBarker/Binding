using Binding.Models;
using Microsoft.EntityFrameworkCore;

namespace Binding.Contexts
{
    public class BindingContext: DbContext
    {
        public BindingContext(DbContextOptions<BindingContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Block> Blocks { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.Pages)
                .WithOne(x => x.Owner);

            modelBuilder.Entity<Page>()
                .HasMany(x => x.Blocks)
                .WithOne(x => x.Page);

            modelBuilder.Entity<Page>()
                .HasMany(x => x.Children)
                .WithOne(x => x.Parent);
        }
    }
}