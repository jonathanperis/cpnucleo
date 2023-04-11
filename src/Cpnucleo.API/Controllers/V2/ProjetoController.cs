namespace Cpnucleo.API.Controllers.V2;

[Authorize]
[ApiController]
[ApiVersion("2")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProjetoController : ControllerBase
{
    private readonly ISender _sender;

    public ProjetoController(ISender sender)
    {
        _sender = sender;
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
    [HttpPost]
    [Route("ListProjeto")]
    public async Task<ActionResult<ListProjetoViewModel>> ListProjeto([FromBody] ListProjetoQuery query)
    {
        return await _sender.Send(query);
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
    [HttpPost]
    [Route("GetProjeto")]
    public async Task<ActionResult<GetProjetoViewModel>> GetProjeto([FromBody] GetProjetoQuery query)
    {
        return await _sender.Send(query);
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
        return await _sender.Send(command);
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
    [HttpPost]
    [Route("UpdateProjeto")]
    public async Task<ActionResult<OperationResult>> UpdateProjeto([FromBody] UpdateProjetoCommand command)
    {
        return await _sender.Send(command);
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
    [HttpPost]
    [Route("RemoveProjeto")]
    public async Task<ActionResult<OperationResult>> RemoveProjeto([FromBody] RemoveProjetoCommand command)
    {
        return await _sender.Send(command);
    }
}
