using Cpnucleo.API.Filters;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Cpnucleo.API.V1.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ServiceFilter(typeof(AuthorizerActionFilter), Order = 1)]
    public class RecursoProjetoController : ControllerBase
    {
        private readonly IAppService<RecursoProjetoViewModel> _recursoProjetoAppService;

        public RecursoProjetoController(IAppService<RecursoProjetoViewModel> recursoProjetoAppService)
        {
            _recursoProjetoAppService = recursoProjetoAppService;
        }

        /// <summary>
        /// Listar recursos de projeto
        /// </summary>
        /// <remarks>
        /// # Listar recursos de projeto
        /// 
        /// Lista recursos de projeto na base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de recursos de projeto</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<RecursoProjetoViewModel> Get()
        {
            return _recursoProjetoAppService.Listar();
        }

        /// <summary>
        /// Consultar recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Consultar recurso de projeto
        /// 
        /// Consulta um recurso de projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso de projeto</param>        
        /// <response code="200">Retorna um recurso de projeto</response>
        /// <response code="404">Recurso de projeto não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<RecursoProjetoViewModel> Get(Guid id)
        {
            RecursoProjetoViewModel recursoProjeto = _recursoProjetoAppService.Consultar(id);

            if (recursoProjeto == null)
            {
                return NotFound();
            }

            return Ok(recursoProjeto);
        }

        /// <summary>
        /// Incluir recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Incluir recurso de projeto
        /// 
        /// Inclui um recurso de projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /recursoProjeto
        ///     {
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idProjeto": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">Recurso de projeto</param>        
        /// <response code="201">Recurso de projeto cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<RecursoProjetoViewModel> Post([FromBody]RecursoProjetoViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _recursoProjetoAppService.Incluir(obj);
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
        /// Alterar recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Alterar recurso de projeto
        /// 
        /// Altera um recurso de projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /recursoProjeto
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idProjeto": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do recurso de projeto</param>        
        /// <param name="obj">Recurso de projeto</param>        
        /// <response code="204">Recurso de projeto alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]RecursoProjetoViewModel obj)
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
                _recursoProjetoAppService.Alterar(obj);
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
        /// Remover recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Remover recurso de projeto
        /// 
        /// Remove um recurso de projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso de projeto</param>        
        /// <response code="204">Recurso de projeto removido com sucesso</response>
        /// <response code="404">Recurso de projeto não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            RecursoProjetoViewModel obj = _recursoProjetoAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _recursoProjetoAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _recursoProjetoAppService.Consultar(id) != null;
        }
    }
}
