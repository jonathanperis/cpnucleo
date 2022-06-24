using Cpnucleo.Shared.Commands.ImpedimentoTarefa;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Application.Commands.ImpedimentoTarefa;

public class CreateImpedimentoTarefaHandler : IRequestHandler<CreateImpedimentoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateImpedimentoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(_mapper.Map<Domain.Entities.ImpedimentoTarefa>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
