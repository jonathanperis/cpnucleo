using AutoMapper;
using Grpc.Net.Client;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Services
{
    public abstract class BaseGrpcService
    {
        protected readonly IMapper _mapper;
        protected readonly GrpcChannel _channel;

        public BaseGrpcService(IMapper mapper)
        {
            _mapper = mapper;
            _channel = GrpcChannel.ForAddress("https://localhost:5001");

            //GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
            //_channel = GrpcChannel.ForAddress("https://cpnucleo-grpc.azurewebsites.net", new GrpcChannelOptions
            //{
            //    HttpClient = new HttpClient(handler)
            //});
        }
    }
}
