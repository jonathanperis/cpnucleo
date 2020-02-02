using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface ISistemaGrpcService
    {
        Task<bool> IncluirAsync(SistemaViewModel obj);

        Task<IEnumerable<SistemaViewModel>> ListarAsync();
    }
}
