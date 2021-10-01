namespace Cpnucleo.API.Controllers.V2;

[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2")]
//[Authorize]
public class SistemaController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public SistemaController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Listar sistemas
    /// </summary>
    /// <remarks>
    /// # Listar sistemas
    /// 
    /// Lista sistemas da base de dados.
    /// </remarks>
    /// <param name="getDependencies">Listar dependências do objeto</param>        
    /// <response code="200">Retorna uma lista de sistemas</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Sistema>> Get(bool getDependencies = false)
    {
        return await _unitOfWork.SistemaRepository.AllAsync(getDependencies);
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
    [HttpGet("{id}", Name = "GetSistema")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Sistema>> Get(Guid id)
    {
        Sistema sistema = await _unitOfWork.SistemaRepository.GetAsync(id);

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
    [ProducesResponseType(typeof(Sistema), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Sistema>> Post([FromBody] Sistema obj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            obj = await _unitOfWork.SistemaRepository.AddAsync(obj);

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

        return CreatedAtRoute("GetSistema", new { id = obj.Id }, obj);
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(Guid id, [FromBody] Sistema obj)
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
            _unitOfWork.SistemaRepository.Update(obj);

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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Sistema obj = await _unitOfWork.SistemaRepository.GetAsync(id);

        if (obj == null)
        {
            return NotFound();
        }

        await _unitOfWork.SistemaRepository.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ObjExists(Guid id)
    {
        return await _unitOfWork.SistemaRepository.GetAsync(id) != null;
    }
}
