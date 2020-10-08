using System;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.BlockService
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
        public async Task ShouldUpdateBlock()
        {
            
            // var originalBlock = new Block()
            // {
            //     Type = BlockType.Heading,
            //     Content = "Hello world",
            //     Id = new Guid("1ed89919-95f0-4a0d-8352-f71a39fb32dc")
            // };
            
            // Change to text, change content
            var block = new Block()
            {
                Type = BlockType.Text,
                Content = "Hello world, This has been updated",
                Id = new Guid("1ed89919-95f0-4a0d-8352-f71a39fb32dc")
            };

            var blockService = new Binding.Services.BlockService(_context);

            var updatedBlock = await blockService.UpdateAsync(block);
            updatedBlock.Should().Be(block);

            var check = await _context.Blocks.FirstOrDefaultAsync(x => x.Id == block.Id);
            check.Should().Be(block);

        }

        [Test]
        public async Task ShouldNotFindBlock()
        {
            var block = new Block()
            {
                Type = BlockType.Text,
                Content = "Hello world, This has been updated",
                Id = Guid.Empty
            };

            var blockService = new Binding.Services.BlockService(_context);
            
            var updatedBlock = blockService.UpdateAsync(block);

            updatedBlock.Exception.Message.Should().Be("Block not found");

            // await updatedBlock.Awaiting().Should().ThrowAsync<>()
        }
    }
}