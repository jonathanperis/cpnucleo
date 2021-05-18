using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.Extensions.Configuration;

namespace Cpnucleo.RazorPages.Services
{
    public abstract class GrpcService
    {
        protected GrpcChannel _channel;
        protected readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public GrpcService(IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _configuration = configuration;
        }  

        protected GrpcChannel CreateAuthenticatedChannel(string token)
        {
            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(token))
                {
                    metadata.Add("Authorization", $"Bearer {token}");
                }

                return Task.CompletedTask;
            });

            //@@JONATHAN - 08/02/2020 - PALIATIVO APENAS PARA CHAMADAS SEM CERTIFICADO: BEGIN

            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            //@@JONATHAN - 08/02/2020 - PALIATIVO APENAS PARA CHAMADAS SEM CERTIFICADO: END                 

            _channel = GrpcChannel.ForAddress(_configuration["AppSettings:UrlCpnucleoGrpc"], 
                new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(httpHandler), //@@JONATHAN - 08/02/2020 - PALIATIVO APENAS PARA CHAMADAS SEM CERTIFICADO
                    Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
                });    

            return _channel;        
        }        
    }
}