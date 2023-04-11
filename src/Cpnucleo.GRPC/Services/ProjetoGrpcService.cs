namespace Cpnucleo.GRPC.Services;

[Authorize]
public class ProjetoGrpcService : ServiceBase<IProjetoGrpcService>, IProjetoGrpcService
{
    private readonly ISender _sender;

    public ProjetoGrpcService(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Incluir projeto
    /// </summary>
    /// <remarks>
    /// # Incluir projeto
    /// 
    /// Inclui um projeto na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> CreateProjeto(CreateProjetoCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Listar projetos
    /// </summary>
    /// <remarks>
    /// # Listar projetos
    /// 
    /// Lista projetos da base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<ListProjetoViewModel> ListProjeto(ListProjetoQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Consultar projeto
    /// </summary>
    /// <remarks>
    /// # Consultar projeto
    /// 
    /// Consulta um projeto na base de dados.
    /// </remarks>
    /// <param name="query">Objeto de consulta com os parametros necessários</param>        
    public async UnaryResult<GetProjetoViewModel> GetProjeto(GetProjetoQuery query)
    {
        return await _sender.Send(query);
    }

    /// <summary>
    /// Remover projeto
    /// </summary>
    /// <remarks>
    /// # Remover projeto
    /// 
    /// Remove um projeto da base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> RemoveProjeto(RemoveProjetoCommand command)
    {
        return await _sender.Send(command);
    }

    /// <summary>
    /// Alterar projeto
    /// </summary>
    /// <remarks>
    /// # Alterar projeto
    /// 
    /// Altera um projeto na base de dados.
    /// </remarks>
    /// <param name="command">Objeto de envio com os parametros necessários</param>        
    public async UnaryResult<OperationResult> UpdateProjeto(UpdateProjetoCommand command)
    {
        return await _sender.Send(command);
    }
}
