using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;

namespace Cpnucleo.Application.Services
{
    public class RecursoAppService : AppService<Recurso, RecursoViewModel>, IRecursoAppService
    {
        public RecursoAppService(IMapper mapper, IRepository<Recurso> repository)
            : base(mapper, repository)
        {

        }

        public RecursoViewModel ValidarRecurso(string usuario, string senha, out bool valido)
        {
            throw new NotImplementedException();
        }
    }
}
