using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class RecursoProjetoAppService : CrudAppService<RecursoProjeto, RecursoProjetoViewModel>, IRecursoProjetoAppService
    {
        private readonly IRecursoProjetoService _recursoProjetoService;

        public RecursoProjetoAppService(IMapper mapper, IRecursoProjetoService recursoProjetoService)
            : base(mapper, recursoProjetoService)
        {
            _recursoProjetoService = recursoProjetoService;
        }

        public IEnumerable<RecursoProjetoViewModel> ListarPorProjeto(Guid idProjeto)
        {
            return _recursoProjetoService.ListarPorProjeto(idProjeto).ProjectTo<RecursoProjetoViewModel>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
