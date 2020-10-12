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
    public class ReOrderAsync
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
        public async Task ShouldReOrderTwoBlocks()
        {
            // var pageWithBlocksId = Guid.Parse("08d86a58-8cde-4f61-8831-ae7d5f8705e6");

            var blockService = new Binding.Services.BlockService(_context);

            var block1 = Guid.Parse("A13E6844-84C1-44AD-8C29-B60CC542934D");
            var block2 = Guid.Parse("D9E10FFE-1022-48F1-8593-9F1B29A97C33");

            var swapped = await blockService.ReOrderAsync(block2, block1);

            swapped.Should().BeTrue();
            
            var block1Now = await _context.Blocks.FirstOrDefaultAsync(x => x.Id == block1);
            var block2Now = await _context.Blocks.FirstOrDefaultAsync(x => x.Id == block2);

            block1Now.Order.Should().BeGreaterThan(block2Now.Order);
        }
    }
}