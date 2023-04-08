﻿using Cpnucleo.Shared.Commands.CreateWorkflow;
using Cpnucleo.Shared.Commands.RemoveWorkflow;
using Cpnucleo.Shared.Commands.UpdateWorkflow;
using Cpnucleo.Shared.Queries.GetWorkflow;
using Cpnucleo.Shared.Queries.ListWorkflow;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class WorkflowGrpcService : ServiceBase<IWorkflowGrpcService>, IWorkflowGrpcService
{
    private readonly IMediator _mediator;

    public WorkflowGrpcService(IMediator mediator)
    {
        _mediator = mediator;
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
    public async UnaryResult<OperationResult> CreateWorkflow(CreateWorkflowCommand command)
    {
        return await _mediator.Send(command);
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
    public async UnaryResult<ListWorkflowViewModel> ListWorkflow(ListWorkflowQuery query)
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
    public async UnaryResult<GetWorkflowViewModel> GetWorkflow(GetWorkflowQuery query)
    {
        return await _mediator.Send(query);
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
    public async UnaryResult<OperationResult> RemoveWorkflow(RemoveWorkflowCommand command)
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
    public async UnaryResult<OperationResult> UpdateWorkflow(UpdateWorkflowCommand command)
    {
        return await _mediator.Send(command);
    }
}
