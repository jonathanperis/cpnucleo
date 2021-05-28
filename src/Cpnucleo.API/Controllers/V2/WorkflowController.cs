using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.API.Controllers.V2
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Workflows
        /// </summary>
        /// <remarks>
        /// # Listar Workflows
        /// 
        /// Lista Workflows da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Workflows</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListWorkflowResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListWorkflowQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Workflow
        /// </summary>
        /// <remarks>
        /// # Consultar Workflow
        /// 
        /// Consulta um Workflow na base de dados.
        /// </remarks>
        /// <param name="id">Id do Workflow</param>        
        /// <response code="200">Retorna um Workflow</response>
        /// <response code="404">Workflow não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetWorkflow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetWorkflowResponse>> Get(Guid id)
        {
            GetWorkflowResponse result = await _mediator.Send(new GetWorkflowQuery
            {
                Id = id
            });

            if (result.Workflow == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Workflow
        /// </summary>
        /// <remarks>
        /// # Incluir Workflow
        /// 
        /// Inclui um Workflow na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /Workflow
        ///     {
        ///       "Workflow": {
        ///         "nome": "Novo Workflow",
        ///         "descricao": "Descrição do novo Workflow"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Workflow</param>        
        /// <response code="201">Workflow cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Workflow), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateWorkflowResponse>> Post([FromBody] CreateWorkflowCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateWorkflowResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetWorkflow", new { id = result.Workflow.Id }, result);
        }

        /// <summary>
        /// Alterar Workflow
        /// </summary>
        /// <remarks>
        /// # Alterar Workflow
        /// 
        /// Altera um Workflow na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /Workflow
        ///     {
        ///       "Workflow": {
        ///           "nome": "Workflow de Teste - 2",
        ///           "descricao": "Descrição do Workflow de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Workflow</param>        
        /// <param name="request">Workflow</param>        
        /// <response code="200">Workflow alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateWorkflowResponse>> Put(Guid id, [FromBody] UpdateWorkflowCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Workflow.Id)
            {
                return BadRequest();
            }

            UpdateWorkflowResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Workflow
        /// </summary>
        /// <remarks>
        /// # Remover Workflow
        /// 
        /// Remove um Workflow da base de dados.
        /// </remarks>
        /// <param name="id">Id do Workflow</param>        
        /// <response code="200">Workflow removido com sucesso</response>
        /// <response code="404">Workflow não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveWorkflowResponse>> Delete(Guid id)
        {
            RemoveWorkflowResponse result = await _mediator.Send(new RemoveWorkflowCommand
            {
                Id = id
            });

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
