namespace Cpnucleo.Infra.CrossCutting.Util.Interfaces;

using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;

public interface IImpedimentoGrpcService : IService<IImpedimentoGrpcService>
{
    UnaryResult<CreateImpedimentoResponse> AddAsync(CreateImpedimentoCommand command);

    UnaryResult<UpdateImpedimentoResponse> UpdateAsync(UpdateImpedimentoCommand command);

    UnaryResult<GetImpedimentoResponse> GetAsync(GetImpedimentoQuery query);

    UnaryResult<ListImpedimentoResponse> AllAsync(ListImpedimentoQuery query);

    UnaryResult<RemoveImpedimentoResponse> RemoveAsync(RemoveImpedimentoCommand command);
}