using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.ImpedimentoTarefa;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.ImpedimentoTarefa;
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
    public class ImpedimentoTarefaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImpedimentoTarefaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Impedimentos de Tarefa
        /// </summary>
        /// <remarks>
        /// # Listar Impedimentos de Tarefa
        /// 
        /// Lista Impedimentos de Tarefa da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Impedimentos de Tarefa</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListImpedimentoTarefaResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListImpedimentoTarefaQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Impedimento de Tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar Impedimento de Tarefa
        /// 
        /// Consulta um Impedimento de Tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id do Impedimento de Tarefa</param>        
        /// <response code="200">Retorna um Impedimento de Tarefa</response>
        /// <response code="404">Impedimento de Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetImpedimentoTarefa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetImpedimentoTarefaResponse>> Get(Guid id)
        {
            GetImpedimentoTarefaResponse result = await _mediator.Send(new GetImpedimentoTarefaQuery
            {
                Id = id
            });

            if (result.ImpedimentoTarefa == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Impedimento de Tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir Impedimento de Tarefa
        /// 
        /// Inclui um Impedimento de Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /ImpedimentoTarefa
        ///     {
        ///       "ImpedimentoTarefa": {
        ///         "nome": "Novo Impedimento de Tarefa",
        ///         "descricao": "Descrição do novo Impedimento de Tarefa"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Impedimento de Tarefa</param>        
        /// <response code="201">Impedimento de Tarefa cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(ImpedimentoTarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateImpedimentoTarefaResponse>> Post([FromBody] CreateImpedimentoTarefaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateImpedimentoTarefaResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetImpedimentoTarefa", new { id = result.ImpedimentoTarefa.Id }, result);
        }

        /// <summary>
        /// Alterar Impedimento de Tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar Impedimento de Tarefa
        /// 
        /// Altera um Impedimento de Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /ImpedimentoTarefa
        ///     {
        ///       "ImpedimentoTarefa": {
        ///           "nome": "Impedimento de Tarefa de Teste - 2",
        ///           "descricao": "Descrição do Impedimento de Tarefa de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Impedimento de Tarefa</param>        
        /// <param name="request">Impedimento de Tarefa</param>        
        /// <response code="200">Impedimento de Tarefa alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateImpedimentoTarefaResponse>> Put(Guid id, [FromBody] UpdateImpedimentoTarefaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.ImpedimentoTarefa.Id)
            {
                return BadRequest();
            }

            UpdateImpedimentoTarefaResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Impedimento de Tarefa
        /// </summary>
        /// <remarks>
        /// # Remover Impedimento de Tarefa
        /// 
        /// Remove um Impedimento de Tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id do Impedimento de Tarefa</param>        
        /// <response code="200">Impedimento de Tarefa removido com sucesso</response>
        /// <response code="404">Impedimento de Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveImpedimentoTarefaResponse>> Delete(Guid id)
        {
            RemoveImpedimentoTarefaResponse result = await _mediator.Send(new RemoveImpedimentoTarefaComand
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
