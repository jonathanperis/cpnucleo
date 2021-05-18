using AutoMapper;
using Cpnucleo.RazorPages.Protos;
using Cpnucleo.RazorPages.Protos.RecursoProto;
using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class RecursoService : GrpcService, IRecursoService
    {
        private RecursoProto.RecursoProtoClient _client;
        
        public RecursoService(IMapper mapper, IConfiguration configuration)
            : base(mapper, configuration)
        {

        }        

        public async Task<(IEnumerable<RecursoViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            _client = InitializeAuthenticatedChannel(token);

            // ListarReply response = await _client.Listar(new Empty());
            // var result =  _mapper.Map<IEnumerable<RecursoViewModel>>(response.Lista);

            List<RecursoViewModel> result = new List<RecursoViewModel>();

            using AsyncServerStreamingCall<RecursoModel> reply = _client.Listar(new Empty());

            while (await reply.ResponseStream.MoveNext())
            {
                result.Add(_mapper.Map<RecursoViewModel>(reply.ResponseStream.Current));
            }            

            return (result, true, HttpStatusCode.OK, "");
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            var result = _mapper.Map<RecursoViewModel>(await _client.ConsultarAsync(request));

            return (result, true, HttpStatusCode.OK, "");
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            _client = InitializeAuthenticatedChannel(token);

            RecursoModel reply = await _client.IncluirAsync(_mapper.Map<RecursoModel>(value));

            HttpStatusCode statusCode = reply.Id != string.Empty ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            bool sucesso = reply.Id != string.Empty ? true : false;

            return (_mapper.Map<RecursoViewModel>(reply), sucesso, statusCode, "");
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseReply reply = await _client.AlterarAsync(_mapper.Map<RecursoModel>(value));

            HttpStatusCode statusCode = reply.Sucesso ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;

            return (null, reply.Sucesso, statusCode, "");
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            BaseReply reply = await _client.RemoverAsync(request);

            HttpStatusCode statusCode = reply.Sucesso ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;

            return (null, reply.Sucesso, statusCode, "");
        }

        public async Task<(RecursoViewModel response, bool sucess, HttpStatusCode code, string message)> AutenticarAsync(string username, string password)
        {            
            _client = InitializeAuthenticatedChannel("");

            AutenticarRequest request = new AutenticarRequest
            {
                Login = username,
                Senha = password
            };

            var result = _mapper.Map<RecursoViewModel>(await _client.AutenticarAsync(request));

            return (result, true, HttpStatusCode.OK, "");
        }

        private RecursoProto.RecursoProtoClient InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return new RecursoProto.RecursoProtoClient(_channel);        
        }        
    }
}