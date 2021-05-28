using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    [ServiceContract]
    public interface IGenericGrpcService<TViewModel>
    {
        [OperationContract]
        Task<TViewModel> AddAsync(TViewModel viewModel, CallContext context = default);

        [OperationContract]
        void Update(TViewModel viewModel, CallContext context = default);

        [OperationContract]
        Task<TViewModel> GetAsync(string id, CallContext context = default);

        [OperationContract]
        Task<IEnumerable<TViewModel>> AllAsync(IEnumerable<SistemaViewModel> viewModel, CallContext context = default);

        [OperationContract]
        Task RemoveAsync(string id, CallContext context = default);
    }
}
