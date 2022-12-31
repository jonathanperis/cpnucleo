namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ApontamentoGrpcService : ServiceBase<IApontamentoGrpcService>, IApontamentoGrpcService
{
    private readonly IMediator _mediator;

    public ApontamentoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Incluir apontamento
    /// </summary>
    /// <remarks>
    /// # Incluir apontamento
    /// 
    /// Inclui um apontamento na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> CreateApontamento(CreateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Listar apontamentos
    /// </summary>
    /// <remarks>
    /// # Listar apontamentos
    /// 
    /// Lista apontamentos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListApontamentoViewModel> ListApontamento(ListApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar apontamento
    /// </summary>
    /// <remarks>
    /// # Consultar apontamento
    /// 
    /// Consulta um apontamento na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetApontamentoViewModel> GetApontamento(GetApontamentoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar apontamento por recurso
    /// </summary>
    /// <remarks>
    /// # Consultar apontamento por recurso
    /// 
    /// Consulta um apontamento por recurso na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListApontamentoByRecursoViewModel> GetApontamentoByRecurso(ListApontamentoByRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Remover apontamento
    /// </summary>
    /// <remarks>
    /// # Remover apontamento
    /// 
    /// Remove um apontamento da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> RemoveApontamento(RemoveApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar apontamento
    /// </summary>
    /// <remarks>
    /// # Alterar apontamento
    /// 
    /// Altera um apontamento na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateApontamento(UpdateApontamentoCommand command)
    {
        return await _mediator.Send(command);
    }
}
