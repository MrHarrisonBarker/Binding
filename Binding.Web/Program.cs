using System;
using System.Collections.Generic;
using System.Linq;
using Binding.Contexts;
using Binding.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Binding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            SeedDatabase(host);
            host.Run();
        }

        private static void SeedDatabase(IHost host)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BindingContext>();
            optionsBuilder.UseMySql("server=localhost;database=BindingDB;user=darth;password=Star3wars");
            var dbContext = new BindingContext(optionsBuilder.Options);

            if (!dbContext.Users.Any())
            {
                AddData(dbContext);
            }
        }

        private static void AddData(BindingContext bindingContext)
        {
            Console.WriteLine("Seeding data");

            var userId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var blockId = Guid.NewGuid();
            var childPageId = Guid.NewGuid();

            var block = new Block()
            {
                Type = BlockType.Heading,
                Content = "Hello world",
                Id = blockId,
                Order = 0,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            var childPage = new Page()
            {
                Id = childPageId,
                Name = "This is a child page",
                Order = 0,
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            var page = new Page()
            {
                Id = pageId,
                Name = "New Page",
                Order = 0,
                Blocks = new List<Block>() {block},
                Childern = new List<Page>() {childPage},
                Created = DateTime.Now,
                Updated = DateTime.Now
            };

            var user = new User()
            {
                Email = "harrison@thebarkers.me.uk",
                Id = userId,
                Password = "Password",
                DisplayName = "Harrison Barker",
                Pages = new List<Page>() {page, childPage}
            };

            bindingContext.Users.Add(user);
            bindingContext.Pages.Add(page);
            bindingContext.Blocks.Add(block);

            bindingContext.SaveChanges();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}