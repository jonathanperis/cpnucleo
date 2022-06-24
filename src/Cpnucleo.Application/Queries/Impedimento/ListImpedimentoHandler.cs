using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Impedimento;

namespace Cpnucleo.Application.Queries.Impedimento;

public class ListImpedimentoHandler : IRequestHandler<ListImpedimentoQuery, ListImpedimentoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListImpedimentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListImpedimentoViewModel> Handle(ListImpedimentoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Impedimento> impedimentos = await _unitOfWork.ImpedimentoRepository.AllAsync(request.GetDependencies);

        if (impedimentos == null)
        {
            return new ListImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ImpedimentoDTO> result = _mapper.Map<IEnumerable<ImpedimentoDTO>>(impedimentos);

        return new ListImpedimentoViewModel { Impedimentos = result, OperationResult = OperationResult.Success };
    }
}
