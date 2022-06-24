using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Apontamento;

namespace Cpnucleo.Application.Queries.Apontamento;

public class GetApontamentoByRecursoHandler : IRequestHandler<GetApontamentoByRecursoQuery, GetApontamentoByRecursoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetApontamentoByRecursoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetApontamentoByRecursoViewModel> Handle(GetApontamentoByRecursoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Apontamento> apontamentos = await _unitOfWork.ApontamentoRepository.GetApontamentoByRecursoAsync(request.IdRecurso);

        if (apontamentos == null)
        {
            return new GetApontamentoByRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ApontamentoDTO> result = _mapper.Map<IEnumerable<ApontamentoDTO>>(apontamentos);

        return new GetApontamentoByRecursoViewModel { Apontamentos = result, OperationResult = OperationResult.Success };
    }
}
