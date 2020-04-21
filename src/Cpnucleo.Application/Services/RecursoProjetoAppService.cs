using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    internal class RecursoProjetoAppService : CrudAppService<RecursoProjeto, RecursoProjetoViewModel>, IRecursoProjetoAppService
    {
        private readonly IRecursoProjetoService _recursoProjetoService;

        public RecursoProjetoAppService(IMapper mapper, IRecursoProjetoService recursoProjetoService)
            : base(mapper, recursoProjetoService)
        {
            _recursoProjetoService = recursoProjetoService;
        }

        public IEnumerable<RecursoProjetoViewModel> ListarPorProjeto(Guid idProjeto)
        {
            return _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(_recursoProjetoService.ListarPorProjeto(idProjeto));
        }
    }
}
