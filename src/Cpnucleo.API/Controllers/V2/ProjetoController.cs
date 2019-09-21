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
    public class ProjetoController : ControllerBase
    {
        private readonly IAppService<ProjetoViewModel> _projetoAppService;

        public ProjetoController(IAppService<ProjetoViewModel> projetoAppService)
        {
            _projetoAppService = projetoAppService;
        }

        /// <summary>
        /// Listar projetos
        /// </summary>
        /// <remarks>
        /// # Listar projetos
        /// 
        /// Lista projetos na base de dados.
        /// </remarks>
        /// <response code="200">Retorna uma lista de projetos</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<ProjetoViewModel> Get()
        {
            return _projetoAppService.Listar();
        }

        /// <summary>
        /// Consultar projeto
        /// </summary>
        /// <remarks>
        /// # Consultar projeto
        /// 
        /// Consulta um projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do projeto</param>        
        /// <response code="200">Retorna um projeto</response>
        /// <response code="404">Projeto não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<ProjetoViewModel> Get(Guid id)
        {
            ProjetoViewModel projeto = _projetoAppService.Consultar(id);

            if (projeto == null)
            {
                return NotFound();
            }

            return Ok(projeto);
        }

        /// <summary>
        /// Incluir projeto
        /// </summary>
        /// <remarks>
        /// # Incluir projeto
        /// 
        /// Inclui um projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     POST /projeto
        ///     {
        ///        "nome": "Novo projeto",
        ///        "idSistema": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
        ///     }
        /// </remarks>
        /// <param name="obj">projeto</param>        
        /// <response code="201">Projeto cadastrado com sucesso</response>
        /// <response code="400">Objetos não preenchidos corretamente</response>
        /// <response code="409">Guid informado já consta na base de dados</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<ProjetoViewModel> Post([FromBody]ProjetoViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _projetoAppService.Incluir(obj);
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
        /// Alterar projeto
        /// </summary>
        /// <remarks>
        /// # Alterar projeto
        /// 
        /// Altera um projeto na base de dados.
        /// 
        /// # Sample request:
        ///
        ///     PUT /projeto
        ///     {
        ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "nome": "Novo projeto - alterado",
        ///        "idSistema": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
        ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
        ///     }
        /// </remarks>
        /// <param name="id">Id do projeto</param>        
        /// <param name="obj">projeto</param>        
        /// <response code="204">Projeto alterado com sucesso</response>
        /// <response code="400">ID informado não é válido</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]ProjetoViewModel obj)
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
                _projetoAppService.Alterar(obj);
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
        /// Remover projeto
        /// </summary>
        /// <remarks>
        /// # Remover projeto
        /// 
        /// Remove um projeto na base de dados.
        /// </remarks>
        /// <param name="id">Id do projeto</param>        
        /// <response code="204">Projeto removido com sucesso</response>
        /// <response code="404">Projeto não encontrado</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            ProjetoViewModel obj = _projetoAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _projetoAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _projetoAppService.Consultar(id) != null;
        }
    }
}
