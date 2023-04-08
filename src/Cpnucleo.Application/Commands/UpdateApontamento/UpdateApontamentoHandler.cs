using Cpnucleo.Shared.Commands.UpdateApontamento;

namespace Cpnucleo.Application.Commands.UpdateApontamento;

public sealed class UpdateApontamentoHandler : IRequestHandler<UpdateApontamentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateApontamentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateApontamentoCommand request, CancellationToken cancellationToken)
    {
        Apontamento apontamento = await _unitOfWork.ApontamentoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (apontamento is null)
        {
            return OperationResult.NotFound;
        }

        apontamento.Descricao = request.Descricao;
        apontamento.DataApontamento = request.DataApontamento;
        apontamento.QtdHoras = request.QtdHoras;
        apontamento.IdTarefa = request.IdTarefa;
        apontamento.IdRecurso = request.IdRecurso;

        _unitOfWork.ApontamentoRepository.Update(apontamento);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
