using Cpnucleo.API.Services;
using Cpnucleo.Infra.CrossCutting.Util.Requests.Auth;

namespace Cpnucleo.API.Controllers.V2;

[Produces("application/json")]
[Route("api/v{version:apiVersion}")]
[ApiController]
[ApiVersion("1")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public AuthController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    /// <summary>
    /// Autenticar
    /// </summary>
    /// <remarks>
    /// # Autenticar
    /// 
    /// Autentica e devolve um token válido por 60 minutos para utilização na API.
    /// 
    /// # Sample request:
    ///
    ///     POST /auth
    ///     {
    ///       "auth": {
    ///         "usuario": "usuario.teste",
    ///         "senha": "12345678"
    ///       }
    ///     }
    /// </remarks>
    /// <param name="query">Recurso</param>
    /// <response code="200">Retorna um recurso</response>
    /// <response code="400">Objetos não preenchidos corretamente</response>
    /// <response code="404">Recurso não encontrado</response>
    /// <response code="500">Erro no processamento da requisição</response>
    [HttpPost("Auth")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuthResponse>> Auth([FromBody] AuthRequest query)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        AuthResponse response = await _mediator.Send(query);

        if (response.Recurso == null)
        {
            return NotFound();
        }

        if (response.Status == OperationResult.Success)
        {
            int.TryParse(_configuration["Jwt:Expires"], out int jwtExpires);

            response.Recurso.Token = TokenService.GenerateToken(response.Recurso.Id.ToString(), _configuration["Jwt:Key"], _configuration["Jwt:Issuer"], jwtExpires);
        }

        return Ok(response);
    }
}
