using System;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Binding.Test.Setups;
using ET.FakeText;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.BlockService
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
        public async Task ShouldCreatesBlock()
        {
            var pageId = Guid.Parse("b1af5bdb-3771-4c17-baa4-346814bb6ecc");
            var block = new Block()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Type = (BlockType) new Random().Next(Enum.GetValues(typeof(BlockType)).Length),
                Content = new TextGenerator().GenerateText(20)
            };
            
            var blockService = new Binding.Services.BlockService(_context);

            var newBlock = await blockService.CreateAsync(block, pageId);

            newBlock.Should().NotBeNull();
            newBlock.Page.Id.Should().Be(pageId);
        }

        [Test]
        public async Task ShouldFailIfNoPage()
        {
            
            var block = new Block()
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                Type = (BlockType) new Random().Next(Enum.GetValues(typeof(BlockType)).Length),
                Content = new TextGenerator().GenerateText(20)
            };
            
            var blockService = new Binding.Services.BlockService(_context);

            var newBlock = await blockService.CreateAsync(block, Guid.Empty);

            newBlock.Should().BeNull();
        }
    }
}