using Cpnucleo.Shared.Commands.CreateWorkflow;
using Cpnucleo.Shared.Commands.RemoveWorkflow;
using Cpnucleo.Shared.Commands.UpdateWorkflow;
using Cpnucleo.Shared.Queries.GetWorkflow;
using Cpnucleo.Shared.Queries.ListWorkflow;

namespace Cpnucleo.API.Controllers.V2;

[Authorize]
[ApiController]
[ApiVersion("2")]
[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
public class WorkflowController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkflowController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Listar workflows
    /// </summary>
    /// <remarks>
    /// # Listar workflows
    /// 
    /// Lista workflows da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpPost]
    [Route("ListWorkflow")]
    public async Task<ActionResult<ListWorkflowViewModel>> ListWorkflow([FromBody] ListWorkflowQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar workflow
    /// </summary>
    /// <remarks>
    /// # Consultar workflow
    /// 
    /// Consulta um workflow na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    [HttpPost]
    [Route("GetWorkflow")]
    public async Task<ActionResult<GetWorkflowViewModel>> GetWorkflow([FromBody] GetWorkflowQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Incluir workflow
    /// </summary>
    /// <remarks>
    /// # Incluir workflow
    /// 
    /// Inclui um workflow na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("CreateWorkflow")]
    public async Task<ActionResult<OperationResult>> CreateWorkflow([FromBody] CreateWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar workflow
    /// </summary>
    /// <remarks>
    /// # Alterar workflow
    /// 
    /// Altera um workflow na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("UpdateWorkflow")]
    public async Task<ActionResult<OperationResult>> UpdateWorkflow([FromBody] UpdateWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Remover workflow
    /// </summary>
    /// <remarks>
    /// # Remover workflow
    /// 
    /// Remove um workflow da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    [HttpPost]
    [Route("RemoveWorkflow")]
    public async Task<ActionResult<OperationResult>> RemoveWorkflow([FromBody] RemoveWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }
}
