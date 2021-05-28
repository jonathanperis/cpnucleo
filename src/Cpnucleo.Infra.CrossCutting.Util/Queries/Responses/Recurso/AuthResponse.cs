using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso
{
    public class AuthResponse
    {
        public OperationResult Status { get; set; }
        public RecursoViewModel Recurso { get; set; }
    }
}
