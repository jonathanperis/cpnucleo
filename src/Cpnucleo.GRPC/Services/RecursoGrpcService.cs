using Cpnucleo.Shared.Commands.CreateRecurso;
using Cpnucleo.Shared.Commands.RemoveRecurso;
using Cpnucleo.Shared.Commands.UpdateRecurso;
using Cpnucleo.Shared.Queries.GetRecurso;
using Cpnucleo.Shared.Queries.ListRecurso;

namespace Cpnucleo.GRPC.Services;

[Authorize]
public class RecursoGrpcService : ServiceBase<IRecursoGrpcService>, IRecursoGrpcService
{
    private readonly IMediator _mediator;

    public RecursoGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Incluir recurso
    /// </summary>
    /// <remarks>
    /// # Incluir recurso
    /// 
    /// Inclui um recurso na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> CreateRecurso(CreateRecursoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Listar recursos
    /// </summary>
    /// <remarks>
    /// # Listar recursos
    /// 
    /// Lista recursos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListRecursoViewModel> ListRecurso(ListRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Consultar recurso
    /// </summary>
    /// <remarks>
    /// # Consultar recurso
    /// 
    /// Consulta um recurso na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetRecursoViewModel> GetRecurso(GetRecursoQuery query)
    {
        return await _mediator.Send(query);
    }

    /// <summary>
    /// Remover recurso
    /// </summary>
    /// <remarks>
    /// # Remover recurso
    /// 
    /// Remove um recurso da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> RemoveRecurso(RemoveRecursoCommand command)
    {
        return await _mediator.Send(command);
    }

    /// <summary>
    /// Alterar recurso
    /// </summary>
    /// <remarks>
    /// # Alterar recurso
    /// 
    /// Altera um recurso na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateRecurso(UpdateRecursoCommand command)
    {
        return await _mediator.Send(command);
    }
}
