using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Services
{
    public abstract class GrpcService
    {
        protected GrpcChannel _channel;
        private readonly IConfiguration _configuration;

        public GrpcService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected GrpcChannel CreateAuthenticatedChannel(string token)
        {
            CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(token))
                {
                    metadata.Add("Authorization", $"Bearer {token}");
                }

                return Task.CompletedTask;
            });

            //@@JONATHAN - 08/02/2020 - PALIATIVO APENAS PARA CHAMADAS SEM CERTIFICADO: BEGIN

            //var httpHandler = new HttpClientHandler
            //{
            //    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            //};

            //@@JONATHAN - 08/02/2020 - PALIATIVO APENAS PARA CHAMADAS SEM CERTIFICADO: END                 

            _channel = GrpcChannel.ForAddress(_configuration["AppSettings:UrlCpnucleoGrpc"],
                new GrpcChannelOptions
                {
                    //HttpHandler = new GrpcWebHandler(httpHandler), //@@JONATHAN - 08/02/2020 - PALIATIVO APENAS PARA CHAMADAS SEM CERTIFICADO
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                });

            return _channel;
        }
    }
}