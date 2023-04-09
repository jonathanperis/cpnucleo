namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ImpedimentoGrpcService : ServiceBase<IImpedimentoGrpcService>, IImpedimentoGrpcService
{
    private readonly IMediator _mediator;

    public ImpedimentoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Incluir impedimento
    /// </summary>
    /// <remarks>
    /// # Incluir impedimento
    /// 
    /// Inclui um impedimento na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> CreateImpedimento(CreateImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Listar impedimentos
    /// </summary>
    /// <remarks>
    /// # Listar impedimentos
    /// 
    /// Lista impedimentos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListImpedimentoViewModel> ListImpedimento(ListImpedimentoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar impedimento
    /// </summary>
    /// <remarks>
    /// # Consultar impedimento
    /// 
    /// Consulta um impedimento na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetImpedimentoViewModel> GetImpedimento(GetImpedimentoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Remover impedimento
    /// </summary>
    /// <remarks>
    /// # Remover impedimento
    /// 
    /// Remove um impedimento da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> RemoveImpedimento(RemoveImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar impedimento
    /// </summary>
    /// <remarks>
    /// # Alterar impedimento
    /// 
    /// Altera um impedimento na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateImpedimento(UpdateImpedimentoCommand command)
    {
        return await _mediator.Send(command);
    }
}
