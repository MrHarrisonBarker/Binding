using System;
using System.Threading.Tasks;

namespace Binding.Services
{
    public interface IBindingService<T>
    {
        Task<T> CreateAsync(T obj);
        Task<T> GetAsync(Guid id);
        Task<T> UpdateAsync(Guid id, T obj);
        Task<bool> DeleteAsync(Guid id);
    }
}