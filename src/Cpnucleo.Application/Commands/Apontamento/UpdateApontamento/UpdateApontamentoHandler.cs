namespace Cpnucleo.Application.Commands.Apontamento.UpdateApontamento;

public class UpdateApontamentoHandler : IRequestHandler<UpdateApontamentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateApontamentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateApontamentoCommand request, CancellationToken cancellationToken)
    {
        var apontamento = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

        if (apontamento == null)
        {
            return OperationResult.NotFound;
        }

        apontamento.Descricao = request.Descricao;
        apontamento.DataApontamento = request.DataApontamento;
        apontamento.QtdHoras = request.QtdHoras;
        apontamento.Ativo = request.Ativo;
        apontamento.IdTarefa = request.IdTarefa;
        apontamento.IdRecurso = request.IdRecurso;

        _unitOfWork.ApontamentoRepository.Update(apontamento);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
