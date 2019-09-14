using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;

namespace Cpnucleo.Application.Services
{
    public class TarefaAppService : AppService<Tarefa, TarefaViewModel>, ITarefaAppService
    {
        public TarefaAppService(IMapper mapper, IRepository<Tarefa> repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {

        }

        public bool AlterarPorApontamento(Guid idTarefa, int? percentualConcluido)
        {
            var tarefa = Consultar(idTarefa);
            tarefa.PercentualConcluido = percentualConcluido;

            return Alterar(tarefa);
        }

        public bool AlterarPorFluxoTrabalho(Guid idTarefa, Guid idWorkflow)
        {
            lock (this)
            {
                var tarefa = Consultar(idTarefa);
                tarefa.IdWorkflow = idWorkflow;

                return Alterar(tarefa);
            }
        }
    }
}
