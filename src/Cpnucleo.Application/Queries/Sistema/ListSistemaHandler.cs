using Cpnucleo.Application.Hubs;
using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Sistema;
using Microsoft.AspNetCore.SignalR;

namespace Cpnucleo.Application.Queries.Sistema;

public class ListSistemaHandler : IRequestHandler<ListSistemaQuery, ListSistemaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHubContext<CpnucleoHub> _hubContext;

    public ListSistemaHandler(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<CpnucleoHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _hubContext = hubContext;
    }

    public async Task<ListSistemaViewModel> Handle(ListSistemaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Sistema> sistemas = await _unitOfWork.SistemaRepository.AllAsync(request.GetDependencies);

        if (sistemas == null)
        {
            return new ListSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<SistemaDTO> result = _mapper.Map<IEnumerable<SistemaDTO>>(sistemas);

        await _hubContext.Clients.All.SendAsync("broadcastMessage", "Broadcast: Test Message.", "Lorem ipsum dolor sit amet", cancellationToken);

        return new ListSistemaViewModel { Sistemas = result, OperationResult = OperationResult.Success };
    }
}
