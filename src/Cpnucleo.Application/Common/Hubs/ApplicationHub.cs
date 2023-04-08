using Cpnucleo.Application.Common.Context;
using Microsoft.AspNetCore.SignalR;

namespace Cpnucleo.Application.Common.Hubs;

public sealed class ApplicationHub : Hub
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ApplicationHub(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Echo(string name, string message)
    {
        //Some business logic here.
        var sistemas = await _context.Sistemas
            .Where(x => x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<SistemaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

        await Clients.Client(Context.ConnectionId).SendAsync("echo", name, $"{message} (echo from server)");
    }
}
