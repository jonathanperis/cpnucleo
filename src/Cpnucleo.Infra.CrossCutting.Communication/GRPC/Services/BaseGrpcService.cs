using AutoMapper;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Grpc.Net.Client;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    internal abstract class BaseGrpcService
    {
        protected readonly IMapper _mapper;
        protected readonly GrpcChannel _channel;

        public BaseGrpcService(IMapper mapper, ISystemConfiguration systemConfiguration)
        {
            _mapper = mapper;
            _channel = GrpcChannel.ForAddress(systemConfiguration.UrlCpnucleoGrpc);

            //GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
            //_channel = GrpcChannel.ForAddress(systemConfiguration.UrlCpnucleoGrpc, new GrpcChannelOptions
            //{
            //    HttpClient = new HttpClient(handler)
            //});
        }
    }
}
