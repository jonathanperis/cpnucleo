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
    public class ImpedimentoController : ControllerBase
    {
        private readonly ICrudAppService<ImpedimentoViewModel> _impedimentoAppService;

        public ImpedimentoController(ICrudAppService<ImpedimentoViewModel> impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        /// <summary>
        /// Listar impedimentos
        /// </summary>
        /// <remarks>
        /// # Listar impedimentos
        /// 
        /// Lista impedimentos da base de dados.
        /// </remarks>
        /// <param name="getDependencies">Listar dependências do objeto</param>        
        /// <response code="200">Retorna uma lista de impedimentos</response>
        /// <response code="401">Acesso não autorizado</response>
        /// <response code="500">Erro no processamento da requisição</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<ImpedimentoViewModel> Get(bool getDependencies = false)
        {
            return _impedimentoAppService.Listar(getDependencies);
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
        [HttpGet("{id}", Name = "GetImpedimento")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ImpedimentoViewModel> Get(Guid id)
        {
            ImpedimentoViewModel impedimento = _impedimentoAppService.Consultar(id);

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
        [ProducesResponseType(typeof(ImpedimentoViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ActionResult<ImpedimentoViewModel> Post([FromBody]ImpedimentoViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                obj.Id = _impedimentoAppService.Incluir(obj);
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

            return CreatedAtAction("GetImpedimento", new { id = obj.Id }, obj);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(Guid id, [FromBody]ImpedimentoViewModel obj)
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
                _impedimentoAppService.Alterar(obj);
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            ImpedimentoViewModel obj = _impedimentoAppService.Consultar(id);

            if (obj == null)
            {
                return NotFound();
            }

            _impedimentoAppService.Remover(id);

            return NoContent();
        }

        private bool ObjExists(Guid id)
        {
            return _impedimentoAppService.Consultar(id) != null;
        }
    }
}
