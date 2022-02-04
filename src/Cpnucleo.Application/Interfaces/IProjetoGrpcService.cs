using Cpnucleo.Application.Commands.Projeto.CreateProjeto;
using Cpnucleo.Application.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Application.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Application.Queries.Projeto.GetProjeto;
using Cpnucleo.Application.Queries.Projeto.ListProjeto;

namespace Cpnucleo.Application.Interfaces;

public interface IProjetoGrpcService : IService<IProjetoGrpcService>
{
    UnaryResult<OperationResult> AddAsync(CreateProjetoCommand command);

    UnaryResult<OperationResult> UpdateAsync(UpdateProjetoCommand command);

    UnaryResult<GetProjetoViewModel> GetAsync(GetProjetoQuery query);

    UnaryResult<ListProjetoViewModel> AllAsync(ListProjetoQuery query);

    UnaryResult<OperationResult> RemoveAsync(RemoveProjetoCommand command);
}