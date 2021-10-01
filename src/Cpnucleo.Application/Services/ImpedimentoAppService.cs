using Cpnucleo.Application.Interfaces;

namespace Cpnucleo.Application.Services;

internal class ImpedimentoAppService : IImpedimentoAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ImpedimentoAppService(IUnitOfWork unitOfWork,
                                 IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ImpedimentoViewModel> AddAsync(ImpedimentoViewModel viewModel)
    {
        ImpedimentoViewModel result = _mapper.Map<ImpedimentoViewModel>(await _unitOfWork.ImpedimentoRepository.AddAsync(_mapper.Map<Impedimento>(viewModel)));
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<ImpedimentoViewModel>> AllAsync(bool getDependencies = false)
    {
        return _mapper.Map<IEnumerable<ImpedimentoViewModel>>(await _unitOfWork.ImpedimentoRepository.AllAsync(getDependencies));
    }

    public async Task<ImpedimentoViewModel> GetAsync(Guid id)
    {
        return _mapper.Map<ImpedimentoViewModel>(await _unitOfWork.ImpedimentoRepository.GetAsync(id));
    }

    public async Task RemoveAsync(Guid id)
    {
        await _unitOfWork.ImpedimentoRepository.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(ImpedimentoViewModel viewModel)
    {
        _unitOfWork.ImpedimentoRepository.Update(_mapper.Map<Impedimento>(viewModel));
        await _unitOfWork.SaveChangesAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
