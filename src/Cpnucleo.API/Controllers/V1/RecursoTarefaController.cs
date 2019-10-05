using Cpnucleo.API.Filters;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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
    public class RecursoTarefaController : ControllerBase
    {
        private readonly ICrudAppService<RecursoTarefaViewModel> _recursoTarefaAppService;

        public RecursoTarefaController(ICrudAppService<RecursoTarefaViewModel> recursoTarefaAppService)
        {
            _recursoTarefaAppService = recursoTarefaAppService;
        }

        /// <summary>
        /// Listar recursos de tarefa
        /// </summary>
        /// <remarks>
        /// # Listar recursos de tarefa
        /// 
        /// Lista recursos de tarefa da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de recursos de tarefa</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<RecursoTarefaViewModel> Get()
        {
            return _recursoTarefaAppService.Listar();
        }

        /// <summary>
        /// Consultar recurso de tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar recurso de tarefa
        /// 
        /// Consulta um recurso de tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso de tarefa</param>        
        /// <response code="200">Retorna um recurso de tarefa</response>
        /// <response code="404">Recurso de tarefa não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<RecursoTarefaViewModel> Get(Guid id)
        {
            RecursoTarefaViewModel recursoTarefa = _recursoTarefaAppService.Consultar(id);

            if (recursoTarefa == null)
            {
                return NotFound();
            }

            return Ok(recursoTarefa);
        }

        /// <summary>
        /// Incluir recurso de tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir recurso de tarefa
        /// 
        /// Inclui um recurso de tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /recursoTarefa
        ///     {
        ///        "percentualTarefa": 15,
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">Recurso de tarefa</param>        
        /// <response code="201">Recurso de tarefa cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<RecursoTarefaViewModel> Post([FromBody]RecursoTarefaViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _recursoTarefaAppService.Incluir(obj);
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
        /// Alterar recurso de tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar recurso de tarefa
        /// 
        /// Altera um recurso de tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /recursoTarefa
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "percentualTarefa": 15,
        ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do recurso de tarefa</param>        
        /// <param name="obj">Recurso de tarefa</param>        
        /// <response code="204">Recurso de tarefa alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]RecursoTarefaViewModel obj)
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
                _recursoTarefaAppService.Alterar(obj);
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
        /// Remover recurso de tarefa
        /// </summary>
        /// <remarks>
        /// # Remover recurso de tarefa
        /// 
        /// Remove um recurso de tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso de tarefa</param>        
        /// <response code="204">Recurso de tarefa removido com sucesso</response>
        /// <response code="404">Recurso de tarefa não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            RecursoTarefaViewModel obj = _recursoTarefaAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _recursoTarefaAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _recursoTarefaAppService.Consultar(id) != null;
        }
    }
}
