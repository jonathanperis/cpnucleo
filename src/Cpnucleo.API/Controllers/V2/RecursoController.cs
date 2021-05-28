using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
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
    public class RecursoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecursoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Recursos
        /// </summary>
        /// <remarks>
        /// # Listar Recursos
        /// 
        /// Lista Recursos da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Recursos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListRecursoResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListRecursoQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Recurso
        /// </summary>
        /// <remarks>
        /// # Consultar Recurso
        /// 
        /// Consulta um Recurso na base de dados.
        /// </remarks>
        /// <param name="id">Id do Recurso</param>        
        /// <response code="200">Retorna um Recurso</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetRecurso")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetRecursoResponse>> Get(Guid id)
        {
            GetRecursoResponse result = await _mediator.Send(new GetRecursoQuery
            {
                Id = id
            });

            if (result.Recurso == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Recurso
        /// </summary>
        /// <remarks>
        /// # Incluir Recurso
        /// 
        /// Inclui um Recurso na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /Recurso
        ///     {
        ///       "Recurso": {
        ///         "nome": "Novo Recurso",
        ///         "descricao": "Descrição do novo Recurso"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Recurso</param>        
        /// <response code="201">Recurso cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Recurso), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateRecursoResponse>> Post([FromBody] CreateRecursoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateRecursoResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetRecurso", new { id = result.Recurso.Id }, result);
        }

        /// <summary>
        /// Alterar Recurso
        /// </summary>
        /// <remarks>
        /// # Alterar Recurso
        /// 
        /// Altera um Recurso na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /Recurso
        ///     {
        ///       "Recurso": {
        ///           "nome": "Recurso de Teste - 2",
        ///           "descricao": "Descrição do Recurso de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Recurso</param>        
        /// <param name="request">Recurso</param>        
        /// <response code="200">Recurso alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateRecursoResponse>> Put(Guid id, [FromBody] UpdateRecursoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Recurso.Id)
            {
                return BadRequest();
            }

            UpdateRecursoResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Recurso
        /// </summary>
        /// <remarks>
        /// # Remover Recurso
        /// 
        /// Remove um Recurso da base de dados.
        /// </remarks>
        /// <param name="id">Id do Recurso</param>        
        /// <response code="200">Recurso removido com sucesso</response>
        /// <response code="404">Recurso não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveRecursoResponse>> Delete(Guid id)
        {
            RemoveRecursoResponse result = await _mediator.Send(new RemoveRecursoComand
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
