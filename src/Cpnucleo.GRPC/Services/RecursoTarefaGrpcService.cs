namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoTarefaGrpcService : ServiceBase<IRecursoTarefaGrpcService>, IRecursoTarefaGrpcService
{
    private readonly ISender _sender;

    public RecursoTarefaGrpcService(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Incluir recurso de tarefa
    /// </summary>
    /// <remarks>
    /// # Incluir recurso de tarefa
    /// 
    /// Inclui um recurso de tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> CreateRecursoTarefa(CreateRecursoTarefaCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Listar recurso de tarefas
    /// </summary>
    /// <remarks>
    /// # Listar recurso de tarefas
    /// 
    /// Lista recurso de tarefas da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListRecursoTarefaViewModel> ListRecursoTarefa(ListRecursoTarefaQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Consultar recurso de tarefa
    /// </summary>
    /// <remarks>
    /// # Consultar recurso de tarefa
    /// 
    /// Consulta um recurso de tarefa na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetRecursoTarefaViewModel> GetRecursoTarefa(GetRecursoTarefaQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Consultar recurso de tarefa por tarefa
    /// </summary>
    /// <remarks>
    /// # Consultar recurso de tarefa por tarefa
    /// 
    /// Consulta um recurso de tarefa por tarefa na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListRecursoTarefaByTarefaViewModel> GetRecursoTarefaByTarefa(ListRecursoTarefaByTarefaQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Remover recurso de tarefa
    /// </summary>
    /// <remarks>
    /// # Remover recurso de tarefa
    /// 
    /// Remove um recurso de tarefa da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> RemoveRecursoTarefa(RemoveRecursoTarefaCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Alterar recurso de tarefa
    /// </summary>
    /// <remarks>
    /// # Alterar recurso de tarefa
    /// 
    /// Altera um recurso de tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateRecursoTarefa(UpdateRecursoTarefaCommand command)
    {
        return await _sender.Send(command);
    }
}
