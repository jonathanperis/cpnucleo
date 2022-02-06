using Cpnucleo.Application;
using Cpnucleo.Application.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Application.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RecursoTarefaController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecursoTarefaController(IMediator mediator)
    {
        _mediator = mediator;
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
    [HttpGet]
    [Route("ListRecursoTarefa")]
    public async Task<ActionResult<ListRecursoTarefaViewModel>> ListRecursoTarefa([FromQuery] ListRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
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
    [HttpGet]
    [Route("GetRecursoTarefa")]
    public async Task<ActionResult<GetRecursoTarefaViewModel>> GetRecursoTarefa([FromQuery] GetRecursoTarefaQuery query)
    {
        return await _mediator.Send(query);
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
    [HttpGet]
    [Route("GetRecursoTarefaByTarefa")]
    public async Task<ActionResult<GetRecursoTarefaByTarefaViewModel>> GetRecursoTarefaByTarefa([FromQuery] GetRecursoTarefaByTarefaQuery query)
    {
        return await _mediator.Send(query);
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
        return await _mediator.Send(command);
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
    [HttpPut]
    [Route("UpdateRecursoTarefa")]
    public async Task<ActionResult<OperationResult>> UpdateRecursoTarefa([FromBody] UpdateRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
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
    [HttpDelete]
    [Route("RemoveRecursoTarefa")]
    public async Task<ActionResult<OperationResult>> RemoveRecursoTarefa([FromBody] RemoveRecursoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
