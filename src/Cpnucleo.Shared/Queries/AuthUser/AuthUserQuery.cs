namespace Cpnucleo.Shared.Queries.AuthUser;

public sealed record AuthUserQuery(string Usuario, string Senha) : BaseQuery, IRequest<AuthUserViewModel>;
