using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Tarefa;
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
    public class TarefaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TarefaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Tarefas
        /// </summary>
        /// <remarks>
        /// # Listar Tarefas
        /// 
        /// Lista Tarefas da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Tarefas</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListTarefaResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListTarefaQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar Tarefa
        /// 
        /// Consulta uma Tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id da Tarefa</param>        
        /// <response code="200">Retorna uma Tarefa</response>
        /// <response code="404">Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetTarefa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTarefaResponse>> Get(Guid id)
        {
            GetTarefaResponse result = await _mediator.Send(new GetTarefaQuery
            {
                Id = id
            });

            if (result.Tarefa == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir Tarefa
        /// 
        /// Inclui uma Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /Tarefa
        ///     {
        ///       "Tarefa": {
        ///         "nome": "Novo Tarefa",
        ///         "descricao": "Descrição do novo Tarefa"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Tarefa</param>        
        /// <response code="201">Tarefa cadastrada com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Tarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateTarefaResponse>> Post([FromBody] CreateTarefaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateTarefaResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetTarefa", new { id = result.Tarefa.Id }, result);
        }

        /// <summary>
        /// Alterar Tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar Tarefa
        /// 
        /// Altera uma Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /Tarefa
        ///     {
        ///       "Tarefa": {
        ///           "nome": "Tarefa de Teste - 2",
        ///           "descricao": "Descrição do Tarefa de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id da Tarefa</param>        
        /// <param name="request">Tarefa</param>        
        /// <response code="200">Tarefa alterada com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateTarefaResponse>> Put(Guid id, [FromBody] UpdateTarefaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Tarefa.Id)
            {
                return BadRequest();
            }

            UpdateTarefaResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Tarefa
        /// </summary>
        /// <remarks>
        /// # Remover Tarefa
        /// 
        /// Remove uma Tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id da Tarefa</param>        
        /// <response code="200">Tarefa removida com sucesso</response>
        /// <response code="404">Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveTarefaResponse>> Delete(Guid id)
        {
            RemoveTarefaResponse result = await _mediator.Send(new RemoveTarefaComand
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
