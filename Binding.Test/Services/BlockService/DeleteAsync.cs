using System;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.BlockService
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
        public async Task ShouldRemoveBlock()
        {
            // var pageId = Guid.Parse("b1af5bdb-3771-4c17-baa4-346814bb6ecc");
            var blockId = Guid.Parse("1ed89919-95f0-4a0d-8352-f71a39fb32dc");
            
            var blockService = new Binding.Services.BlockService(_context);

            var hasDeleted = await blockService.DeleteAsync(blockId);

            hasDeleted.Should().BeTrue();
        }

        [Test]
        public async Task ShouldNotFindBlock()
        {
            var blockId = Guid.Parse("00000000-0000-0000-0000-000000000000");
            
            var blockService = new Binding.Services.BlockService(_context);

            var hasDeleted = await blockService.DeleteAsync(blockId);

            hasDeleted.Should().BeFalse();
        }
    }
}