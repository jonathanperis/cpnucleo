namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;

public class GetSistemaQuery : IRequest<GetSistemaViewModel>
{
    public Guid Id { get; set; }
}
