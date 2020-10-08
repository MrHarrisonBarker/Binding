using System;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Test.Seeds;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.BlockService
{
    [TestFixture]
    public class GetAsync
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
        public async Task ReturnsCorrectBlock()
        {
            var blockService = new Binding.Services.BlockService(_context);
            
            var blockId = Guid.Parse("1ed89919-95f0-4a0d-8352-f71a39fb32dc");

            var block = await blockService.GetAsync(blockId);

            block.Id.Should().Be(blockId);
        }    
    }
}