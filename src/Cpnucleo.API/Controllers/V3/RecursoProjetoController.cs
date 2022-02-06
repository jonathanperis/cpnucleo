using Cpnucleo.Application;
using Cpnucleo.Application.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto.ListRecursoProjeto;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RecursoProjetoController : ControllerBase
{
    private readonly IMediator _mediator;

    public RecursoProjetoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar recursos de projetos
    /// </summary>
    /// <remarks>
    /// # Listar recursos de projetos
    /// 
    /// Lista recursos de projetos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("ListRecursoProjeto")]
    public async Task<ActionResult<ListRecursoProjetoViewModel>> ListRecursoProjeto([FromQuery] ListRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar recurso de projeto
    /// </summary>
    /// <remarks>
    /// # Consultar recurso de projeto
    /// 
    /// Consulta um recurso de projeto na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("GetRecursoProjeto")]
    public async Task<ActionResult<GetRecursoProjetoViewModel>> GetRecursoProjeto([FromQuery] GetRecursoProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar recurso de projeto por projeto
    /// </summary>
    /// <remarks>
    /// # Consultar recurso de projeto por projeto
    /// 
    /// Consulta um recurso de projeto por projeto na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("GetByProjeto")]
    public async Task<ActionResult<GetByProjetoViewModel>> GetByProjeto([FromQuery] GetByProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Incluir recurso de projeto
    /// </summary>
    /// <remarks>
    /// # Incluir recurso de projeto
    /// 
    /// Inclui um recurso de projeto na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("PostRecursoProjeto")]
    public async Task<ActionResult<OperationResult>> PostRecursoProjeto([FromBody] CreateRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar recurso de projeto
    /// </summary>
    /// <remarks>
    /// # Alterar recurso de projeto
    /// 
    /// Altera um recurso de projeto na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPut]
    [Route("PutRecursoProjeto")]
    public async Task<ActionResult<OperationResult>> PutRecursoProjeto([FromBody] UpdateRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Remover recurso de projeto
    /// </summary>
    /// <remarks>
    /// # Remover recurso de projeto
    /// 
    /// Remove um recurso de projeto da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpDelete]
    [Route("DeleteRecursoProjeto")]
    public async Task<ActionResult<OperationResult>> DeleteRecursoProjeto([FromBody] RemoveRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }
}
