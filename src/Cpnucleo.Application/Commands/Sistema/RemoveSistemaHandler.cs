using Cpnucleo.Application.Common.Bus.Interfaces;
using Cpnucleo.Shared.Events.Sistema;

namespace Cpnucleo.Application.Commands.Sistema;

public sealed class RemoveSistemaHandler : IRequestHandler<RemoveSistemaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEventHandler _eventHandler;

    public RemoveSistemaHandler(IUnitOfWork unitOfWork, IEventHandler eventHandler)
    {
        _unitOfWork = unitOfWork;
        _eventHandler = eventHandler;
    }

    public async Task<OperationResult> Handle(RemoveSistemaCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Sistema sistema = await _unitOfWork.SistemaRepository.GetAsync(request.Id);

        if (sistema == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.SistemaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        if (result == OperationResult.Success)
        {
            await _eventHandler.PublishEventAsync(new RemoveSistemaEvent(request.Id));
        }

        return result;
    }
}
