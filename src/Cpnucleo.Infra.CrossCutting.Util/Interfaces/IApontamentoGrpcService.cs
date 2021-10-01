using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;

namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

public interface IApontamentoGrpcService : IService<IApontamentoGrpcService>
{
    UnaryResult<CreateApontamentoResponse> AddAsync(CreateApontamentoCommand command);

    UnaryResult<UpdateApontamentoResponse> UpdateAsync(UpdateApontamentoCommand command);

    UnaryResult<GetApontamentoResponse> GetAsync(GetApontamentoQuery query);

    UnaryResult<ListApontamentoResponse> AllAsync(ListApontamentoQuery query);

    UnaryResult<RemoveApontamentoResponse> RemoveAsync(RemoveApontamentoCommand command);

    UnaryResult<GetByRecursoResponse> GetByRecursoAsync(GetByRecursoQuery query);
}