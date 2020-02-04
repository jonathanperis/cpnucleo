using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface IRecursoProjetoGrpcService : ICrudGrpcService<RecursoProjetoViewModel>
    {
        Task<IEnumerable<RecursoProjetoViewModel>> ListarPorProjetoAsync(Guid idProjeto);
    }
}
