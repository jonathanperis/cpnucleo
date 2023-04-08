﻿using Cpnucleo.Application.Common.Context;
using Cpnucleo.Domain.Constants;
using Cpnucleo.Shared.Queries.ListTarefaByRecurso;

namespace Cpnucleo.Application.Queries.ListTarefaByRecurso;

public sealed class ListTarefaByRecursoQueryHandler : IRequestHandler<ListTarefaByRecursoQuery, ListTarefaByRecursoViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListTarefaByRecursoQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListTarefaByRecursoViewModel> Handle(ListTarefaByRecursoQuery request, CancellationToken cancellationToken)
    {
        var tarefas = await _context.Tarefas
            .Where(x => x.IdRecurso == request.IdRecurso && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<TarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (tarefas is null)
        {
            return new ListTarefaByRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        await PreencherDadosAdicionaisAsync(tarefas, cancellationToken);

        return new ListTarefaByRecursoViewModel { Tarefas = tarefas, OperationResult = OperationResult.Success };
    }

    private async Task PreencherDadosAdicionaisAsync(List<TarefaDTO> lista, CancellationToken cancellationToken)
    {
        var colunas = _context.Workflows.Where(x => x.Ativo).Count();

        foreach (TarefaDTO item in lista)
        {
            item.Workflow.TamanhoColuna = Workflow.GetTamanhoColuna(colunas);

            item.HorasConsumidas = _context.Apontamentos
                .Where(x => x.IdRecurso == item.IdRecurso && x.IdTarefa == item.Id && x.Ativo)
                .Sum(x => x.QtdHoras);

            item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

            var impedimentos = await _context.ImpedimentoTarefas
                .Where(x => x.IdTarefa == item.Id && x.Ativo)
                .OrderBy(x => x.DataInclusao)
                .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (impedimentos.Any())
            {
                item.TipoTarefa.Element = TipoTarefaConstants.WarningElement;
            }
            else if (DateTime.UtcNow.Date >= item.DataInicio && DateTime.UtcNow.Date <= item.DataTermino)
            {
                item.TipoTarefa.Element = TipoTarefaConstants.SuccessElement;
            }
            else if (DateTime.UtcNow.Date > item.DataTermino)
            {
                item.TipoTarefa.Element = TipoTarefaConstants.DangerElement;
            }
            else
            {
                item.TipoTarefa.Element = TipoTarefaConstants.InfoElement;
            }
        }
    }
}