using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.API.Controllers.V2
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    [Authorize]
    public class RecursoTarefaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecursoTarefaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar recursos de tarefa
        /// </summary>
        /// <remarks>
        /// # Listar recursos de tarefa
        /// 
        /// Lista recursos de tarefa da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de recursos de tarefa</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<RecursoTarefa>> Get(bool getDependencies = false)
        {
            return await _unitOfWork.RecursoTarefaRepository.AllAsync(getDependencies);
        }

        /// <summary>
        /// Listar recursos de tarefa por id tarefa
        /// </summary>
        /// <remarks>
        /// # Listar recursos de tarefa id tarefa
        /// 
        /// Lista recursos de tarefa id tarefa na base de dados.
        /// </remarks>
        /// <param name="idTarefa">Id da tarefa</param>        
        /// <response code="200">Retorna uma lista de recursos de tarefa</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("GetByTarefa/{idTarefa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<RecursoTarefa>> GetByTarefa(Guid idTarefa)
        {
            return await _unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(idTarefa);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetRecursoTarefa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecursoTarefa>> Get(Guid id)
        {
            RecursoTarefa recursoTarefa = await _unitOfWork.RecursoTarefaRepository.GetAsync(id);

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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(RecursoTarefa), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<RecursoTarefa>> Post([FromBody] RecursoTarefa obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                obj = await _unitOfWork.RecursoTarefaRepository.AddAsync(obj);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (await ObjExists(obj.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("GetRecursoTarefa", new { id = obj.Id }, obj);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(Guid id, [FromBody] RecursoTarefa obj)
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
                _unitOfWork.RecursoTarefaRepository.Update(obj);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                if (!await ObjExists(id))
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            RecursoTarefa obj = await _unitOfWork.RecursoTarefaRepository.GetAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            await _unitOfWork.RecursoTarefaRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ObjExists(Guid id)
        {
            return await _unitOfWork.RecursoTarefaRepository.GetAsync(id) != null;
        }
    }
}
