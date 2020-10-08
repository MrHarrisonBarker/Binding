using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Binding.Contexts;
using Binding.Test.Seeds;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Binding.Test.Services.PageService
{
    [TestFixture]
    public class DeleteAsync
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
        public async Task ShouldFailIfNoPage()
        {
            var pageService = new Binding.Services.PageService(_context);

            var hasDeleted = await pageService.DeleteAsync(Guid.Empty);

            hasDeleted.Should().BeFalse();
        }

        [Test]
        public async Task ShouldDeleteEmptyPage()
        {
            var emptyPageId = Guid.Parse("08d86a58-8c6e-4a0c-8af8-8dee9e95ed66");
            var pageService = new Binding.Services.PageService(_context);

            var hasDeleted = await pageService.DeleteAsync(emptyPageId);

            hasDeleted.Should().BeTrue();

            (await _context.Pages.FirstOrDefaultAsync(x => x.Id == emptyPageId)).Should().BeNull();
        }

        // testing that the service deletes the page and the associated blocks
        [Test]
        public async Task ShouldDeletePageAndBlocks()
        {
            var pageWithBlocksId = Guid.Parse("08d86a58-8cde-4f61-8831-ae7d5f8705e6");

            var pageService = new Binding.Services.PageService(_context);

            List<Guid> blockIds = new List<Guid>
            {
                Guid.Parse("D9E10FFE-1022-48F1-8593-9F1B29A97C33"),
                Guid.Parse("A13E6844-84C1-44AD-8C29-B60CC542934D")
            };

            var hasDeleted = await pageService.DeleteAsync(pageWithBlocksId);
            hasDeleted.Should().BeTrue();

            var unDeletedBlocks = await _context.Blocks.Where(x => blockIds.Contains(x.Id)).ToListAsync();
            unDeletedBlocks.Count.Should().Be(0);
        }

        [Test]
        public async Task ShouldDeletePageAndChildren()
        {
            var pageWithChildrenId = Guid.Parse("AB7DFBF6-0EFD-4425-AC7F-84239116AB95");
            List<Guid> childIds = new List<Guid>()
            {
                new Guid("80FC8B21-1816-4648-87E4-E8052056A823"), 
                new Guid("7074922C-0F9E-4383-8291-ED3A0A1C3006")
            };

            var pageService = new Binding.Services.PageService(_context);
            
            var hasDeleted = await pageService.DeleteAsync(pageWithChildrenId);
            hasDeleted.Should().BeTrue();

            var childPages = await _context.Pages.Where(x => childIds.Contains(pageWithChildrenId)).ToListAsync();
            childPages.Count.Should().Be(0);
        }
    }
}