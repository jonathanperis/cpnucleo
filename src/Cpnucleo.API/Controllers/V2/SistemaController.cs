using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using MediatR;
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
    // [Authorize]
    public class SistemaController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SistemaController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar sistemas
        /// </summary>
        /// <remarks>
        /// # Listar sistemas
        /// 
        /// Lista sistemas da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de sistemas</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListSistemaResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListSistemaQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar sistema
        /// </summary>
        /// <remarks>
        /// # Consultar sistema
        /// 
        /// Consulta um sistema na base de dados.
        /// </remarks>
        /// <param name="id">Id do sistema</param>        
        /// <response code="200">Retorna um sistema</response>
        /// <response code="404">Sistema não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetSistema")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetSistemaResponse>> Get(Guid id)
        {
            GetSistemaResponse result = await _mediator.Send(new GetSistemaQuery
            {
                Id = id
            });

            if (result.Sistema == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir sistema
        /// </summary>
        /// <remarks>
        /// # Incluir sistema
        /// 
        /// Inclui um sistema na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /sistema
        ///     {
        ///       "sistema": {
        ///         "nome": "Novo sistema",
        ///         "descricao": "Descrição do novo sistema"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Sistema</param>        
        /// <response code="201">Sistema cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Sistema), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateSistemaResponse>> Post([FromBody] CreateSistemaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateSistemaResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetSistema", new { id = result.Sistema.Id }, result);
        }

        /// <summary>
        /// Alterar sistema
        /// </summary>
        /// <remarks>
        /// # Alterar sistema
        /// 
        /// Altera um sistema na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /sistema
        ///     {
        ///       "sistema": {
        ///           "nome": "Sistema de Teste - 2",
        ///           "descricao": "Descrição do Sistema de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do sistema</param>        
        /// <param name="request">Sistema</param>        
        /// <response code="200">Sistema alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateSistemaResponse>> Put(Guid id, [FromBody] UpdateSistemaComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Sistema.Id)
            {
                return BadRequest();
            }

            UpdateSistemaResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover sistema
        /// </summary>
        /// <remarks>
        /// # Remover sistema
        /// 
        /// Remove um sistema da base de dados.
        /// </remarks>
        /// <param name="id">Id do sistema</param>        
        /// <response code="200">Sistema removido com sucesso</response>
        /// <response code="404">Sistema não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveSistemaResponse>> Delete(Guid id)
        {
            RemoveSistemaResponse result = await _mediator.Send(new RemoveSistemaComand
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
