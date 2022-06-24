using Cpnucleo.Shared.Commands.Recurso;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Recurso;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RecursoController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecursoController(IMediator mediator)
    {
        _mediator = mediator;
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
    [HttpGet]
    [Route("ListRecurso")]
    public async Task<ActionResult<ListRecursoViewModel>> ListRecurso([FromQuery] ListRecursoQuery query)
    {
        return await _mediator.Send(query);
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
    [HttpGet]
    [Route("GetRecurso")]
    public async Task<ActionResult<GetRecursoViewModel>> GetRecurso([FromQuery] GetRecursoQuery query)
    {
        return await _mediator.Send(query);
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
        return await _mediator.Send(command);
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
        return await _mediator.Send(command);
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
        return await _mediator.Send(command);
    }
}
