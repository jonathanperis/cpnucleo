using Cpnucleo.MVC.Services;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected GrpcChannel _channel;
        private readonly IConfiguration _configuration;

        public BaseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Token => ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.Hash);

        protected GrpcChannel CreateAuthenticatedChannel()
        {
            CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
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
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                });

            return _channel;
        }
    }
}