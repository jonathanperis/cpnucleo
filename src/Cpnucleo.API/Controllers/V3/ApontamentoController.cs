using Cpnucleo.Application;
using Cpnucleo.Application.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Application.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Application.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetApontamento;
using Cpnucleo.Application.Queries.Apontamento.GetApontamentoByRecurso;
using Cpnucleo.Application.Queries.Apontamento.ListApontamento;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApontamentoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApontamentoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar apontamentos
    /// </summary>
    /// <remarks>
    /// # Listar apontamentos
    /// 
    /// Lista apontamentos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("ListApontamento")]
    public async Task<ActionResult<ListApontamentoViewModel>> ListApontamento([FromQuery] ListApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar apontamento
    /// </summary>
    /// <remarks>
    /// # Consultar apontamento
    /// 
    /// Consulta um apontamento na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("GetApontamento")]
    public async Task<ActionResult<GetApontamentoViewModel>> GetApontamento([FromQuery] GetApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar apontamento por recurso
    /// </summary>
    /// <remarks>
    /// # Consultar apontamento por recurso
    /// 
    /// Consulta um apontamento por recurso na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("GetApontamentoByRecurso")]
    public async Task<ActionResult<GetApontamentoByRecursoViewModel>> GetApontamentoByRecurso([FromQuery] GetApontamentoByRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Incluir apontamento
    /// </summary>
    /// <remarks>
    /// # Incluir apontamento
    /// 
    /// Inclui um apontamento na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("PostApontamento")]
    public async Task<ActionResult<OperationResult>> PostApontamento([FromBody] CreateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar apontamento
    /// </summary>
    /// <remarks>
    /// # Alterar apontamento
    /// 
    /// Altera um apontamento na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPut]
    [Route("PutApontamento")]
    public async Task<ActionResult<OperationResult>> PutApontamento([FromBody] UpdateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Remover apontamento
    /// </summary>
    /// <remarks>
    /// # Remover apontamento
    /// 
    /// Remove um apontamento da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpDelete]
    [Route("DeleteApontamento")]
    public async Task<ActionResult<OperationResult>> DeleteApontamento([FromBody] RemoveApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }
}
