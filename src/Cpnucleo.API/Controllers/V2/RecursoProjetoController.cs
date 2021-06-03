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
    public class RecursoProjetoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecursoProjetoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Listar recursos de projeto
        /// </summary>
        /// <remarks>
        /// # Listar recursos de projeto
        /// 
        /// Lista recursos de projeto da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de recursos de projeto</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<RecursoProjeto>> Get(bool getDependencies = false)
        {
            return await _unitOfWork.RecursoProjetoRepository.AllAsync(getDependencies);
        }

        /// <summary>
        /// Listar recursos de projeto por id recurso
        /// </summary>
        /// <remarks>
        /// # Listar recursos de projeto por id recurso
        /// 
        /// Lista recursos de projeto por id recurso na base de dados.
        /// </remarks>
        /// <param name="idRecurso">Id do recurso</param>        
        /// <response code="200">Retorna uma lista de recursos de projeto</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("GetByProjeto/{idRecurso}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<RecursoProjeto>> GetByProjeto(Guid idRecurso)
        {
            return await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(idRecurso);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetRecursoProjeto")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecursoProjeto>> Get(Guid id)
        {
            RecursoProjeto recursoProjeto = await _unitOfWork.RecursoProjetoRepository.GetAsync(id);

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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(RecursoProjeto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<RecursoProjeto>> Post([FromBody] RecursoProjeto obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                obj = await _unitOfWork.RecursoProjetoRepository.AddAsync(obj);

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

            return CreatedAtRoute("GetRecursoProjeto", new { id = obj.Id }, obj);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(Guid id, [FromBody] RecursoProjeto obj)
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
                _unitOfWork.RecursoProjetoRepository.Update(obj);

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
        /// Remover recurso de projeto
        /// </summary>
        /// <remarks>
        /// # Remover recurso de projeto
        /// 
        /// Remove um recurso de projeto da base de dados.
        /// </remarks>
        /// <param name="id">Id do recurso de projeto</param>        
        /// <response code="204">Recurso de projeto removido com sucesso</response>
        /// <response code="404">Recurso de projeto não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            RecursoProjeto obj = await _unitOfWork.RecursoProjetoRepository.GetAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            await _unitOfWork.RecursoProjetoRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ObjExists(Guid id)
        {
            return await _unitOfWork.RecursoProjetoRepository.GetAsync(id) != null;
        }
    }
}
