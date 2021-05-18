using AutoMapper;
using Cpnucleo.RazorPages.Protos;
using Cpnucleo.RazorPages.Protos.RecursoTarefaProto;
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
    internal class RecursoTarefaService : GrpcService, IRecursoTarefaService
    {
        private RecursoTarefaProto.RecursoTarefaProtoClient _client;
        
        public RecursoTarefaService(IMapper mapper, IConfiguration configuration)
            : base(mapper, configuration)
        {

        }        

        public async Task<(IEnumerable<RecursoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarAsync(string token, bool getDependencies = false)
        {
            _client = InitializeAuthenticatedChannel(token);

            ListarReply response = await _client.ListarAsync(new Empty());
            var result =  _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(response.Lista);

            return (result, true, HttpStatusCode.OK, "");
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> ConsultarAsync(string token, Guid id)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            var result = _mapper.Map<RecursoTarefaViewModel>(await _client.ConsultarAsync(request));

            return (result, true, HttpStatusCode.OK, "");
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> IncluirAsync(string token, object value)
        {
            _client = InitializeAuthenticatedChannel(token);

            RecursoTarefaModel reply = await _client.IncluirAsync(_mapper.Map<RecursoTarefaModel>(value));

            HttpStatusCode statusCode = reply.Id != string.Empty ? HttpStatusCode.Created : HttpStatusCode.BadRequest;
            bool sucesso = reply.Id != string.Empty ? true : false;

            return (_mapper.Map<RecursoTarefaViewModel>(reply), sucesso, statusCode, "");
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> AlterarAsync(string token, Guid id, object value)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseReply reply = await _client.AlterarAsync(_mapper.Map<RecursoTarefaModel>(value));

            HttpStatusCode statusCode = reply.Sucesso ? HttpStatusCode.NoContent : HttpStatusCode.BadRequest;

            return (null, reply.Sucesso, statusCode, "");
        }

        public async Task<(RecursoTarefaViewModel response, bool sucess, HttpStatusCode code, string message)> RemoverAsync(string token, Guid id)
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

        public async Task<(IEnumerable<RecursoTarefaViewModel> response, bool sucess, HttpStatusCode code, string message)> ListarPorTarefaAsync(string token, Guid id)
        {
            _client = InitializeAuthenticatedChannel(token);

            BaseRequest request = new BaseRequest
            {
                Id = id.ToString()
            };

            ListarPorTarefaReply response = await _client.ListarPorTarefaAsync(request);
            var result =  _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(response.Lista);

            return (result, true, HttpStatusCode.OK, "");
        }

        private RecursoTarefaProto.RecursoTarefaProtoClient InitializeAuthenticatedChannel(string token)
        {
            _channel = CreateAuthenticatedChannel(token);
            return new RecursoTarefaProto.RecursoTarefaProtoClient(_channel);        
        }        
    }
}