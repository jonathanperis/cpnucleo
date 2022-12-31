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
        List<SistemaDTO> sistemas = _mapper.Map<List<SistemaDTO>>(await _unitOfWork.SistemaRepository.All(true).ToListAsync());

        await Clients.Client(Context.ConnectionId).SendAsync("echo", name, $"{message} (echo from server)");
    }
}
