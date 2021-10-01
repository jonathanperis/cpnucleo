using Cpnucleo.Application.Interfaces;

namespace Cpnucleo.Application.Services;

internal class SistemaAppService : ISistemaAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SistemaAppService(IUnitOfWork unitOfWork,
                                 IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SistemaViewModel> AddAsync(SistemaViewModel viewModel)
    {
        SistemaViewModel result = _mapper.Map<SistemaViewModel>(await _unitOfWork.SistemaRepository.AddAsync(_mapper.Map<Sistema>(viewModel)));
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<SistemaViewModel>> AllAsync(bool getDependencies = false)
    {
        return _mapper.Map<IEnumerable<SistemaViewModel>>(await _unitOfWork.SistemaRepository.AllAsync(getDependencies));
    }

    public async Task<SistemaViewModel> GetAsync(Guid id)
    {
        return _mapper.Map<SistemaViewModel>(await _unitOfWork.SistemaRepository.GetAsync(id));
    }

    public async Task RemoveAsync(Guid id)
    {
        await _unitOfWork.SistemaRepository.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(SistemaViewModel viewModel)
    {
        _unitOfWork.SistemaRepository.Update(_mapper.Map<Sistema>(viewModel));
        await _unitOfWork.SaveChangesAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
