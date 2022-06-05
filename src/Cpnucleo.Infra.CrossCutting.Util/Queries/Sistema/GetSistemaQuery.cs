namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

public class GetSistemaQuery : BaseQuery, IRequest<GetSistemaViewModel>
{
    public Guid Id { get; set; }
}
