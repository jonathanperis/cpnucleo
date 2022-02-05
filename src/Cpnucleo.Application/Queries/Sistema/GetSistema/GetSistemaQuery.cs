namespace Cpnucleo.Application.Queries.Sistema.GetSistema;

public class GetSistemaQuery : IRequest<GetSistemaViewModel>
{
    public Guid Id { get; set; }
}
