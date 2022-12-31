using Cpnucleo.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Cpnucleo.Application.Queries.Sistema;

public sealed class ListSistemaHandler : IRequestHandler<ListSistemaQuery, ListSistemaViewModel>
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
        List<SistemaDTO> sistemas = await _unitOfWork.SistemaRepository.List(request.GetDependencies)
            .ProjectTo<SistemaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (sistemas is null)
        {
            return new ListSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        await _hubContext.Clients.All.SendAsync("broadcastMessage", "Broadcast: Test Message.", "Lorem ipsum dolor sit amet", cancellationToken);

        return new ListSistemaViewModel { Sistemas = sistemas, OperationResult = OperationResult.Success };
    }
}
