﻿using Cpnucleo.Shared.Events.Sistema;
using Ev.ServiceBus.Reception;

namespace Cpnucleo.Application.Events.Sistema;

public sealed class RemoveSistemaHandler : IMessageReceptionHandler<RemoveSistemaEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RemoveSistemaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(RemoveSistemaEvent @event, CancellationToken cancellationToken)
    {
        //Some business logic here.
        List<SistemaDTO> sistemas = _mapper.Map<List<SistemaDTO>>(await _unitOfWork.SistemaRepository.List(true).ToListAsync());
    }
}
