using Microsoft.AspNetCore.SignalR;

namespace Cpnucleo.Application.Hubs;

public sealed class CpnucleoHub : Hub
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CpnucleoHub(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Echo(string name, string message)
    {
        //Some business logic here.
        IEnumerable<SistemaDTO> sistemas = _mapper.Map<IEnumerable<SistemaDTO>>(await _unitOfWork.SistemaRepository.AllAsync(true));

        await Clients.Client(Context.ConnectionId).SendAsync("echo", name, $"{message} (echo from server)");
    }
}
