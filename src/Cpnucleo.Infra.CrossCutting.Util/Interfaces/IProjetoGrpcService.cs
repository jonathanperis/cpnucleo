using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using MagicOnion;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces
{
    public interface IProjetoGrpcService : IService<IProjetoGrpcService>
    {
        UnaryResult<CreateProjetoResponse> AddAsync(CreateProjetoCommand command);

        UnaryResult<UpdateProjetoResponse> UpdateAsync(UpdateProjetoCommand command);

        UnaryResult<GetProjetoResponse> GetAsync(GetProjetoQuery query);

        UnaryResult<ListProjetoResponse> AllAsync(ListProjetoQuery query);

        UnaryResult<RemoveProjetoResponse> RemoveAsync(RemoveProjetoCommand command);
    }
}
