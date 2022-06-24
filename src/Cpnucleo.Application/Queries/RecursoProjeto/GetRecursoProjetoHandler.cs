using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.RecursoProjeto;

namespace Cpnucleo.Application.Queries.RecursoProjeto;

public class GetRecursoProjetoHandler : IRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecursoProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRecursoProjetoViewModel> Handle(GetRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.RecursoProjeto recursoProjeto = await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id);

        if (recursoProjeto == null)
        {
            return new GetRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        RecursoProjetoDTO result = _mapper.Map<RecursoProjetoDTO>(recursoProjeto);

        return new GetRecursoProjetoViewModel { RecursoProjeto = result, OperationResult = OperationResult.Success };
    }
}
