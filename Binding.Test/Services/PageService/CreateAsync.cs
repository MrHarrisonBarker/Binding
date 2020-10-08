using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Binding.Test.Setups;
using ET.FakeText;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.PageService
{
    [TestFixture]
    public class CreateAsync
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
        public async Task ShouldNotFindOwner()
        {
            var page = new Page()
            {
                Name = "Empty Test Page"
            };

            var pageService = new Binding.Services.PageService(_context);

            var newPage = await pageService.CreateAsync(page, Guid.Empty);
            
            newPage.Should().BeNull();
        }
        

        [Test]
        public async Task CreatesEmptyPage()
        {
            var userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");
            var page = new Page()
            {
                Name = "Empty Test Page"
            };

            var pageService = new Binding.Services.PageService(_context);

            var newPage = await pageService.CreateAsync(page, userId);
            
            newPage.Should().NotBeNull();
            newPage.Owner.Id.Should().Be(userId);
        }

        [Test]
        public async Task CreatesEmptyPageWithParent()
        {
            var userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");
            var parentPageId = Guid.Parse("b1af5bdb-3771-4c17-baa4-346814bb6ecc");
            
            var page = new Page()
            {
                Name = "Empty Test Page",
                Parent = new Page()
                {
                    Id = parentPageId
                }
            };

            var pageService = new Binding.Services.PageService(_context);

            var newPage = await pageService.CreateAsync(page, userId);

            newPage.Should().NotBeNull();
            newPage.Owner.Id.Should().Be(userId);

            var parent = await _context.Pages.FirstOrDefaultAsync(x => x.Id == parentPageId);
            parent.Childern.Where(x => x.Id == newPage.Id).Should().NotBeNull();
        }

        [Test]
        public async Task ShouldNotFindParent()
        {
            var userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");
            var page = new Page()
            {
                Name = "Empty Test Page",
                Parent = new Page()
                {
                    Id = Guid.Empty
                }
            };

            var pageService = new Binding.Services.PageService(_context);

            var newPage = await pageService.CreateAsync(page, userId);

            newPage.Should().BeNull();
        }

        [Test]
        public async Task CreatesPageWithSomeBlocks()
        {
            var userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");

            var blocks = new List<Block>();
            var randomText = new TextGenerator();
            for (int i = 0; i < 10; i++)
            {
                blocks.Add(new Block()
                {
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Type = (BlockType) new Random().Next(Enum.GetValues(typeof(BlockType)).Length),
                    Content = randomText.GenerateText(20)
                });
            }

            var page = new Page()
            {
                Name = "Empty Test Page",
                Blocks = blocks
            };
            
            var pageService = new Binding.Services.PageService(_context);

            var newPage = await pageService.CreateAsync(page, userId);
            
            newPage.Should().NotBeNull();
            newPage.Owner.Id.Should().Be(userId);
            newPage.Blocks.Count.Should().Be(10);
            
            foreach (var newPageBlock in newPage.Blocks)
            {
                newPageBlock.Should().NotBeNull();
            }
        }

        // TODO: Children?
        [Test]
        public async Task ShouldCreatePageAndChildren()
        {
            var userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");
            var page = new Page()
            {
                Name = "Empty Test Page",
                Childern = new List<Page>()
                {
                    new Page()
                    {
                        Name = "Child Page 1",
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                }
            };

            var pageService = new Binding.Services.PageService(_context);

            var newPage = await pageService.CreateAsync(page, userId);
        }
    }
}