using Cpnucleo.Domain.Entities;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
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
    public class ProjetoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjetoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Listar Projetos
        /// </summary>
        /// <remarks>
        /// # Listar Projetos
        /// 
        /// Lista Projetos da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de Projetos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ListProjetoResponse> Get(bool getDependencies = false)
        {
            return await _mediator.Send(new ListProjetoQuery
            {
                GetDependencies = getDependencies
            });
        }

        /// <summary>
        /// Consultar Projeto
        /// </summary>
        /// <remarks>
        /// # Consultar Projeto
        /// 
        /// Consulta um Projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do Projeto</param>        
        /// <response code="200">Retorna um Projeto</response>
        /// <response code="404">Projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetProjeto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetProjetoResponse>> Get(Guid id)
        {
            GetProjetoResponse result = await _mediator.Send(new GetProjetoQuery
            {
                Id = id
            });

            if (result.Projeto == null)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Incluir Projeto
        /// </summary>
        /// <remarks>
        /// # Incluir Projeto
        /// 
        /// Inclui um Projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /Projeto
        ///     {
        ///       "Projeto": {
        ///         "nome": "Novo Projeto",
        ///         "descricao": "Descrição do novo Projeto"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="request">Projeto</param>        
        /// <response code="201">Projeto cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(Projeto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<CreateProjetoResponse>> Post([FromBody] CreateProjetoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CreateProjetoResponse result = await _mediator.Send(request);

            return CreatedAtRoute("GetProjeto", new { id = result.Projeto.Id }, result);
        }

        /// <summary>
        /// Alterar Projeto
        /// </summary>
        /// <remarks>
        /// # Alterar Projeto
        /// 
        /// Altera um Projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /Projeto
        ///     {
        ///       "Projeto": {
        ///           "nome": "Projeto de Teste - 2",
        ///           "descricao": "Descrição do Projeto de Teste - 2",
        ///           "id": "b98631f9-89b4-4414-2353-08d7555e3dd6",
        ///           "dataInclusao": "2019-10-20T13:05:57"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="id">Id do Projeto</param>        
        /// <param name="request">Projeto</param>        
        /// <response code="200">Projeto alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UpdateProjetoResponse>> Put(Guid id, [FromBody] UpdateProjetoComand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.Projeto.Id)
            {
                return BadRequest();
            }

            UpdateProjetoResponse result = await _mediator.Send(request);

            return Ok(result);
        }

        /// <summary>
        /// Remover Projeto
        /// </summary>
        /// <remarks>
        /// # Remover Projeto
        /// 
        /// Remove um Projeto da base de dados.
        /// </remarks>
        /// <param name="id">Id do Projeto</param>        
        /// <response code="200">Projeto removido com sucesso</response>
        /// <response code="404">Projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RemoveProjetoResponse>> Delete(Guid id)
        {
            RemoveProjetoResponse result = await _mediator.Send(new RemoveProjetoComand
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
