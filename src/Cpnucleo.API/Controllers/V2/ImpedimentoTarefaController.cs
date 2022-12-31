namespace Cpnucleo.API.Controllers.V2;

[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2", Deprecated = true)]
//[Authorize]
public class ImpedimentoTarefaController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ImpedimentoTarefaController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Listar impedimentos de tarefa
    /// </summary>
    /// <remarks>
    /// # Listar impedimentos de tarefa
    /// 
    /// Lista impedimentos de tarefa da base de dados.
    /// </remarks>
    /// <param name="getDependencies">Listar dependências do objeto</param>        
    /// <response code="200">Retorna uma lista de impedimentos de tarefa</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ImpedimentoTarefa>> Get(bool getDependencies = false)
    {
        return await _unitOfWork.ImpedimentoTarefaRepository.List(getDependencies).ToListAsync();
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
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet("GetByTarefa/{idTarefa}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<List<ImpedimentoTarefa>> GetByTarefa(Guid idTarefa)
    {
        return await _unitOfWork.ImpedimentoTarefaRepository.ListImpedimentoTarefaByTarefa(idTarefa).ToListAsync();
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
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet("{id}", Name = "GetImpedimentoTarefa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ImpedimentoTarefa>> Get(Guid id)
    {
        ImpedimentoTarefa impedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.Get(id).FirstOrDefaultAsync();

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
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpPost]
    [ProducesResponseType(typeof(ImpedimentoTarefa), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ImpedimentoTarefa>> Post([FromBody] ImpedimentoTarefa obj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            obj = await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(obj);

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

        return CreatedAtRoute("GetImpedimentoTarefa", new { id = obj.Id }, obj);
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
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(Guid id, [FromBody] ImpedimentoTarefa obj)
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
            _unitOfWork.ImpedimentoTarefaRepository.Update(obj);

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
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        ImpedimentoTarefa obj = await _unitOfWork.ImpedimentoTarefaRepository.Get(id).FirstOrDefaultAsync();

        if (obj == null)
        {
            return NotFound();
        }

        await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ObjExists(Guid id)
    {
        return await _unitOfWork.ImpedimentoTarefaRepository.Get(id).FirstOrDefaultAsync() != null;
    }
}
