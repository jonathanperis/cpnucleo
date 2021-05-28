using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoTarefa;
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
    public class RecursoTarefaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecursoTarefaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Recursos de Tarefa
        /// </summary>
        /// <remarks>
        /// # Listar Recursos de Tarefa
        /// 
        /// Lista Recursos de Tarefa da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Recursos de Tarefa</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListRecursoTarefaResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListRecursoTarefaQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Recurso de Tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar Recurso de Tarefa
        /// 
        /// Consulta um Recurso de Tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id do Recurso de Tarefa</param>        
        /// <response code="200">Retorna um Recurso de Tarefa</response>
        /// <response code="404">Recurso de Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetRecursoTarefa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetRecursoTarefaResponse>> Get(Guid id)
        {
            GetRecursoTarefaResponse result = await _mediator.Send(new GetRecursoTarefaQuery
            {
                Id = id
            });

            if (result.RecursoTarefa == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Recurso de Tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir Recurso de Tarefa
        /// 
        /// Inclui um Recurso de Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /RecursoTarefa
        ///     {
        ///       "RecursoTarefa": {
        ///         "nome": "Novo RecursoTarefa",
        ///         "descricao": "Descrição do novo RecursoTarefa"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Recurso de Tarefa</param>        
        /// <response code="201">Recurso de Tarefa cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(RecursoTarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateRecursoTarefaResponse>> Post([FromBody] CreateRecursoTarefaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateRecursoTarefaResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetRecursoTarefa", new { id = result.RecursoTarefa.Id }, result);
        }

        /// <summary>
        /// Alterar Recurso de Tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar Recurso de Tarefa
        /// 
        /// Altera um Recurso de Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /RecursoTarefa
        ///     {
        ///       "RecursoTarefa": {
        ///           "nome": "RecursoTarefa de Teste - 2",
        ///           "descricao": "Descrição do RecursoTarefa de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Recurso de Tarefa</param>        
        /// <param name="request">Recurso de Tarefa</param>        
        /// <response code="200">Recurso de Tarefa alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateRecursoTarefaResponse>> Put(Guid id, [FromBody] UpdateRecursoTarefaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.RecursoTarefa.Id)
            {
                return BadRequest();
            }

            UpdateRecursoTarefaResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Recurso de Tarefa
        /// </summary>
        /// <remarks>
        /// # Remover Recurso de Tarefa
        /// 
        /// Remove um Recurso de Tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id do Recurso de Tarefa</param>        
        /// <response code="200">Recurso de Tarefa removido com sucesso</response>
        /// <response code="404">Recurso de Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveRecursoTarefaResponse>> Delete(Guid id)
        {
            RemoveRecursoTarefaResponse result = await _mediator.Send(new RemoveRecursoTarefaComand
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
