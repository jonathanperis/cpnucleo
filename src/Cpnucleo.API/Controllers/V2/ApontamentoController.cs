namespace Cpnucleo.API.Controllers.V2;

[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2", Deprecated = true)]
[Authorize]
public class ApontamentoController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ApontamentoController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Listar apontamentos
    /// </summary>
    /// <remarks>
    /// # Listar apontamentos
    /// 
    /// Lista apontamentos na base de dados.
    /// </remarks>
    /// <param name="getDependencies">Listar dependências do objeto</param>        
    /// <response code="200">Retorna uma lista de apontamentos</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Apontamento>> Get(bool getDependencies = false)
    {
        return await _unitOfWork.ApontamentoRepository.AllAsync(getDependencies);
    }

    /// <summary>
    /// Listar apontamentos por id recurso
    /// </summary>
    /// <remarks>
    /// # Listar apontamentos por id recurso
    /// 
    /// Lista apontamentos por id recurso da base de dados.
    /// </remarks>
    /// <param name="idRecurso">Id do recurso</param>        
    /// <response code="200">Retorna uma lista de apontamentos</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet("GetByRecurso/{idRecurso}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Apontamento>> GetByRecurso(Guid idRecurso)
    {
        return await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(idRecurso);
    }

    /// <summary>
    /// Consultar apontamento
    /// </summary>
    /// <remarks>
    /// # Consultar apontamento
    /// 
    /// Consulta um apontamento na base de dados.
    /// </remarks>
    /// <param name="id">Id do apontamento</param>        
    /// <response code="200">Retorna um apontamento</response>
    /// <response code="404">Apontamento não encontrado</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet("{id}", Name = "GetApontamento")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Apontamento>> Get(Guid id)
    {
        Apontamento apontamento = await _unitOfWork.ApontamentoRepository.GetAsync(id);

        if (apontamento == null)
        {
            return NotFound();
        }

        return Ok(apontamento);
    }

    /// <summary>
    /// Incluir apontamento
    /// </summary>
    /// <remarks>
    /// # Incluir apontamento
    /// 
    /// Inclui um apontamento na base de dados.
    /// 
    /// # Sample request:
    ///
    ///     POST /apontamento
    ///     {
    ///        "descricao": "Descrição do novo apontamento",
    ///        "dataApontamento": "2019-09-21T15:56:00.503Z",
    ///        "qtdHoras": 6,
    ///        "percentualConcluido": 10,
    ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
    ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91"
    ///     }
    /// </remarks>
    /// <param name="obj">Apontamento</param>        
    /// <response code="201">Apontamento cadastrado com sucesso</response>
    /// <response code="400">Objetos não preenchidos corretamente</response>
    /// <response code="409">Guid informado já consta na base de dados</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpPost]
    [ProducesResponseType(typeof(Apontamento), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Apontamento>> Post([FromBody] Apontamento obj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            obj = await _unitOfWork.ApontamentoRepository.AddAsync(obj);

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

        return CreatedAtRoute("GetApontamento", new { id = obj.Id }, obj);
    }

    /// <summary>
    /// Alterar apontamento
    /// </summary>
    /// <remarks>
    /// # Alterar apontamento
    /// 
    /// Altera um apontamento na base de dados.
    /// 
    /// # Sample request:
    ///
    ///     PUT /apontamento
    ///     {
    ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
    ///        "descricao": "Descrição do novo apontamento - alterado",
    ///        "dataApontamento": "2019-09-21T15:56:00.503Z",
    ///        "qtdHoras": 6,
    ///        "percentualConcluido": 10,
    ///        "idTarefa": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
    ///        "idRecurso": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
    ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
    ///     }
    /// </remarks>
    /// <param name="id">Id do apontamento</param>        
    /// <param name="obj">Apontamento</param>        
    /// <response code="204">Apontamento alterado com sucesso</response>
    /// <response code="400">ID informado não é válido</response>
    /// <response code="404">Atendimento não encontrado</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(Guid id, [FromBody] Apontamento obj)
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
            _unitOfWork.ApontamentoRepository.Update(obj);

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
    /// Remover apontamento
    /// </summary>
    /// <remarks>
    /// # Remover apontamento
    /// 
    /// Remove um apontamento da base de dados.
    /// </remarks>
    /// <param name="id">Id do apontamento</param>        
    /// <response code="204">Apontamento removido com sucesso</response>
    /// <response code="404">Apontamento não encontrado</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Apontamento obj = await _unitOfWork.ApontamentoRepository.GetAsync(id);

        if (obj == null)
        {
            return NotFound();
        }

        await _unitOfWork.ApontamentoRepository.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ObjExists(Guid id)
    {
        return await _unitOfWork.ApontamentoRepository.GetAsync(id) != null;
    }
}
