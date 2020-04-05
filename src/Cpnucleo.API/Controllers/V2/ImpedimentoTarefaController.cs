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
    public class ImpedimentoTarefaController : ControllerBase
    {
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        public ImpedimentoTarefaController(IImpedimentoTarefaAppService impedimentoTarefaAppService)
        {
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
        }

        /// <summary>
        /// Listar impedimentos de tarefa
        /// </summary>
        /// <remarks>
        /// # Listar impedimentos de tarefa
        /// 
        /// Lista impedimentos de tarefa da base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de impedimentos de tarefa</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<ImpedimentoTarefaViewModel> Get()
        {
            return _impedimentoTarefaAppService.Listar();
        }

        /// <summary>
        /// Listar impedimentos de tarefa por id tarefa
        /// </summary>
        /// <remarks>
        /// # Listar impedimentos de tarefa por id tarefa
        /// 
        /// Lista impedimentos de tarefa por id tarefa na base de dados.
        /// </remarks>
        /// <param name="idTarefa">Id da tarefa</param>        
        /// <response code="200">Retorna uma lista de impedimentos de tarefa</response>
        [HttpGet("GetByTarefa/{idTarefa}")]
        [ProducesResponseType(200)]
        public IEnumerable<ImpedimentoTarefaViewModel> GetByTarefa(Guid idTarefa)
        {
            return _impedimentoTarefaAppService.ListarPorTarefa(idTarefa);
        }

        /// <summary>
        /// Consultar impedimento de tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar impedimento de tarefa
        /// 
        /// Consulta um impedimento de tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id do impedimento de tarefa</param>        
        /// <response code="200">Retorna um impedimento de tarefa</response>
        /// <response code="404">Impedimento de tarefa não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ImpedimentoTarefaViewModel> Get(Guid id)
        {
            ImpedimentoTarefaViewModel impedimentoTarefa = _impedimentoTarefaAppService.Consultar(id);

            if (impedimentoTarefa == null)
            {
                return NotFound();
            }

            return Ok(impedimentoTarefa);
        }

        /// <summary>
        /// Incluir impedimento de tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir impedimento de tarefa
        /// 
        /// Inclui um impedimento de tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /impedimentoTarefa
        ///     {
        ///        "descricao": "Descrição do novo impedimento de tarefa",
        ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idImpedimento": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">Impedimento de tarefa</param>        
        /// <response code="201">Impedimento de tarefa cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<ImpedimentoTarefaViewModel> Post([FromBody]ImpedimentoTarefaViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _impedimentoTarefaAppService.Incluir(obj);
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
        /// Alterar impedimento de tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar impedimento de tarefa
        /// 
        /// Altera um impedimento de tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /impedimentoTarefa
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "descricao": "Descrição do novo impedimento de tarefa - alterado",
        ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "idImpedimento": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do impedimento de tarefa</param>        
        /// <param name="obj">Impedimento de tarefa</param>        
        /// <response code="204">Impedimento de tarefa alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]ImpedimentoTarefaViewModel obj)
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
                _impedimentoTarefaAppService.Alterar(obj);
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
        /// Remover impedimento de tarefa
        /// </summary>
        /// <remarks>
        /// # Remover impedimento de tarefa
        /// 
        /// Remove um impedimento de tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id do impedimento de tarefa</param>        
        /// <response code="204">Impedimento de tarefa removido com sucesso</response>
        /// <response code="404">Impedimento de tarefa não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            ImpedimentoTarefaViewModel obj = _impedimentoTarefaAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _impedimentoTarefaAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _impedimentoTarefaAppService.Consultar(id) != null;
        }
    }
}
