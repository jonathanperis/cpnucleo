using Cpnucleo.Shared.Commands.ImpedimentoTarefa;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.ImpedimentoTarefa;

namespace Cpnucleo.API.Controllers.V3;

[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ImpedimentoTarefaController : ControllerBase
{
    private readonly IMediator _mediator;

    public ImpedimentoTarefaController(IMediator mediator)
    {
        _mediator = mediator;
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
    [HttpGet]
    [Route("ListImpedimentoTarefa")]
    public async Task<ActionResult<ListImpedimentoTarefaViewModel>> ListImpedimentoTarefa([FromQuery] ListImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
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
    [HttpGet]
    [Route("GetImpedimentoTarefa")]
    public async Task<ActionResult<GetImpedimentoTarefaViewModel>> GetImpedimentoTarefa([FromQuery] GetImpedimentoTarefaQuery query)
    {
        return await _mediator.Send(query);
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
    [HttpGet]
    [Route("GetImpedimentoTarefaByTarefa")]
    public async Task<ActionResult<GetImpedimentoTarefaByTarefaViewModel>> GetImpedimentoTarefaByTarefa([FromQuery] GetImpedimentoTarefaByTarefaQuery query)
    {
        return await _mediator.Send(query);
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
    [HttpPost]
    [Route("CreateImpedimentoTarefa")]
    public async Task<ActionResult<OperationResult>> CreateImpedimentoTarefa([FromBody] CreateImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
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
    [HttpPost]
    [Route("UpdateImpedimentoTarefa")]
    public async Task<ActionResult<OperationResult>> UpdateImpedimentoTarefa([FromBody] UpdateImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
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
    [HttpPost]
    [Route("RemoveImpedimentoTarefa")]
    public async Task<ActionResult<OperationResult>> RemoveImpedimentoTarefa([FromBody] RemoveImpedimentoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
