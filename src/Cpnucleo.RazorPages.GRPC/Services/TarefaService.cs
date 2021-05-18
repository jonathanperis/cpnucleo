using AutoMapper;
using Cpnucleo.RazorPages.Protos;
using Cpnucleo.RazorPages.Protos.TarefaProto;
using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Services
{
    internal class TarefaService : GrpcService, ITarefaService
    {
        private TarefaProto.TarefaProtoClient _client;
        
        public TarefaService(IMapper mapper, IConfiguration configuration)
            : base(mapper, configuration)
        {

        }        

        public async Task<(IEnumerable<TarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            _client = InitializeAuthenticatedChannel(token);

            ListarReply response = await _client.ListarAsync(new Empty());
            var result =  _mapper.Map<IEnumerable<TarefaViewModel>>(response.Lista);

            return (result, true, HttpStatusCode.OK, "");
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            var result = _mapper.Map<TarefaViewModel>(await _client.ConsultarAsync(request));

            return (result, true, HttpStatusCode.OK, "");
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            _client = InitializeAuthenticatedChannel(token);

            TarefaModel reply = await _client.IncluirAsync(_mapper.Map<TarefaModel>(value));

            HttpStatusCode statusCode = reply.Id != string.Empty ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            bool sucesso = reply.Id != string.Empty ? true : false;

            return (_mapper.Map<TarefaViewModel>(reply), sucesso, statusCode, "");
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseReply reply = await _client.AlterarAsync(_mapper.Map<TarefaModel>(value));

            HttpStatusCode statusCode = reply.Sucesso ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;

            return (null, reply.Sucesso, statusCode, "");
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
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

        public async Task<(IEnumerable<TarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorRecursoAsync(string token, Guid id)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            ListarPorRecursoReply response = await _client.ListarPorRecursoAsync(request);
            var result =  _mapper.Map<IEnumerable<TarefaViewModel>>(response.Lista);

            return (result, true, HttpStatusCode.OK, "");
        }

        public async Task<(TarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarPorWorkflowAsync(string token, Guid id, object value)
        {
            _client = InitializeAuthenticatedChannel(token);

            AlterarPorWorkflowRequest request = new AlterarPorWorkflowRequest
            {
                IdTarefa = id.ToString(),
                IdWorkflow = value.ToString()
            };

            BaseReply reply = await _client.AlterarPorWorkflowAsync(request);

            HttpStatusCode statusCode = reply.Sucesso ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;

            return (null, reply.Sucesso, statusCode, "");
        }

        private TarefaProto.TarefaProtoClient InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return new TarefaProto.TarefaProtoClient(_channel);        
        }        
    }
}