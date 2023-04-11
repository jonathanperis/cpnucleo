namespace Cpnucleo.API.Controllers.V2;

[Authorize]
[ApiController]
[ApiVersion("2")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TipoTarefaController : ControllerBase
{
    private readonly ISender _sender;

    public TipoTarefaController(ISender sender)
    {
        _sender = sender;
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
    [HttpPost]
    [Route("ListTipoTarefa")]
    public async Task<ActionResult<ListTipoTarefaViewModel>> ListTipoTarefa([FromBody] ListTipoTarefaQuery query)
    {
        return await _sender.Send(query);
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
    [HttpPost]
    [Route("GetTipoTarefa")]
    public async Task<ActionResult<GetTipoTarefaViewModel>> GetTipoTarefa([FromBody] GetTipoTarefaQuery query)
    {
        return await _sender.Send(query);
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
    [Route("CreateTipoTarefa")]
    public async Task<ActionResult<OperationResult>> CreateTipoTarefa([FromBody] CreateTipoTarefaCommand command)
    {
        return await _sender.Send(command);
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
    [HttpPost]
    [Route("UpdateTipoTarefa")]
    public async Task<ActionResult<OperationResult>> UpdateTipoTarefa([FromBody] UpdateTipoTarefaCommand command)
    {
        return await _sender.Send(command);
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
    [HttpPost]
    [Route("RemoveTipoTarefa")]
    public async Task<ActionResult<OperationResult>> RemoveTipoTarefa([FromBody] RemoveTipoTarefaCommand command)
    {
        return await _sender.Send(command);
    }
}
