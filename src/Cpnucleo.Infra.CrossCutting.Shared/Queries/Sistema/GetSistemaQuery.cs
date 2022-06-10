namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Sistema;

public class GetSistemaQuery : BaseQuery, IRequest<GetSistemaViewModel>
{
    public Guid Id { get; set; }
}
