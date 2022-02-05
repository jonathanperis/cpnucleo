using Cpnucleo.Application;
using Cpnucleo.Application.Commands.Sistema.CreateSistema;
using Cpnucleo.Application.Commands.Sistema.RemoveSistema;
using Cpnucleo.Application.Commands.Sistema.UpdateSistema;
using Cpnucleo.Application.Queries.Sistema.GetSistema;
using Cpnucleo.Application.Queries.Sistema.ListSistema;

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
    [Route("PostSistema")]
    public async Task<ActionResult<OperationResult>> PostSistema([FromBody] CreateSistemaCommand command)
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
    [HttpPut]
    [Route("PutSistema")]
    public async Task<ActionResult<OperationResult>> PutSistema([FromBody] UpdateSistemaCommand command)
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
    [HttpDelete]
    [Route("DeleteSistema")]
    public async Task<ActionResult<OperationResult>> DeleteSistema([FromBody] RemoveSistemaCommand command)
    {
        return await _mediator.Send(command);
    }
}
