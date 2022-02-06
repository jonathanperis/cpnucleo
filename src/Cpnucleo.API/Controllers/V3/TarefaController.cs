using Cpnucleo.Application;
using Cpnucleo.Application.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Application.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Application.Commands.Tarefa.UpdateByWorkflow;
using Cpnucleo.Application.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Application.Queries.Tarefa.GetByRecurso;
using Cpnucleo.Application.Queries.Tarefa.GetTarefa;
using Cpnucleo.Application.Queries.Tarefa.ListTarefa;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TarefaController : ControllerBase
{
    private readonly IMediator _mediator;

    public TarefaController(IMediator mediator)
    {
        _mediator = mediator;
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
    [HttpGet]
    [Route("ListTarefa")]
    public async Task<ActionResult<ListTarefaViewModel>> ListTarefa([FromQuery] ListTarefaQuery query)
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
    [HttpGet]
    [Route("GetTarefa")]
    public async Task<ActionResult<GetTarefaViewModel>> GetTarefa([FromQuery] GetTarefaQuery query)
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
    [HttpGet]
    [Route("GetByRecurso")]
    public async Task<ActionResult<GetByRecursoViewModel>> GetByRecurso([FromQuery] GetByRecursoQuery query)
    {
        return await _mediator.Send(query);
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
    [HttpPost]
    [Route("PostTarefa")]
    public async Task<ActionResult<OperationResult>> PostTarefa([FromBody] CreateTarefaCommand command)
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
    [HttpPut]
    [Route("PutTarefa")]
    public async Task<ActionResult<OperationResult>> PutTarefa([FromBody] UpdateTarefaCommand command)
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
    [HttpPut]
    [Route("PutByWorkflow")]
    public async Task<ActionResult<OperationResult>> PutByWorkflowTarefa([FromBody] UpdateByWorkflowCommand command)
    {
        return await _mediator.Send(command);
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
    [HttpDelete]
    [Route("DeleteTarefa")]
    public async Task<ActionResult<OperationResult>> DeleteTarefa([FromBody] RemoveTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
