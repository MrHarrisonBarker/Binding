using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Binding.Contexts;
using Binding.Mapping;
using Binding.Test.Seeds;
using Binding.Test.Setups;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Binding.Test.Services.UserService
{
    [TestFixture]
    public class GetAllAsync
    {
        private BindingContext _context;
        private IMapper _mapper;
        private IOptions<AppSettings> _appSettings;

        [SetUp]
        public async Task Setup()
        {
            var myProfile = new AutoMapping();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            _mapper = new Mapper(configuration);

            _context = await new BasicSetup().Setup();

            _appSettings = Options.Create(new AppSettings()
            {
                Secret = "this is a secret"
            });
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
            var userService = new Binding.Services.UserService(_context, _mapper, _appSettings);
            var users = await userService.GetAllAsync();

            users.Should().HaveCount(c => c > 0).And.OnlyHaveUniqueItems();
        }

        [Test]
        public async Task UsersPageShouldBeOwnedByUser()
        {
            var userService = new Binding.Services.UserService(_context, _mapper, _appSettings);
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