using Cpnucleo.Application.Common.Context;
using Cpnucleo.Application.Common.Services;
using Cpnucleo.Shared.Queries.AuthUser;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.Application.Queries.AuthUser;

public sealed class AuthUserQueryHandler : IRequestHandler<AuthUserQuery, AuthUserViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthUserQueryHandler(IApplicationDbContext context, IMapper mapper, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<AuthUserViewModel> Handle(AuthUserQuery request, CancellationToken cancellationToken)
    {
        AuthUserViewModel result = new()
        {
            Status = OperationResult.Failed
        };

        var recurso = await _context.Recursos
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

        result.Recurso = _mapper.Map<RecursoDTO>(recurso);
        result.Recurso.Senha = null;

        int.TryParse(_configuration["Jwt:Expires"], out int jwtExpires);
        result.Token = TokenService.GenerateToken(result.Recurso.Id.ToString(), _configuration["Jwt:Key"]!, _configuration["Jwt:Issuer"]!, jwtExpires);

        result.Status = OperationResult.Success;

        return result;
    }
}
