using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces
{
    public interface IRecursoGrpcService : ICrudGrpcService<RecursoViewModel>
    {
        Task<RecursoViewModel> AutenticarAsync(string login, string senha);
    }
}
