using Cpnucleo.Shared.Commands.Projeto;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Application.Commands.Projeto;

public class CreateProjetoHandler : IRequestHandler<CreateProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ProjetoRepository.AddAsync(_mapper.Map<Domain.Entities.Projeto>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
