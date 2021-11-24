using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

namespace Cpnucleo.Application.Handlers;

public class TarefaHandler :
    IRequestHandler<CreateTarefaCommand, OperationResult>,
    IRequestHandler<GetTarefaQuery, TarefaViewModel>,
    IRequestHandler<ListTarefaQuery, IEnumerable<TarefaViewModel>>,
    IRequestHandler<RemoveTarefaCommand, OperationResult>,
    IRequestHandler<UpdateTarefaCommand, OperationResult>,
    IRequestHandler<GetByRecursoQuery, IEnumerable<TarefaViewModel>>,
    IRequestHandler<UpdateByWorkflowCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.TarefaRepository.AddAsync(_mapper.Map<Tarefa>(request.Tarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<TarefaViewModel> Handle(GetTarefaQuery request, CancellationToken cancellationToken)
    {
        TarefaViewModel result = _mapper.Map<TarefaViewModel>(await _unitOfWork.TarefaRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<TarefaViewModel>> Handle(ListTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TarefaViewModel> result = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.AllAsync(request.GetDependencies));

        await PreencherDadosAdicionaisAsync(result);

        return result;
    }

    public async Task<OperationResult> Handle(RemoveTarefaCommand request, CancellationToken cancellationToken)
    {
        Tarefa obj = await _unitOfWork.TarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.TarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<OperationResult> Handle(UpdateTarefaCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.TarefaRepository.Update(_mapper.Map<Tarefa>(request.Tarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<IEnumerable<TarefaViewModel>> Handle(GetByRecursoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TarefaViewModel> result = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.GetByRecursoAsync(request.IdRecurso));

        await PreencherDadosAdicionaisAsync(result);

        return result;
    }

    public async Task<OperationResult> Handle(UpdateByWorkflowCommand request, CancellationToken cancellationToken)
    {
        Tarefa tarefa = await _unitOfWork.TarefaRepository.GetAsync(request.IdTarefa);

        tarefa.IdWorkflow = request.Workflow.Id;
        tarefa.Workflow = _mapper.Map<Workflow>(request.Workflow); //TODO: - VERIFICAR NECESSIDADE.

        _unitOfWork.TarefaRepository.Update(tarefa);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

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
