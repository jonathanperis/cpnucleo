namespace Cpnucleo.Infra.CrossCutting.Util.Common.Interfaces;

public interface IImpedimentoTarefaGrpcService : IService<IImpedimentoTarefaGrpcService>
{
    UnaryResult<OperationResult> CreateImpedimentoTarefa(CreateImpedimentoTarefaCommand command);

    UnaryResult<OperationResult> UpdateImpedimentoTarefa(UpdateImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaViewModel> GetImpedimentoTarefa(GetImpedimentoTarefaQuery query);

    UnaryResult<ListImpedimentoTarefaViewModel> ListImpedimentoTarefa(ListImpedimentoTarefaQuery query);

    UnaryResult<OperationResult> RemoveImpedimentoTarefa(RemoveImpedimentoTarefaCommand command);

    UnaryResult<GetImpedimentoTarefaByTarefaViewModel> GetImpedimentoTarefaByTarefa(GetImpedimentoTarefaByTarefaQuery query);
}