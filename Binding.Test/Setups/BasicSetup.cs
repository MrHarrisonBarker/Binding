using System;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Test.Seeds;
using Microsoft.EntityFrameworkCore;

namespace Binding.Test.Setups
{
    public class BasicSetup
    {
        public async Task<BindingContext> Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<BindingContext>()
                .UseInMemoryDatabase(databaseName: "BindingDB").Options;

            await using var context = new BindingContext(dbContextOptions);
            Console.WriteLine("Starting data seed");
            await BindingContextSeed.SeedAsync(context);
            return new BindingContext(dbContextOptions);
        }
    }
}