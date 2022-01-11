using Cpnucleo.MVC.Services;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Security.Claims;

namespace Cpnucleo.MVC.Controllers;

public class BaseController : Controller
{
    protected GrpcChannel _channel;

    public string Token => ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.Hash);

    protected GrpcChannel CreateAuthenticatedChannel(string grpcAddress)
    {
        CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
        {
            if (!string.IsNullOrEmpty(Token))
            {
                metadata.Add("Authorization", $"Bearer {Token}");
            }

            return Task.CompletedTask;
        });

        GrpcWebHandler handler = new(GrpcWebMode.GrpcWeb,
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });

        handler.HttpVersion = new Version(1, 1);

        _channel = GrpcChannel.ForAddress(grpcAddress,
            new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials),
                HttpClient = new HttpClient(handler)
            });

        return _channel;
    }
}
