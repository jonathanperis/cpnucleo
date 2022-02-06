using Cpnucleo.Application.Commands.Projeto.CreateProjeto;
using Cpnucleo.Application.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Application.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Application.Queries.Projeto.GetProjeto;
using Cpnucleo.Application.Queries.Projeto.ListProjeto;

namespace Cpnucleo.Application.Interfaces;

public interface IProjetoGrpcService : IService<IProjetoGrpcService>
{
    UnaryResult<OperationResult> CreateProjeto(CreateProjetoCommand command);

    UnaryResult<OperationResult> UpdateProjeto(UpdateProjetoCommand command);

    UnaryResult<GetProjetoViewModel> GetProjeto(GetProjetoQuery query);

    UnaryResult<ListProjetoViewModel> ListProjeto(ListProjetoQuery query);

    UnaryResult<OperationResult> RemoveProjeto(RemoveProjetoCommand command);
}