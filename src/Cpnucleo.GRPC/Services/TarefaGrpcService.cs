namespace Cpnucleo.GRPC.Services;

[Authorize]
public class TarefaGrpcService : ServiceBase<ITarefaGrpcService>, ITarefaGrpcService
{
    private readonly IMediator _mediator;

    public TarefaGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Incluir tarefa
    /// </summary>
    /// <remarks>
    /// # Incluir tarefa
    /// 
    /// Inclui um tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> CreateTarefa(CreateTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Listar tarefas
    /// </summary>
    /// <remarks>
    /// # Listar tarefas
    /// 
    /// Lista tarefas da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListTarefaViewModel> ListTarefa(ListTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar tarefa
    /// </summary>
    /// <remarks>
    /// # Consultar tarefa
    /// 
    /// Consulta um tarefa na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetTarefaViewModel> GetTarefa(GetTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar tarefa por recurso
    /// </summary>
    /// <remarks>
    /// # Consultar tarefa por recurso
    /// 
    /// Consulta um tarefa por recurso na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetTarefaByRecursoViewModel> GetTarefaByRecurso(GetTarefaByRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Remover tarefa
    /// </summary>
    /// <remarks>
    /// # Remover tarefa
    /// 
    /// Remove um tarefa da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> RemoveTarefa(RemoveTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar tarefa
    /// </summary>
    /// <remarks>
    /// # Alterar tarefa
    /// 
    /// Altera um tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateTarefa(UpdateTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar workflow da tarefa
    /// </summary>
    /// <remarks>
    /// # Alterar workflow da tarefa
    /// 
    /// Altera um workflow da tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateTarefaByWorkflow(UpdateTarefaByWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }
}
