using System;
using System.Linq;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Test.Seeds;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Binding.Test.Services.UserService
{
    [TestFixture]
    public class GetAllAsync
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
        public async Task ReturnsListOfUniqueUsers()
        {
            var userService = new Binding.Services.UserService(_context);
            var users = await userService.GetAllAsync();

            users.Should().HaveCount(c => c > 0).And.OnlyHaveUniqueItems();
        }

        [Test]
        public async Task UsersPageShouldBeOwnedByUser()
        {
            var userService = new Binding.Services.UserService(_context);
            var users = await userService.GetAllAsync();

            users.ForEach(user =>
            {
                if (user.Pages.Any())
                {
                    foreach (var page in user.Pages)
                    {
                        Console.WriteLine($"{user.Id} -> {page.Owner.Id}");
                        page.Owner.Id.Should().Be(user.Id);
                    }
                }
            });
        }
    }
}