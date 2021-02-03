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
    public class ImpedimentoController : ControllerBase
    {
        private readonly ICrudService<Impedimento> _impedimentoService;

        public ImpedimentoController(ICrudService<Impedimento> impedimentoService)
        {
            _impedimentoService = impedimentoService;
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public IEnumerable<Impedimento> Get()
        {
            return _impedimentoService.Listar();
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Impedimento> Get(Guid id)
        {
            Impedimento impedimento = _impedimentoService.Consultar(id);

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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public ActionResult<Impedimento> Post([FromBody]Impedimento obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _impedimentoService.Incluir(obj);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Put(Guid id, [FromBody]Impedimento obj)
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
                _impedimentoService.Alterar(obj);
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
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(Guid id)
        {
            Impedimento obj = _impedimentoService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _impedimentoService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _impedimentoService.Consultar(id) != null;
        }
    }
}
