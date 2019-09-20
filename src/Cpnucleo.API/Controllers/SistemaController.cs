using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cpnucleo.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SistemaController : Controller
    {
        private readonly IAppService<SistemaViewModel> _sistemaAppService;

        public SistemaController(IAppService<SistemaViewModel> sistemaAppService)
        {
            _sistemaAppService = sistemaAppService;
        }

        /// <summary>
        /// Listar sistemas
        /// </summary>
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

            return sistema;
        }

        /// <summary>
        /// Incluir sistema
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /sistema
        ///     {
        ///        "nome": "Novo sistema",
        ///        "descricao": "Descrição do novo sistema"
        ///     }
        ///
        /// </remarks>
        /// <param name="obj">sistema</param>        
        /// <response code="201">Retorna um novo sistema</response>
        [HttpPost]
        [ProducesResponseType(201)]
        public ActionResult<SistemaViewModel> Post([FromBody]SistemaViewModel obj)
        {
            _sistemaAppService.Incluir(obj);

            return CreatedAtAction(nameof(Get), new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Alterar sistema
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /sistema
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo sistema - alterado",
        ///        "descricao": "Descrição do novo sistema - alterado"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">Id do sistema</param>        
        /// <param name="obj">sistema</param>        
        /// <response code="204">Sistema alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]SistemaViewModel obj)
        {
            if (id != obj.Id)
            {
                return BadRequest();
            }

            _sistemaAppService.Alterar(obj);

            return NoContent();
        }

        /// <summary>
        /// Remover sistema
        /// </summary>
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
    }
}
