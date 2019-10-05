using Cpnucleo.API.Filters;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Cpnucleo.API.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1", Deprecated = true)]
    [ServiceFilter(typeof(AuthorizerActionFilter), Order = 1)]
    public class ApontamentoController : ControllerBase
    {
        private readonly ICrudAppService<ApontamentoViewModel> _apontamentoAppService;

        public ApontamentoController(ICrudAppService<ApontamentoViewModel> apontamentoAppService)
        {
            _apontamentoAppService = apontamentoAppService;
        }

        /// <summary>
        /// Listar apontamentos
        /// </summary>
        /// <remarks>
        /// # Listar apontamentos
        /// 
        /// Lista apontamentos da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de apontamentos</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<ApontamentoViewModel> Get()
        {
            return _apontamentoAppService.Listar();
        }

        /// <summary>
        /// Consultar apontamento
        /// </summary>
        /// <remarks>
        /// # Consultar apontamento
        /// 
        /// Consulta um apontamento na base de dados.
        /// </remarks>
        /// <param name="id">Id do apontamento</param>        
        /// <response code="200">Retorna um apontamento</response>
        /// <response code="404">Apontamento não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ApontamentoViewModel> Get(Guid id)
        {
            ApontamentoViewModel apontamento = _apontamentoAppService.Consultar(id);

            if (apontamento == null)
            {
                return NotFound();
            }

            return Ok(apontamento);
        }

        /// <summary>
        /// Incluir apontamento
        /// </summary>
        /// <remarks>
        /// # Incluir apontamento
        /// 
        /// Inclui um apontamento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /apontamento
        ///     {
        ///        "descricao": "Descrição do novo apontamento",
        ///        "dataApontamento": "2019-09-21T15:56:00.503Z",
        ///        "qtdHoras": 6,
        ///        "percentualConcluido": 10,
        ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">Apontamento</param>        
        /// <response code="201">Apontamento cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<ApontamentoViewModel> Post([FromBody]ApontamentoViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _apontamentoAppService.Incluir(obj);
            }
            catch (DbUpdateException)
            {
                if (ObjExists(obj.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Alterar apontamento
        /// </summary>
        /// <remarks>
        /// # Alterar apontamento
        /// 
        /// Altera um apontamento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /apontamento
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "descricao": "Descrição do novo apontamento - alterado",
        ///        "dataApontamento": "2019-09-21T15:56:00.503Z",
        ///        "qtdHoras": 6,
        ///        "percentualConcluido": 10,
        ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do apontamento</param>        
        /// <param name="obj">Apontamento</param>        
        /// <response code="204">Apontamento alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]ApontamentoViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obj.Id)
            {
                return BadRequest();
            }

            try
            {
                _apontamentoAppService.Alterar(obj);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ObjExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Remover apontamento
        /// </summary>
        /// <remarks>
        /// # Remover apontamento
        /// 
        /// Remove um apontamento da base de dados.
        /// </remarks>
        /// <param name="id">Id do apontamento</param>        
        /// <response code="204">Apontamento removido com sucesso</response>
        /// <response code="404">Apontamento não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            ApontamentoViewModel obj = _apontamentoAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _apontamentoAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _apontamentoAppService.Consultar(id) != null;
        }
    }
}
