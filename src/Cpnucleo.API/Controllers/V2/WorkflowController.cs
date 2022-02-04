namespace Cpnucleo.API.Controllers.V2;

[Produces("application/json")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2", Deprecated = true)]
[Authorize]
public class WorkflowController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public WorkflowController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Listar workflows
    /// </summary>
    /// <remarks>
    /// # Listar workflows
    /// 
    /// Lista workflows da base de dados.
    /// </remarks>
    /// <param name="getDependencies">Listar dependências do objeto</param>        
    /// <response code="200">Retorna uma lista de workflows</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Workflow>> Get(bool getDependencies = false)
    {
        IEnumerable<Workflow> result = await _unitOfWork.WorkflowRepository.AllAsync(getDependencies);

        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        foreach (Workflow item in result)
        {
            item.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
        }

        return result;
    }

    /// <summary>
    /// Consultar workflow
    /// </summary>
    /// <remarks>
    /// # Consultar workflow
    /// 
    /// Consulta um workflow na base de dados.
    /// </remarks>
    /// <param name="id">Id do workflow</param>        
    /// <response code="200">Retorna um workflow</response>
    /// <response code="404">Workflow não encontrado</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpGet("{id}", Name = "GetWorkflow")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Workflow>> Get(Guid id)
    {
        Workflow workflow = await _unitOfWork.WorkflowRepository.GetAsync(id);

        if (workflow == null)
        {
            return NotFound();
        }

        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);

        return Ok(workflow);
    }

    /// <summary>
    /// Incluir workflow
    /// </summary>
    /// <remarks>
    /// # Incluir workflow
    /// 
    /// Inclui um workflow na base de dados.
    /// 
    /// # Sample request:
    ///
    ///     POST /workflow
    ///     {
    ///        "nome": "Novo workflow",
    ///        "ordem": "3"
    ///     }
    /// </remarks>
    /// <param name="obj">Workflow</param>        
    /// <response code="201">Workflow cadastrado com sucesso</response>
    /// <response code="400">Objetos não preenchidos corretamente</response>
    /// <response code="409">Guid informado já consta na base de dados</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpPost]
    [ProducesResponseType(typeof(Workflow), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<Workflow>> Post([FromBody] Workflow obj)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            obj = await _unitOfWork.WorkflowRepository.AddAsync(obj);

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

        return CreatedAtRoute("GetWorkflow", new { id = obj.Id }, obj);
    }

    /// <summary>
    /// Alterar workflow
    /// </summary>
    /// <remarks>
    /// # Alterar workflow
    /// 
    /// Altera um workflow na base de dados.
    /// 
    /// # Sample request:
    ///
    ///     PUT /workflow
    ///     {
    ///        "id": "fffc0a28-b9e9-4ffd-0053-08d73d64fb91",
    ///        "nome": "Novo workflow - alterado",
    ///        "ordem": "3,
    ///        "dataInclusao": "2019-09-21T19:15:23.519Z"
    ///     }
    /// </remarks>
    /// <param name="id">Id do workflow</param>        
    /// <param name="obj">Workflow</param>        
    /// <response code="204">Workflow alterado com sucesso</response>
    /// <response code="400">ID informado não é válido</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(Guid id, [FromBody] Workflow obj)
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
            _unitOfWork.WorkflowRepository.Update(obj);

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
    /// Remover workflow
    /// </summary>
    /// <remarks>
    /// # Remover workflow
    /// 
    /// Remove um workflow da base de dados.
    /// </remarks>
    /// <param name="id">Id do workflow</param>        
    /// <response code="204">Workflow removido com sucesso</response>
    /// <response code="404">Workflow não encontrado</response>
    /// <response code="401">Acesso não autorizado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Workflow obj = await _unitOfWork.WorkflowRepository.GetAsync(id);

        if (obj == null)
        {
            return NotFound();
        }

        await _unitOfWork.WorkflowRepository.RemoveAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ObjExists(Guid id)
    {
        return await _unitOfWork.WorkflowRepository.GetAsync(id) != null;
    }
}
