using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Binding.Contexts;
using Binding.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Binding.Services
{
    public interface IUserService
    {
        Task<UserViewModel> Authenticate(string email, string password);
        Task<List<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<UserViewModel> GetAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
        Task<User> UpdateAsync(User user);
    }

    public class UserService : IUserService
    {
        private readonly BindingContext _bindingContext;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(BindingContext bindingContext, IMapper mapper,IOptions<AppSettings> appSettings)
        {
            _bindingContext = bindingContext;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }
        
        public async Task<UserViewModel> Authenticate(string email, string password)
        {
            PasswordHasher<UserViewModel> hasher = new PasswordHasher<UserViewModel>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );
            
            var user = await _bindingContext.Users.Select(v => new UserViewModel()
            {
                Id = v.Id,
                Created = v.Created,
                Email = v.Email,
                Password = v.Password,
                Updated = v.Updated,
                DisplayName = v.DisplayName,
                Pages = v.Pages.Where(x => x.Parent == null).Select(p => new PageWithNoBlocksViewModel()
                {
                    Id = p.Id,
                    Created = p.Created,
                    Name = p.Name,
                    Order = p.Order,
                    Updated = p.Updated,
                    Children = p.Childern.Select(child => _mapper.Map<PageWithNoBlocksViewModel>(child)).ToList()
                }).ToList()
            }).FirstOrDefaultAsync(x => x.Email == email);
            
            if (hasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
            {
                Console.WriteLine("Password incorrect");
                throw new Exception("Password incorrect");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;

            return user;
        }

        public Task<List<User>> GetAllAsync()
        {
            return _bindingContext.Users.Include(x => x.Pages).ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            var exists = await _bindingContext.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (exists != null)
            {
                Console.WriteLine("User already exists");
                throw new Exception("User already exists");
            }
            
            PasswordHasher<User> hasher = new PasswordHasher<User>(
                new OptionsWrapper<PasswordHasherOptions>(
                    new PasswordHasherOptions()
                    {
                        CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV2
                    })
            );

            user.Password = hasher.HashPassword(user, user.Password);
            user.Created = DateTime.Now;
            user.Updated = DateTime.Now;

            try
            {
                await _bindingContext.Users.AddAsync(user);
                await _bindingContext.SaveChangesAsync();
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            throw new NotImplementedException();
        }

        public async Task<UserViewModel> GetAsync(Guid id)
        {
            var user = await _bindingContext.Users.Select(v => new UserViewModel()
            {
                Id = v.Id,
                Created = v.Created,
                Email = v.Email,
                Updated = v.Updated,
                DisplayName = v.DisplayName,
                Pages = v.Pages.Where(x => x.Parent == null).Select(p => new PageWithNoBlocksViewModel()
                {
                    Id = p.Id,
                    Created = p.Created,
                    Name = p.Name,
                    Order = p.Order,
                    Updated = p.Updated,
                    Children = p.Childern.Select(child => _mapper.Map<PageWithNoBlocksViewModel>(child)).ToList()
                }).ToList()
            }).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new Exception("User Not Found");
            }

            return user;
        }

        public Task<User> UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}