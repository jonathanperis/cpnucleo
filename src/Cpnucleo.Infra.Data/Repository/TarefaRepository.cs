using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;

namespace Cpnucleo.Infra.Data.Repository
{
    public class TarefaRepository : Repository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public void AlterarPorFluxoTrabalho(Guid idTarefa, Guid idWorkflow)
        {
            throw new NotImplementedException();
        }
    }
}
