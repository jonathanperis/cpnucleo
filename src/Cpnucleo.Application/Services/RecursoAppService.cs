using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;

namespace Cpnucleo.Application.Services;

internal class RecursoAppService : IRecursoAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICryptographyManager _cryptographyManager;

    public RecursoAppService(IUnitOfWork unitOfWork, IMapper mapper, ICryptographyManager cryptographyManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cryptographyManager = cryptographyManager;
    }

    public async Task<RecursoViewModel> AddAsync(RecursoViewModel viewModel)
    {
        _cryptographyManager.CryptPbkdf2(viewModel.Senha, out string senhaCrypt, out string salt);

        viewModel.Senha = senhaCrypt;
        viewModel.Salt = salt;

        RecursoViewModel result = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.AddAsync(_mapper.Map<Recurso>(viewModel)));
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<IEnumerable<RecursoViewModel>> AllAsync(bool getDependencies = false)
    {
        IEnumerable<RecursoViewModel> result = _mapper.Map<IEnumerable<RecursoViewModel>>(await _unitOfWork.RecursoRepository.AllAsync(getDependencies));

        foreach (RecursoViewModel item in result)
        {
            item.Senha = null;
            item.Salt = null;
        }

        return result;
    }

    public async Task<RecursoViewModel> GetAsync(Guid id)
    {
        RecursoViewModel result = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetAsync(id));

        result.Senha = null;
        result.Salt = null;

        return result;
    }

    public async Task RemoveAsync(Guid id)
    {
        await _unitOfWork.RecursoRepository.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(RecursoViewModel viewModel)
    {
        _cryptographyManager.CryptPbkdf2(viewModel.Senha, out string senhaCrypt, out string salt);

        viewModel.Senha = senhaCrypt;
        viewModel.Salt = salt;

        _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(viewModel));
        await _unitOfWork.SaveChangesAsync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
