using Cpnucleo.MVC.Services;
using Grpc.Core;
using Grpc.Net.Client;
using System.Security.Claims;

namespace Cpnucleo.MVC.Controllers;

public class BaseController : Controller
{
    protected GrpcChannel _channel;

    private readonly IConfiguration _configuration;

    public string Token => ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.Hash);

    public BaseController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected GrpcChannel CreateAuthenticatedChannel()
    {
        var credentials = CallCredentials.FromInterceptor((context, metadata) =>
        {
            if (!string.IsNullOrEmpty(Token))
            {
                metadata.Add("Authorization", $"Bearer {Token}");
            }

            return Task.CompletedTask;
        });

        _channel = GrpcChannel.ForAddress(_configuration["AppSettings:UrlCpnucleoGrpc"],
            new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
            });

        return _channel;
    }
}
