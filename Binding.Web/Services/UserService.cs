using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Binding.Contexts;
using Binding.Models;
using Microsoft.EntityFrameworkCore;

namespace Binding.Services
{
    public interface IUserService<T> : IBindingService<T> where T : User
    {
        Task<List<User>> GetAllAsync();
    }

    public class UserService : IUserService<User>
    {
        private readonly BindingContext _bindingContext;

        public UserService(BindingContext bindingContext)
        {
            _bindingContext = bindingContext;
        }

        public Task<List<User>> GetAllAsync()
        {
            return _bindingContext.Users.Include(x => x.Pages).ToListAsync();
        }

        public Task<User> CreateAsync(User obj)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(Guid id, User obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}