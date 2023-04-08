using Cpnucleo.Shared.Commands.CreateProjeto;
using Cpnucleo.Shared.Commands.RemoveProjeto;
using Cpnucleo.Shared.Commands.UpdateProjeto;
using Cpnucleo.Shared.Queries.GetProjeto;
using Cpnucleo.Shared.Queries.ListProjeto;

namespace Cpnucleo.Shared.Common.Interfaces;

public interface IProjetoGrpcService : IService<IProjetoGrpcService>
{
    UnaryResult<OperationResult> CreateProjeto(CreateProjetoCommand command);

    UnaryResult<OperationResult> UpdateProjeto(UpdateProjetoCommand command);

    UnaryResult<GetProjetoViewModel> GetProjeto(GetProjetoQuery query);

    UnaryResult<ListProjetoViewModel> ListProjeto(ListProjetoQuery query);

    UnaryResult<OperationResult> RemoveProjeto(RemoveProjetoCommand command);
}