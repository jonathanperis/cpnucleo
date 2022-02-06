using Cpnucleo.Application;
using Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Application.Queries.TipoTarefa.ListTipoTarefa;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TipoTarefaController : ControllerBase
{
    private readonly IMediator _mediator;

    public TipoTarefaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar tipos de tarefa
    /// </summary>
    /// <remarks>
    /// # Listar tipos de tarefa
    /// 
    /// Lista tipos de tarefa da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("ListTipoTarefa")]
    public async Task<ActionResult<ListTipoTarefaViewModel>> ListTipoTarefa([FromQuery] ListTipoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar tipo de tarefa
    /// </summary>
    /// <remarks>
    /// # Consultar tipo de tarefa
    /// 
    /// Consulta um tipo de tarefa na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("GetTipoTarefa")]
    public async Task<ActionResult<GetTipoTarefaViewModel>> GetTipoTarefa([FromQuery] GetTipoTarefaQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Incluir tipo de tarefa
    /// </summary>
    /// <remarks>
    /// # Incluir tipo de tarefa
    /// 
    /// Inclui um tipo de tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("PostTipoTarefa")]
    public async Task<ActionResult<OperationResult>> PostTipoTarefa([FromBody] CreateTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar tipo de tarefa
    /// </summary>
    /// <remarks>
    /// # Alterar tipo de tarefa
    /// 
    /// Altera um tipo de tarefa na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPut]
    [Route("PutTipoTarefa")]
    public async Task<ActionResult<OperationResult>> PutTipoTarefa([FromBody] UpdateTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Remover tipo de tarefa
    /// </summary>
    /// <remarks>
    /// # Remover tipo de tarefa
    /// 
    /// Remove um tipo de tarefa da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpDelete]
    [Route("DeleteTipoTarefa")]
    public async Task<ActionResult<OperationResult>> DeleteTipoTarefa([FromBody] RemoveTipoTarefaCommand command)
    {
        return await _mediator.Send(command);
    }
}
