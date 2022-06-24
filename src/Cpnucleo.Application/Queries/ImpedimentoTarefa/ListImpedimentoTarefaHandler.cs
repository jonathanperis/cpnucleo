using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.ImpedimentoTarefa;

namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public class ListImpedimentoTarefaHandler : IRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListImpedimentoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListImpedimentoTarefaViewModel> Handle(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.ImpedimentoTarefa> impedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.AllAsync(request.GetDependencies);

        if (impedimentoTarefas == null)
        {
            return new ListImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ImpedimentoTarefaDTO> result = _mapper.Map<IEnumerable<ImpedimentoTarefaDTO>>(impedimentoTarefas);

        return new ListImpedimentoTarefaViewModel { ImpedimentoTarefas = result, OperationResult = OperationResult.Success };
    }
}
