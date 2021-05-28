using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
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
    public class TipoTarefaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TipoTarefaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Tipos de Tarefa
        /// </summary>
        /// <remarks>
        /// # Listar Tipos de Tarefa
        /// 
        /// Lista Tipos de Tarefa da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Tipos de Tarefa</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListTipoTarefaResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListTipoTarefaQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Tipo de Tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar Tipo de Tarefa
        /// 
        /// Consulta um Tipo de Tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id do Tipo de Tarefa</param>        
        /// <response code="200">Retorna um Tipo de Tarefa</response>
        /// <response code="404">Tipo de Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetTipoTarefa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTipoTarefaResponse>> Get(Guid id)
        {
            GetTipoTarefaResponse result = await _mediator.Send(new GetTipoTarefaQuery
            {
                Id = id
            });

            if (result.TipoTarefa == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Tipo de Tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir Tipo de Tarefa
        /// 
        /// Inclui um Tipo de Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /TipoTarefa
        ///     {
        ///       "TipoTarefa": {
        ///         "nome": "Novo TipoTarefa",
        ///         "descricao": "Descrição do novo TipoTarefa"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Tipo de Tarefa</param>        
        /// <response code="201">Tipo de Tarefa cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(TipoTarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateTipoTarefaResponse>> Post([FromBody] CreateTipoTarefaCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateTipoTarefaResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetTipoTarefa", new { id = result.TipoTarefa.Id }, result);
        }

        /// <summary>
        /// Alterar Tipo de Tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar Tipo de Tarefa
        /// 
        /// Altera um Tipo de Tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /TipoTarefa
        ///     {
        ///       "TipoTarefa": {
        ///           "nome": "TipoTarefa de Teste - 2",
        ///           "descricao": "Descrição do TipoTarefa de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Tipo de Tarefa</param>        
        /// <param name="request">Tipo de Tarefa</param>        
        /// <response code="200">Tipo de Tarefa alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateTipoTarefaResponse>> Put(Guid id, [FromBody] UpdateTipoTarefaCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.TipoTarefa.Id)
            {
                return BadRequest();
            }

            UpdateTipoTarefaResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Tipo de Tarefa
        /// </summary>
        /// <remarks>
        /// # Remover Tipo de Tarefa
        /// 
        /// Remove um Tipo de Tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id do Tipo de Tarefa</param>        
        /// <response code="200">Tipo de Tarefa removido com sucesso</response>
        /// <response code="404">Tipo de Tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveTipoTarefaResponse>> Delete(Guid id)
        {
            RemoveTipoTarefaResponse result = await _mediator.Send(new RemoveTipoTarefaCommand
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
