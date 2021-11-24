namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

public class GetSistemaQuery : BaseQuery, IRequest<SistemaViewModel>
{
    public Guid Id { get; set; }
}