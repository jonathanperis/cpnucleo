﻿using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.TipoTarefa;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    [ServiceContract]
    public interface ITipoTarefaGrpcService
    {
        [OperationContract]
        Task<CreateTipoTarefaResponse> AddAsync(CreateTipoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<UpdateTipoTarefaResponse> UpdateAsync(UpdateTipoTarefaCommand command, CallContext context = default);

        [OperationContract]
        Task<GetTipoTarefaResponse> GetAsync(GetTipoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<ListTipoTarefaResponse> AllAsync(ListTipoTarefaQuery query, CallContext context = default);

        [OperationContract]
        Task<RemoveTipoTarefaResponse> RemoveAsync(RemoveTipoTarefaCommand command, CallContext context = default);
    }
}
