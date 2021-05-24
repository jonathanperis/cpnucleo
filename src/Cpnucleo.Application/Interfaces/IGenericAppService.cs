using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Interfaces
{
    public interface IGenericAppService<TViewModel> : IDisposable
    {
        Task<TViewModel> AddAsync(TViewModel viewModel);

        void Update(TViewModel entity);

        Task<TViewModel> GetAsync(Guid id);

        Task<IEnumerable<TViewModel>> AllAsync(bool getDependencies = false);

        Task RemoveAsync(Guid id);

        Task<bool> SaveChangesAsync();
    }
}
