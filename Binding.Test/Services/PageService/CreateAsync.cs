using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Binding.Contexts;
using Binding.Mapping;
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
        private IMapper _mapper;

        [SetUp]
        public async Task Setup()
        {
            _context = await new BasicSetup().Setup();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapping()));
            _mapper = new Mapper(configuration);
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

            var pageService = new Binding.Services.PageService(_context, _mapper);

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

            var pageService = new Binding.Services.PageService(_context, _mapper);

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

            var pageService = new Binding.Services.PageService(_context, _mapper);

            var newPage = await pageService.CreateAsync(page, userId);

            newPage.Should().NotBeNull();
            newPage.Owner.Id.Should().Be(userId);

            var parent = await _context.Pages.FirstOrDefaultAsync(x => x.Id == parentPageId);
            parent.Children.Where(x => x.Id == newPage.Id).Should().NotBeNull();
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

            var pageService = new Binding.Services.PageService(_context, _mapper);

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

            var pageService = new Binding.Services.PageService(_context, _mapper);

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
                Children = new List<Page>()
                {
                    new Page()
                    {
                        Name = "Child Page 1",
                        Created = DateTime.Now,
                        Updated = DateTime.Now
                    }
                }
            };

            var pageService = new Binding.Services.PageService(_context, _mapper);

            var newPage = await pageService.CreateAsync(page, userId);
            // TODO: Finish
        }

        [Test]
        public async Task ShouldIncrementOrder()
        {
            var userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");
            var page1 = new Page()
            {
                Name = "Empty Test Page 1"
            };

            var page2 = new Page()
            {
                Name = "Empty Test Page 2"
            };

            var pageService = new Binding.Services.PageService(_context, _mapper);

            var p1 = await pageService.CreateAsync(page1, userId);
            var p2 = await pageService.CreateAsync(page2, userId);

            p2.Order.Should().BeGreaterThan(p1.Order);
        }

        [Test]
        public async Task ShouldIncrementOrderWithParent()
        {
            Guid userId = Guid.Parse("c385730a-91a5-429f-9c0c-b3e9bd4e5788");
            Guid parentId = new Guid("2D039F83-4775-4ADD-ADDA-8AE31A8A7924");

            var parent = new Page()
            {
                Id = parentId,
                Name = "Empty Parent Page"
            };

            var child1 = new Page()
            {
                Name = "Empty Test Child Page 1",
                Parent = new Page() {Id = parentId}
            };

            var child2 = new Page()
            {
                Name = "Empty Test Child Page 2",
                Parent = new Page() {Id = parentId}
            };

            var pageService = new Binding.Services.PageService(_context, _mapper);

            await pageService.CreateAsync(parent, userId);
            var child1Result = await pageService.CreateAsync(child1, userId);
            var child2Result = await pageService.CreateAsync(child2, userId);

            child2Result.Order.Should().BeGreaterThan(child1Result.Order);
        }
    }
}