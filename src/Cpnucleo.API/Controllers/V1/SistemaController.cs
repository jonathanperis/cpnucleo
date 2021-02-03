using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cpnucleo.API.Controllers.V1
{
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1", Deprecated = true)]
    [Authorize]
    public class SistemaController : ControllerBase
    {
        private readonly ICrudService<Sistema> _sistemaService;

        public SistemaController(ICrudService<Sistema> sistemaService)
        {
            _sistemaService = sistemaService;
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Sistema> Get()
        {
            return _sistemaService.Listar();
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Sistema> Get(Guid id)
        {
            Sistema sistema = _sistemaService.Consultar(id);

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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<Sistema> Post([FromBody]Sistema obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _sistemaService.Incluir(obj);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]Sistema obj)
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
                _sistemaService.Alterar(obj);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            Sistema obj = _sistemaService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _sistemaService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _sistemaService.Consultar(id) != null;
        }
    }
}
