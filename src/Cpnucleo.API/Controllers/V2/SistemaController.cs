using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Security.Filters;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Cpnucleo.API.Controllers.V2
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    [ServiceFilter(typeof(AuthorizerActionFilter), Order = 1)]
    public class SistemaController : ControllerBase
    {
        private readonly ICrudAppService<SistemaViewModel> _sistemaAppService;

        public SistemaController(ICrudAppService<SistemaViewModel> sistemaAppService)
        {
            _sistemaAppService = sistemaAppService;
        }

        /// <summary>
        /// Listar sistemas
        /// </summary>
        /// <remarks>
        /// # Listar sistemas
        /// 
        /// Lista sistemas da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de sistemas</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<SistemaViewModel> Get()
        {
            return _sistemaAppService.Listar();
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
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<SistemaViewModel> Get(Guid id)
        {
            SistemaViewModel sistema = _sistemaAppService.Consultar(id);

            if (sistema == null)
            {
                return NotFound();
            }

            return Ok(sistema);
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
        ///        "nome": "Novo sistema",
        ///        "descricao": "Descrição do novo sistema"
        ///     }
        /// </remarks>
        /// <param name="obj">Sistema</param>        
        /// <response code="201">Sistema cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<SistemaViewModel> Post([FromBody]SistemaViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _sistemaAppService.Incluir(obj);
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
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo sistema - alterado",
        ///        "descricao": "Descrição do novo sistema - alterado",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do sistema</param>        
        /// <param name="obj">Sistema</param>        
        /// <response code="204">Sistema alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]SistemaViewModel obj)
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
                _sistemaAppService.Alterar(obj);
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
        /// Remover sistema
        /// </summary>
        /// <remarks>
        /// # Remover sistema
        /// 
        /// Remove um sistema da base de dados.
        /// </remarks>
        /// <param name="id">Id do sistema</param>        
        /// <response code="204">Sistema removido com sucesso</response>
        /// <response code="404">Sistema não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            SistemaViewModel obj = _sistemaAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _sistemaAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _sistemaAppService.Consultar(id) != null;
        }
    }
}
