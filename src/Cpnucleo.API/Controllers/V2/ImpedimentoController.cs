using Cpnucleo.API.Filters;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
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
    public class ImpedimentoController : ControllerBase
    {
        private readonly IAppService<ImpedimentoViewModel> _impedimentoAppService;

        public ImpedimentoController(IAppService<ImpedimentoViewModel> impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        /// <summary>
        /// Listar impedimentos
        /// </summary>
        /// <remarks>
        /// # Listar impedimentos
        /// 
        /// Lista impedimentos da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de impedimentos</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<ImpedimentoViewModel> Get()
        {
            return _impedimentoAppService.Listar();
        }

        /// <summary>
        /// Consultar impedimento
        /// </summary>
        /// <remarks>
        /// # Consultar impedimento
        /// 
        /// Consulta um impedimento na base de dados.
        /// </remarks>
        /// <param name="id">Id do impedimento</param>        
        /// <response code="200">Retorna um impedimento</response>
        /// <response code="404">Impedimento não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ImpedimentoViewModel> Get(Guid id)
        {
            ImpedimentoViewModel impedimento = _impedimentoAppService.Consultar(id);

            if (impedimento == null)
            {
                return NotFound();
            }

            return Ok(impedimento);
        }

        /// <summary>
        /// Incluir impedimento
        /// </summary>
        /// <remarks>
        /// # Incluir impedimento
        /// 
        /// Inclui um impedimento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /impedimento
        ///     {
        ///        "nome": "Novo impedimento"
        ///     }
        /// </remarks>
        /// <param name="obj">Impedimento</param>        
        /// <response code="201">Impedimento cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<ImpedimentoViewModel> Post([FromBody]ImpedimentoViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _impedimentoAppService.Incluir(obj);
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
        /// Alterar impedimento
        /// </summary>
        /// <remarks>
        /// # Alterar impedimento
        /// 
        /// Altera um impedimento na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /impedimento
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo impedimento - alterado",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do impedimento</param>        
        /// <param name="obj">Impedimento</param>        
        /// <response code="204">Impedimento alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]ImpedimentoViewModel obj)
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
                _impedimentoAppService.Alterar(obj);
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
        /// Remover impedimento
        /// </summary>
        /// <remarks>
        /// # Remover impedimento
        /// 
        /// Remove um impedimento da base de dados.
        /// </remarks>
        /// <param name="id">Id do impedimento</param>        
        /// <response code="204">Impedimento removido com sucesso</response>
        /// <response code="404">Impedimento não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            ImpedimentoViewModel obj = _impedimentoAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _impedimentoAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _impedimentoAppService.Consultar(id) != null;
        }
    }
}
