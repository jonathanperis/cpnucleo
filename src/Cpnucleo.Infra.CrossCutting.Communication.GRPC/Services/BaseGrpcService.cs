using AutoMapper;
using Cpnucleo.GRPC;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        //public async Task<bool> IncluirAsync(TViewModel obj)
        //{
        //    BaseReply reply = await _client.IncluirAsync(_mapper.Map<TModel>(obj));

        //    return reply.Sucesso;
        //}

        //public async Task<TViewModel> ConsultarAsync(Guid id)
        //{
        //    BaseRequest request = new BaseRequest
        //    {
        //        Id = id.ToString()
        //    };

        //    return _mapper.Map<TViewModel>(await _client.ConsultarAsync(request));
        //}

        //public async Task<IEnumerable<TViewModel>> ListarAsync()
        //{
        //    List<TViewModel> result = new List<TViewModel>();

        //    //using (var reply = _client.Listar(new ListarRequest()))
        //    //{
        //    //    await foreach (var item in reply.ResponseStream.ReadAllAsync())
        //    //    {
        //    //        result.Add(_mapper.Map<SistemaViewModel>(item));
        //    //    }
        //    //}

        //    using (AsyncServerStreamingCall<SistemaModel> reply = _client.Listar(new Empty()))
        //    {
        //        while (await reply.ResponseStream.MoveNext())
        //        {
        //            result.Add(_mapper.Map<TViewModel>(reply.ResponseStream.Current));
        //        }
        //    }

        //    return result;
        //}

        //public async Task<bool> AlterarAsync(TViewModel obj)
        //{
        //    BaseReply reply = await _client.AlterarAsync(_mapper.Map<TModel>(obj));

        //    return reply.Sucesso;
        //}

        //public async Task<bool> RemoverAsync(Guid id)
        //{
        //    BaseRequest request = new BaseRequest
        //    {
        //        Id = id.ToString()
        //    };

        //    BaseReply reply = await _client.RemoverAsync(request);

        //    return reply.Sucesso;
        //}
    }
}
