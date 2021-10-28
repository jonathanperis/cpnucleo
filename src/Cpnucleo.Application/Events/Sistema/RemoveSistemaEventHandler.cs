using Cpnucleo.Infra.CrossCutting.Util.Events.Sistema;
using Ev.ServiceBus.Reception;

namespace Cpnucleo.Application.Events.Sistema;

public class RemoveSistemaEventHandler : IMessageReceptionHandler<RemoveSistemaEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RemoveSistemaEventHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(RemoveSistemaEvent @event, CancellationToken cancellationToken)
    {
        //Some business logic here.
        IEnumerable<SistemaViewModel> sistemas = _mapper.Map<IEnumerable<SistemaViewModel>>(await _unitOfWork.SistemaRepository.AllAsync(true));
    }
}
