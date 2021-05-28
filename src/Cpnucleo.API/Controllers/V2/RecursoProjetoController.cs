using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
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
    public class RecursoProjetoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecursoProjetoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Recursos de Projeto
        /// </summary>
        /// <remarks>
        /// # Listar Recursos de Projeto
        /// 
        /// Lista Recursos de Projeto da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Recursos de Projeto</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListRecursoProjetoResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListRecursoProjetoQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Recurso de Projeto
        /// </summary>
        /// <remarks>
        /// # Consultar Recurso de Projeto
        /// 
        /// Consulta um Recurso de Projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do Recurso de Projeto</param>        
        /// <response code="200">Retorna um Recurso de Projeto</response>
        /// <response code="404">Recurso de Projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetRecursoProjeto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetRecursoProjetoResponse>> Get(Guid id)
        {
            GetRecursoProjetoResponse result = await _mediator.Send(new GetRecursoProjetoQuery
            {
                Id = id
            });

            if (result.RecursoProjeto == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Recurso de Projeto
        /// </summary>
        /// <remarks>
        /// # Incluir Recurso de Projeto
        /// 
        /// Inclui um Recurso de Projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /RecursoProjeto
        ///     {
        ///       "RecursoProjeto": {
        ///         "nome": "Novo RecursoProjeto",
        ///         "descricao": "Descrição do novo RecursoProjeto"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Recurso de Projeto</param>        
        /// <response code="201">Recurso de Projeto cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(RecursoProjeto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateRecursoProjetoResponse>> Post([FromBody] CreateRecursoProjetoCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateRecursoProjetoResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetRecursoProjeto", new { id = result.RecursoProjeto.Id }, result);
        }

        /// <summary>
        /// Alterar Recurso de Projeto
        /// </summary>
        /// <remarks>
        /// # Alterar Recurso de Projeto
        /// 
        /// Altera um Recurso de Projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /RecursoProjeto
        ///     {
        ///       "RecursoProjeto": {
        ///           "nome": "RecursoProjeto de Teste - 2",
        ///           "descricao": "Descrição do RecursoProjeto de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Recurso de Projeto</param>        
        /// <param name="request">Recurso de Projeto</param>        
        /// <response code="200">Recurso de Projeto alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateRecursoProjetoResponse>> Put(Guid id, [FromBody] UpdateRecursoProjetoCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.RecursoProjeto.Id)
            {
                return BadRequest();
            }

            UpdateRecursoProjetoResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Recurso de Projeto
        /// </summary>
        /// <remarks>
        /// # Remover Recurso de Projeto
        /// 
        /// Remove um Recurso de Projeto da base de dados.
        /// </remarks>
        /// <param name="id">Id do Recurso de Projeto</param>        
        /// <response code="200">Recurso de Projeto removido com sucesso</response>
        /// <response code="404">Recurso de Projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveRecursoProjetoResponse>> Delete(Guid id)
        {
            RemoveRecursoProjetoResponse result = await _mediator.Send(new RemoveRecursoProjetoCommand
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
