using Cpnucleo.Application;
using Cpnucleo.Application.Commands.Projeto.CreateProjeto;
using Cpnucleo.Application.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Application.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Application.Queries.Projeto.GetProjeto;
using Cpnucleo.Application.Queries.Projeto.ListProjeto;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProjetoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjetoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar projetos
    /// </summary>
    /// <remarks>
    /// # Listar projetos
    /// 
    /// Lista projetos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("ListProjeto")]
    public async Task<ActionResult<ListProjetoViewModel>> ListProjeto([FromQuery] ListProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar projeto
    /// </summary>
    /// <remarks>
    /// # Consultar projeto
    /// 
    /// Consulta um projeto na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpGet]
    [Route("GetProjeto")]
    public async Task<ActionResult<GetProjetoViewModel>> GetProjeto([FromQuery] GetProjetoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Incluir projeto
    /// </summary>
    /// <remarks>
    /// # Incluir projeto
    /// 
    /// Inclui um projeto na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("CreateProjeto")]
    public async Task<ActionResult<OperationResult>> CreateProjeto([FromBody] CreateProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar projeto
    /// </summary>
    /// <remarks>
    /// # Alterar projeto
    /// 
    /// Altera um projeto na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPut]
    [Route("UpdateProjeto")]
    public async Task<ActionResult<OperationResult>> UpdateProjeto([FromBody] UpdateProjetoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Remover projeto
    /// </summary>
    /// <remarks>
    /// # Remover projeto
    /// 
    /// Remove um projeto da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpDelete]
    [Route("RemoveProjeto")]
    public async Task<ActionResult<OperationResult>> RemoveProjeto([FromBody] RemoveProjetoCommand command)
    {
        return await _mediator.Send(command);
    }
}
