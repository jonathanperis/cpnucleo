using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Impedimento;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
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
    public class ImpedimentoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImpedimentoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Impedimentos
        /// </summary>
        /// <remarks>
        /// # Listar Impedimentos
        /// 
        /// Lista Impedimentos da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Impedimentos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListImpedimentoResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListImpedimentoQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Impedimento
        /// </summary>
        /// <remarks>
        /// # Consultar Impedimento
        /// 
        /// Consulta um Impedimento na base de dados.
        /// </remarks>
        /// <param name="id">Id do Impedimento</param>        
        /// <response code="200">Retorna um Impedimento</response>
        /// <response code="404">Impedimento não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetImpedimento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetImpedimentoResponse>> Get(Guid id)
        {
            GetImpedimentoResponse result = await _mediator.Send(new GetImpedimentoQuery
            {
                Id = id
            });

            if (result.Impedimento == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Impedimento
        /// </summary>
        /// <remarks>
        /// # Incluir Impedimento
        /// 
        /// Inclui um Impedimento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /Impedimento
        ///     {
        ///       "Impedimento": {
        ///         "nome": "Novo Impedimento",
        ///         "descricao": "Descrição do novo Impedimento"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Impedimento</param>        
        /// <response code="201">Impedimento cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Impedimento), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateImpedimentoResponse>> Post([FromBody] CreateImpedimentoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateImpedimentoResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetImpedimento", new { id = result.Impedimento.Id }, result);
        }

        /// <summary>
        /// Alterar Impedimento
        /// </summary>
        /// <remarks>
        /// # Alterar Impedimento
        /// 
        /// Altera um Impedimento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /Impedimento
        ///     {
        ///       "Impedimento": {
        ///           "nome": "Impedimento de Teste - 2",
        ///           "descricao": "Descrição do Impedimento de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Impedimento</param>        
        /// <param name="request">Impedimento</param>        
        /// <response code="200">Impedimento alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateImpedimentoResponse>> Put(Guid id, [FromBody] UpdateImpedimentoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Impedimento.Id)
            {
                return BadRequest();
            }

            UpdateImpedimentoResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Impedimento
        /// </summary>
        /// <remarks>
        /// # Remover Impedimento
        /// 
        /// Remove um Impedimento da base de dados.
        /// </remarks>
        /// <param name="id">Id do Impedimento</param>        
        /// <response code="200">Impedimento removido com sucesso</response>
        /// <response code="404">Impedimento não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveImpedimentoResponse>> Delete(Guid id)
        {
            RemoveImpedimentoResponse result = await _mediator.Send(new RemoveImpedimentoComand
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
