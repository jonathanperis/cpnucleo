using System;
using System.Linq;
using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;

namespace Cpnucleo.Application.Services
{
    public class ApontamentoAppService : AppService<Apontamento, ApontamentoViewModel>, IApontamentoAppService
    {
        public ApontamentoAppService(IMapper mapper, IRepository<Apontamento> repository)
            : base(mapper, repository)
        {

        }

        public void ApontarHoras(ApontamentoViewModel apontamento)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ApontamentoViewModel> ListarPoridRecurso(Guid idRecurso)
        {
            throw new System.NotImplementedException();
        }

        public int ObterTotalHorasPoridRecurso(Guid idRecurso, Guid idTarefa)
        {
            throw new System.NotImplementedException();
        }
    }
}
