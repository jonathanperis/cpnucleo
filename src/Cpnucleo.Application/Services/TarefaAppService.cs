using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Application.Services
{
    public class TarefaAppService : CrudAppService<Tarefa, TarefaViewModel>, ITarefaAppService
    {
        public TarefaAppService(IMapper mapper, ICrudRepository<Tarefa> repository, IUnitOfWork unitOfWork)
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
