using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface ICrudGrpcService<TViewModel>
    {
        Task<bool> IncluirAsync(TViewModel obj);

        Task<TViewModel> ConsultarAsync(Guid id);

        Task<IEnumerable<TViewModel>> ListarAsync();

        Task<bool> AlterarAsync(TViewModel obj);

        Task<bool> RemoverAsync(Guid id);
    }
}
