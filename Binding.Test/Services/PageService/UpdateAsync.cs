using System;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.PageService
{
    [TestFixture]
    public class UpdateAsync
    {
        private BindingContext _context;

        [SetUp]
        public async Task Setup()
        {
            _context = await new BasicSetup().Setup();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }

        [Test]
        public async Task ShouldUpdatePage()
        {
            var page = new Page()
            {
                Id = new Guid("08d86a58-8c6e-4a0c-8af8-8dee9e95ed66"),
                Name = "Empty Page, has been updated",
            };
            
            var pageService = new Binding.Services.PageService(_context);

            var updatedBlock = await pageService.UpdateAsync(page);
            updatedBlock.Should().Be(page);

            var check = await _context.Pages.FirstOrDefaultAsync(x => x.Id == page.Id);
            check.Should().Be(page);
        }

        [Test]
        public async Task ShouldNotFindPage()
        {
            var page = new Page()
            {
                Id = Guid.Empty,
                Name = "Empty Page, does not exist",
            };
            
            var pageService = new Binding.Services.PageService(_context);
            
            FluentActions.Invoking(async () => await pageService.UpdateAsync(page))
                .Should().Throw<Exception>()
                .WithMessage("Page not found");
        }
    }
}