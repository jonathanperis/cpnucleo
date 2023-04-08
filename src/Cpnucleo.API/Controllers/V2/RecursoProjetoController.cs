using Cpnucleo.Shared.Commands.CreateRecursoProjeto;
using Cpnucleo.Shared.Commands.RemoveRecursoProjeto;
using Cpnucleo.Shared.Commands.UpdateRecursoProjeto;
using Cpnucleo.Shared.Queries.GetRecursoProjeto;
using Cpnucleo.Shared.Queries.ListRecursoProjeto;
using Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

namespace Cpnucleo.API.Controllers.V2;

[Authorize]
[ApiController]
[ApiVersion("2")]
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
    [HttpPost]
    [Route("ListRecursoProjeto")]
    public async Task<ActionResult<ListRecursoProjetoViewModel>> ListRecursoProjeto([FromBody] ListRecursoProjetoQuery query)
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
    [HttpPost]
    [Route("GetRecursoProjeto")]
    public async Task<ActionResult<GetRecursoProjetoViewModel>> GetRecursoProjeto([FromBody] GetRecursoProjetoQuery query)
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
    [HttpPost]
    [Route("GetRecursoProjetoByProjeto")]
    public async Task<ActionResult<ListRecursoProjetoByProjetoViewModel>> GetRecursoProjetoByProjeto([FromBody] ListRecursoProjetoByProjetoQuery query)
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
    [Route("CreateRecursoProjeto")]
    public async Task<ActionResult<OperationResult>> CreateRecursoProjeto([FromBody] CreateRecursoProjetoCommand command)
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
    [HttpPost]
    [Route("UpdateRecursoProjeto")]
    public async Task<ActionResult<OperationResult>> UpdateRecursoProjeto([FromBody] UpdateRecursoProjetoCommand command)
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
    [HttpPost]
    [Route("RemoveRecursoProjeto")]
    public async Task<ActionResult<OperationResult>> RemoveRecursoProjeto([FromBody] RemoveRecursoProjetoCommand command)
    {
        return await _mediator.Send(command);
    }
}
