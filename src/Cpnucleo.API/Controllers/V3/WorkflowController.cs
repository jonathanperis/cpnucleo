using Cpnucleo.Application;
using Cpnucleo.Application.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Application.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Application.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Application.Queries.Workflow.GetWorkflow;
using Cpnucleo.Application.Queries.Workflow.ListWorkflow;

namespace Cpnucleo.API.Controllers.V3;

//[Authorize]
[ApiController]
[ApiVersion("3")]
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
    [HttpGet]
    [Route("ListWorkflow")]
    public async Task<ActionResult<ListWorkflowViewModel>> ListWorkflow([FromQuery] ListWorkflowQuery query)
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
    [HttpGet]
    [Route("GetWorkflow")]
    public async Task<ActionResult<GetWorkflowViewModel>> GetWorkflow([FromQuery] GetWorkflowQuery query)
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
    [Route("PostWorkflow")]
    public async Task<ActionResult<OperationResult>> PostWorkflow([FromBody] CreateWorkflowCommand command)
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
    [HttpPut]
    [Route("PutWorkflow")]
    public async Task<ActionResult<OperationResult>> PutWorkflow([FromBody] UpdateWorkflowCommand command)
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
    [HttpDelete]
    [Route("DeleteWorkflow")]
    public async Task<ActionResult<OperationResult>> DeleteWorkflow([FromBody] RemoveWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }
}
