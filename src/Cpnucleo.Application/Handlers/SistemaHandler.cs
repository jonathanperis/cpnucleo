﻿using Cpnucleo.Application.Hubs;
using Cpnucleo.Infra.CrossCutting.Bus.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Events.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;
using Microsoft.AspNetCore.SignalR;

namespace Cpnucleo.Application.Handlers;

public class SistemaHandler :
    IRequestHandler<CreateSistemaCommand, OperationResult>,
    IRequestHandler<GetSistemaQuery, SistemaViewModel>,
    IRequestHandler<ListSistemaQuery, IEnumerable<SistemaViewModel>>,
    IRequestHandler<RemoveSistemaCommand, OperationResult>,
    IRequestHandler<UpdateSistemaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IEventHandler _eventHandler;
    private readonly IHubContext<CpnucleoHub> _hubContext;

    public SistemaHandler(IUnitOfWork unitOfWork, IMapper mapper, IEventHandler eventHandler, IHubContext<CpnucleoHub> hubContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _eventHandler = eventHandler;
        _hubContext = hubContext;
    }

    public async Task<OperationResult> Handle(CreateSistemaCommand request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.SistemaRepository.AddAsync(_mapper.Map<Sistema>(request.Sistema));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<SistemaViewModel> Handle(GetSistemaQuery request, CancellationToken cancellationToken = default)
    {
        SistemaViewModel result = _mapper.Map<SistemaViewModel>(await _unitOfWork.SistemaRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<SistemaViewModel>> Handle(ListSistemaQuery request, CancellationToken cancellationToken = default)
    {
        IEnumerable<SistemaViewModel> result = _mapper.Map<IEnumerable<SistemaViewModel>>(await _unitOfWork.SistemaRepository.AllAsync(request.GetDependencies));

        await _hubContext.Clients.All.SendAsync("broadcastMessage", "Broadcast: Test Message.", "Lorem ipsum dolor sit amet", cancellationToken);

        return result;
    }

    public async Task<OperationResult> Handle(RemoveSistemaCommand request, CancellationToken cancellationToken = default)
    {
        Sistema obj = await _unitOfWork.SistemaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.SistemaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        if (result == OperationResult.Success)
        {
            await _eventHandler.PublishEventAsync(new RemoveSistemaEvent { Id = request.Id });
        }

        return result;
    }

    public async Task<OperationResult> Handle(UpdateSistemaCommand request, CancellationToken cancellationToken = default)
    {
        _unitOfWork.SistemaRepository.Update(_mapper.Map<Sistema>(request.Sistema));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
