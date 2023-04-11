namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ImpedimentoTarefaGrpcService : ServiceBase<IImpedimentoTarefaGrpcService>, IImpedimentoTarefaGrpcService
{
    private readonly ISender _sender;

    public ImpedimentoTarefaGrpcService(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Incluir impedimento de tarefa
    /// </summary>
    /// <remarks>
    /// # Incluir impedimento de tarefa
    /// 
    /// Inclui um impedimento de tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> CreateImpedimentoTarefa(CreateImpedimentoTarefaCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Listar impedimentos de tarefas
    /// </summary>
    /// <remarks>
    /// # Listar impedimentos de tarefas
    /// 
    /// Lista impedimentos de tarefas da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListImpedimentoTarefaViewModel> ListImpedimentoTarefa(ListImpedimentoTarefaQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Consultar impedimento de tarefa
    /// </summary>
    /// <remarks>
    /// # Consultar impedimento de tarefa
    /// 
    /// Consulta um impedimento de tarefa na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetImpedimentoTarefaViewModel> GetImpedimentoTarefa(GetImpedimentoTarefaQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Consultar impedimento de tarefa por tarefa
    /// </summary>
    /// <remarks>
    /// # Consultar impedimento de tarefa por tarefa
    /// 
    /// Consulta um impedimento de tarefa por tarefa na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListImpedimentoTarefaByTarefaViewModel> GetImpedimentoTarefaByTarefa(ListImpedimentoTarefaByTarefaQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Remover impedimento de tarefa
    /// </summary>
    /// <remarks>
    /// # Remover impedimento de tarefa
    /// 
    /// Remove um impedimento de tarefa da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> RemoveImpedimentoTarefa(RemoveImpedimentoTarefaCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Alterar impedimento de tarefa
    /// </summary>
    /// <remarks>
    /// # Alterar impedimento de tarefa
    /// 
    /// Altera um impedimento de tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateImpedimentoTarefa(UpdateImpedimentoTarefaCommand command)
    {
        return await _sender.Send(command);
    }
}
