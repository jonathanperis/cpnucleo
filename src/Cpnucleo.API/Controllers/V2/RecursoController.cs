namespace Cpnucleo.API.Controllers.V2;

[Authorize]
[ApiController]
[ApiVersion("2")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RecursoController : ControllerBase
{
    private readonly ISender _sender;

    public RecursoController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Listar recursos
    /// </summary>
    /// <remarks>
    /// # Listar recursos
    /// 
    /// Lista recursos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpPost]
    [Route("ListRecurso")]
    public async Task<ActionResult<ListRecursoViewModel>> ListRecurso([FromBody] ListRecursoQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Consultar recurso
    /// </summary>
    /// <remarks>
    /// # Consultar recurso
    /// 
    /// Consulta um recurso na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpPost]
    [Route("GetRecurso")]
    public async Task<ActionResult<GetRecursoViewModel>> GetRecurso([FromBody] GetRecursoQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Incluir recurso
    /// </summary>
    /// <remarks>
    /// # Incluir recurso
    /// 
    /// Inclui um recurso na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("CreateRecurso")]
    public async Task<ActionResult<OperationResult>> CreateRecurso([FromBody] CreateRecursoCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Alterar recurso
    /// </summary>
    /// <remarks>
    /// # Alterar recurso
    /// 
    /// Altera um recurso na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("UpdateRecurso")]
    public async Task<ActionResult<OperationResult>> UpdateRecurso([FromBody] UpdateRecursoCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Remover recurso
    /// </summary>
    /// <remarks>
    /// # Remover recurso
    /// 
    /// Remove um recurso da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("RemoveRecurso")]
    public async Task<ActionResult<OperationResult>> RemoveRecurso([FromBody] RemoveRecursoCommand command)
    {
        return await _sender.Send(command);
    }
}
