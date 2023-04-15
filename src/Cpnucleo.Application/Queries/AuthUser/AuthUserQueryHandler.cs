namespace Cpnucleo.Application.Queries.AuthUser;

public sealed class AuthUserQueryHandler : IRequestHandler<AuthUserQuery, AuthUserViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthUserQueryHandler(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async ValueTask<AuthUserViewModel> Handle(AuthUserQuery request, CancellationToken cancellationToken)
    {
        AuthUserViewModel result = new()
        {
            Status = OperationResult.Failed
        };

        var recurso = await _context.Recursos
            .AsNoTracking()
            .Where(x => x.Login == request.Usuario && x.Ativo)
            .FirstOrDefaultAsync(cancellationToken);

        if (recurso is null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        bool success = Recurso.VerifyPassword(request.Senha, recurso.Senha!, recurso.Salt!);

        if (!success)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Recurso = recurso.MapToDto();
        result.Recurso.Senha = null;

        int.TryParse(_configuration["Jwt:Expires"], out int jwtExpires);
        result.Token = TokenService.GenerateToken(result.Recurso.Id.ToString(), _configuration["Jwt:Key"]!, _configuration["Jwt:Issuer"]!, jwtExpires);

        result.Status = OperationResult.Success;

        return result;
    }
}
