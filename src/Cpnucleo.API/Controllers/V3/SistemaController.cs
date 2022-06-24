using Cpnucleo.Shared.Commands.Sistema;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Sistema;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class SistemaController : ControllerBase
{
    private readonly IMediator _mediator;

    public SistemaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar sistemas
    /// </summary>
    /// <remarks>
    /// # Listar sistemas
    /// 
    /// Lista sistemas da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("ListSistema")]
    public async Task<ActionResult<ListSistemaViewModel>> ListSistema([FromQuery] ListSistemaQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar sistema
    /// </summary>
    /// <remarks>
    /// # Consultar sistema
    /// 
    /// Consulta um sistema na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("GetSistema")]
    public async Task<ActionResult<GetSistemaViewModel>> GetSistema([FromQuery] GetSistemaQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Incluir sistema
    /// </summary>
    /// <remarks>
    /// # Incluir sistema
    /// 
    /// Inclui um sistema na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("CreateSistema")]
    public async Task<ActionResult<OperationResult>> CreateSistema([FromBody] CreateSistemaCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar sistema
    /// </summary>
    /// <remarks>
    /// # Alterar sistema
    /// 
    /// Altera um sistema na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("UpdateSistema")]
    public async Task<ActionResult<OperationResult>> UpdateSistema([FromBody] UpdateSistemaCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Remover sistema
    /// </summary>
    /// <remarks>
    /// # Remover sistema
    /// 
    /// Remove um sistema da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("RemoveSistema")]
    public async Task<ActionResult<OperationResult>> RemoveSistema([FromBody] RemoveSistemaCommand command)
    {
        return await _mediator.Send(command);
    }
}
