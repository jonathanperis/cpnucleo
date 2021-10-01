namespace Cpnucleo.Application.Interfaces;

public interface IGenericAppService<TViewModel> : IDisposable
{
    Task<TViewModel> AddAsync(TViewModel viewModel);

    Task UpdateAsync(TViewModel entity);

    Task<TViewModel> GetAsync(Guid id);

    Task<IEnumerable<TViewModel>> AllAsync(bool getDependencies = false);

    Task RemoveAsync(Guid id);
}
