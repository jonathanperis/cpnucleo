using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cpnucleo.API.Controllers.V2
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2")]
    [Authorize]
    public class TipoTarefaController : ControllerBase
    {
        private readonly ICrudAppService<TipoTarefaViewModel> _tipoTarefaAppService;

        public TipoTarefaController(ICrudAppService<TipoTarefaViewModel> tipoTarefaAppService)
        {
            _tipoTarefaAppService = tipoTarefaAppService;
        }

        /// <summary>
        /// Listar tipo de tarefas
        /// </summary>
        /// <remarks>
        /// # Listar tipos de Tarefas
        /// 
        /// Lista tipos de tarefas da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de tipos de tarefas</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<TipoTarefaViewModel> Get(bool getDependencies = false)
        {
            return _tipoTarefaAppService.Listar(getDependencies);
        }

        /// <summary>
        /// Consultar tipo de tarefa
        /// </summary>
        /// <remarks>
        /// # Consultar tipo de tarefa
        /// 
        /// Consulta um tipo de tarefa na base de dados.
        /// </remarks>
        /// <param name="id">Id do tipo de tarefa</param>        
        /// <response code="200">Retorna um tipo de tarefa</response>
        /// <response code="404">Tipo de tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}", Name = "GetTipoTarefa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TipoTarefaViewModel> Get(Guid id)
        {
            TipoTarefaViewModel tipoTarefa = _tipoTarefaAppService.Consultar(id);

            if (tipoTarefa == null)
            {
                return NotFound();
            }

            return Ok(tipoTarefa);
        }

        /// <summary>
        /// Incluir tipo de tarefa
        /// </summary>
        /// <remarks>
        /// # Incluir tipo de tarefa
        /// 
        /// Inclui um tipo de tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /tipoTarefa
        ///     {
        ///        "nome": "Novo tipoTarefa",
        ///        "descricao": "Descrição do novo tipoTarefa"
        ///     }
        /// </remarks>
        /// <param name="obj">Tipo de tarefa</param>        
        /// <response code="201">Tipo de tarefa cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(typeof(TipoTarefaViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<TipoTarefaViewModel> Post([FromBody]TipoTarefaViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                obj.Id = _tipoTarefaAppService.Incluir(obj);
            }
            catch (Exception)
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

            return CreatedAtAction("GetTipoTarefa", new { id = obj.Id }, obj);
        }

        /// <summary>
        /// Alterar tipo de tarefa
        /// </summary>
        /// <remarks>
        /// # Alterar tipo de tarefa
        /// 
        /// Altera um tipo de tarefa na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /tipoTarefa
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo tipoTarefa - alterado",
        ///        "descricao": "Descrição do novo tipoTarefa - alterado",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do tipo de tarefa</param>        
        /// <param name="obj">Tipo de tarefa</param>        
        /// <response code="204">Tipo de tarefa alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(Guid id, [FromBody]TipoTarefaViewModel obj)
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
                _tipoTarefaAppService.Alterar(obj);
            }
            catch (Exception)
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
        /// Remover tipo de tarefa
        /// </summary>
        /// <remarks>
        /// # Remover tipo de tarefa
        /// 
        /// Remove um tipo de tarefa da base de dados.
        /// </remarks>
        /// <param name="id">Id do tipo de tarefa</param>        
        /// <response code="204">Tipo de tarefa removido com sucesso</response>
        /// <response code="404">Tipo de tarefa não encontrado</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            TipoTarefaViewModel obj = _tipoTarefaAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _tipoTarefaAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _tipoTarefaAppService.Consultar(id) != null;
        }
    }
}
