using Cpnucleo.Domain.Commands.Requests.Apontamento;
using Cpnucleo.Domain.Commands.Responses.Apontamento;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Queries.Requests.Apontamento;
using Cpnucleo.Domain.Queries.Responses.Apontamento;
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
    public class ApontamentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApontamentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Apontamentos
        /// </summary>
        /// <remarks>
        /// # Listar Apontamentos
        /// 
        /// Lista Apontamentos da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Apontamentos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListApontamentoResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListApontamentoQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Apontamento
        /// </summary>
        /// <remarks>
        /// # Consultar Apontamento
        /// 
        /// Consulta um Apontamento na base de dados.
        /// </remarks>
        /// <param name="id">Id do Apontamento</param>        
        /// <response code="200">Retorna um Apontamento</response>
        /// <response code="404">Apontamento não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetApontamento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetApontamentoResponse>> Get(Guid id)
        {
            GetApontamentoResponse result = await _mediator.Send(new GetApontamentoQuery
            {
                Id = id
            });

            if (result.Apontamento == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Apontamento
        /// </summary>
        /// <remarks>
        /// # Incluir Apontamento
        /// 
        /// Inclui um Apontamento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /Apontamento
        ///     {
        ///       "Apontamento": {
        ///         "nome": "Novo Apontamento",
        ///         "descricao": "Descrição do novo Apontamento"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Apontamento</param>        
        /// <response code="201">Apontamento cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Apontamento), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateApontamentoResponse>> Post([FromBody] CreateApontamentoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateApontamentoResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetApontamento", new { id = result.Apontamento.Id }, result);
        }

        /// <summary>
        /// Alterar Apontamento
        /// </summary>
        /// <remarks>
        /// # Alterar Apontamento
        /// 
        /// Altera um Apontamento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /Apontamento
        ///     {
        ///       "Apontamento": {
        ///           "nome": "Apontamento de Teste - 2",
        ///           "descricao": "Descrição do Apontamento de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Apontamento</param>        
        /// <param name="request">Apontamento</param>        
        /// <response code="200">Apontamento alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateApontamentoResponse>> Put(Guid id, [FromBody] UpdateApontamentoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Apontamento.Id)
            {
                return BadRequest();
            }

            UpdateApontamentoResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Apontamento
        /// </summary>
        /// <remarks>
        /// # Remover Apontamento
        /// 
        /// Remove um Apontamento da base de dados.
        /// </remarks>
        /// <param name="id">Id do Apontamento</param>        
        /// <response code="200">Apontamento removido com sucesso</response>
        /// <response code="404">Apontamento não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveApontamentoResponse>> Delete(Guid id)
        {
            RemoveApontamentoResponse result = await _mediator.Send(new RemoveApontamentoComand
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
