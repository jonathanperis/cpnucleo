using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.RecursoTarefa;

namespace Cpnucleo.Application.Queries.RecursoTarefa;

public class ListRecursoTarefaHandler : IRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListRecursoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListRecursoTarefaViewModel> Handle(ListRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.RecursoTarefa> recursoTarefas = await _unitOfWork.RecursoTarefaRepository.AllAsync(request.GetDependencies);

        if (recursoTarefas == null)
        {
            return new ListRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoTarefaDTO> result = _mapper.Map<IEnumerable<RecursoTarefaDTO>>(recursoTarefas);

        return new ListRecursoTarefaViewModel { RecursoTarefas = result, OperationResult = OperationResult.Success };
    }
}
