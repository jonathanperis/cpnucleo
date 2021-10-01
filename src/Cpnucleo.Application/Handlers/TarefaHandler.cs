using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateByWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;

namespace Cpnucleo.Application.Handlers;

public class TarefaHandler :
    IAsyncRequestHandler<CreateTarefaCommand, CreateTarefaResponse>,
    IAsyncRequestHandler<GetTarefaQuery, GetTarefaResponse>,
    IAsyncRequestHandler<ListTarefaQuery, ListTarefaResponse>,
    IAsyncRequestHandler<RemoveTarefaCommand, RemoveTarefaResponse>,
    IAsyncRequestHandler<UpdateTarefaCommand, UpdateTarefaResponse>,
    IAsyncRequestHandler<GetByRecursoQuery, GetByRecursoResponse>,
    IAsyncRequestHandler<UpdateByWorkflowCommand, UpdateByWorkflowResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<CreateTarefaResponse> InvokeAsync(CreateTarefaCommand request, CancellationToken cancellationToken)
    {
        CreateTarefaResponse result = new CreateTarefaResponse
        {
            Status = OperationResult.Failed
        };

        Tarefa obj = await _unitOfWork.TarefaRepository.AddAsync(_mapper.Map<Tarefa>(request.Tarefa));
        result.Tarefa = _mapper.Map<TarefaViewModel>(obj);

        await _unitOfWork.SaveChangesAsync();

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<GetTarefaResponse> InvokeAsync(GetTarefaQuery request, CancellationToken cancellationToken)
    {
        GetTarefaResponse result = new GetTarefaResponse
        {
            Status = OperationResult.Failed
        };

        result.Tarefa = _mapper.Map<TarefaViewModel>(await _unitOfWork.TarefaRepository.GetAsync(request.Id));

        if (result.Tarefa == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<ListTarefaResponse> InvokeAsync(ListTarefaQuery request, CancellationToken cancellationToken)
    {
        ListTarefaResponse result = new ListTarefaResponse
        {
            Status = OperationResult.Failed
        };

        result.Tarefas = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.AllAsync(request.GetDependencies));
        result.Status = OperationResult.Success;

        await PreencherDadosAdicionaisAsync(result.Tarefas);

        return result;
    }

    public async ValueTask<RemoveTarefaResponse> InvokeAsync(RemoveTarefaCommand request, CancellationToken cancellationToken)
    {
        RemoveTarefaResponse result = new RemoveTarefaResponse
        {
            Status = OperationResult.Failed
        };

        Tarefa obj = await _unitOfWork.TarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        await _unitOfWork.TarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<UpdateTarefaResponse> InvokeAsync(UpdateTarefaCommand request, CancellationToken cancellationToken)
    {
        UpdateTarefaResponse result = new UpdateTarefaResponse
        {
            Status = OperationResult.Failed
        };

        _unitOfWork.TarefaRepository.Update(_mapper.Map<Tarefa>(request.Tarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<GetByRecursoResponse> InvokeAsync(GetByRecursoQuery request, CancellationToken cancellationToken)
    {
        GetByRecursoResponse result = new GetByRecursoResponse
        {
            Status = OperationResult.Failed
        };

        result.Tarefas = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.GetByRecursoAsync(request.IdRecurso));
        result.Status = OperationResult.Success;

        await PreencherDadosAdicionaisAsync(result.Tarefas);

        return result;
    }

    public async ValueTask<UpdateByWorkflowResponse> InvokeAsync(UpdateByWorkflowCommand request, CancellationToken cancellationToken)
    {
        UpdateByWorkflowResponse result = new UpdateByWorkflowResponse
        {
            Status = OperationResult.Failed
        };

        Tarefa tarefa = await _unitOfWork.TarefaRepository.GetAsync(request.IdTarefa);

        tarefa.IdWorkflow = request.Workflow.Id;
        tarefa.Workflow = _mapper.Map<Workflow>(request.Workflow); //TODO: - VERIFICAR NECESSIDADE.

        _unitOfWork.TarefaRepository.Update(tarefa);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    private async Task PreencherDadosAdicionaisAsync(IEnumerable<TarefaViewModel> lista)
    {
        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        foreach (TarefaViewModel item in lista)
        {
            item.Workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);

            item.HorasConsumidas = await _unitOfWork.ApontamentoRepository.GetTotalHorasByRecursoAsync(item.IdRecurso, item.Id);
            item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

            IEnumerable<ImpedimentoTarefa> impedimentos = await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(item.Id);

            if (impedimentos.Any())
            {
                item.TipoTarefa.Element = "warning-element";
            }
            else if (DateTime.Now.Date >= item.DataInicio && DateTime.Now.Date <= item.DataTermino)
            {
                item.TipoTarefa.Element = "success-element";
            }
            else if (DateTime.Now.Date > item.DataTermino)
            {
                item.TipoTarefa.Element = "danger-element";
            }
            else
            {
                item.TipoTarefa.Element = "info-element";
            }
        }
    }
}
