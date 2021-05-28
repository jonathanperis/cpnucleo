using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.ServiceModel;

namespace Cpnucleo.Infra.CrossCutting.Util
{
    [ServiceContract]
    public interface ISistemaGrpcService : IGenericGrpcService<SistemaViewModel>
    {
    }
}
