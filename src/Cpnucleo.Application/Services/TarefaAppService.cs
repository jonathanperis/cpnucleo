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

        public bool AlterarPorPercentualConcluido(Guid idTarefa, int? percentualConcluido)
        {
            TarefaViewModel tarefa = Consultar(idTarefa);
            tarefa.PercentualConcluido = percentualConcluido;

            return Alterar(tarefa);
        }

        public bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow)
        {
            lock (this)
            {
                TarefaViewModel tarefa = Consultar(idTarefa);
                tarefa.IdWorkflow = idWorkflow;

                return Alterar(tarefa);
            }
        }
    }
}
