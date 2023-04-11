namespace Cpnucleo.API.Controllers.V2;

[Authorize]
[ApiController]
[ApiVersion("2")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RecursoTarefaController : ControllerBase
{
    private readonly ISender _sender;

    public RecursoTarefaController(ISender sender)
    {
        _sender = sender;
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
    [HttpPost]
    [Route("ListRecursoTarefa")]
    public async Task<ActionResult<ListRecursoTarefaViewModel>> ListRecursoTarefa([FromBody] ListRecursoTarefaQuery query)
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
    [HttpPost]
    [Route("GetRecursoTarefa")]
    public async Task<ActionResult<GetRecursoTarefaViewModel>> GetRecursoTarefa([FromBody] GetRecursoTarefaQuery query)
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
    [HttpPost]
    [Route("GetRecursoTarefaByTarefa")]
    public async Task<ActionResult<ListRecursoTarefaByTarefaViewModel>> GetRecursoTarefaByTarefa([FromBody] ListRecursoTarefaByTarefaQuery query)
    {
        return await _sender.Send(query);
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
    [HttpPost]
    [Route("CreateRecursoTarefa")]
    public async Task<ActionResult<OperationResult>> CreateRecursoTarefa([FromBody] CreateRecursoTarefaCommand command)
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
    [HttpPost]
    [Route("UpdateRecursoTarefa")]
    public async Task<ActionResult<OperationResult>> UpdateRecursoTarefa([FromBody] UpdateRecursoTarefaCommand command)
    {
        return await _sender.Send(command);
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
    [HttpPost]
    [Route("RemoveRecursoTarefa")]
    public async Task<ActionResult<OperationResult>> RemoveRecursoTarefa([FromBody] RemoveRecursoTarefaCommand command)
    {
        return await _sender.Send(command);
    }
}
