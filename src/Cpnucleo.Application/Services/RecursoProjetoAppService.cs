using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class RecursoProjetoAppService : CrudAppService<RecursoProjeto, RecursoProjetoViewModel>, IRecursoProjetoAppService
    {
        private readonly IRecursoProjetoRepository _recursoProjetoRepository;

        public RecursoProjetoAppService(IMapper mapper, ICrudRepository<RecursoProjeto> repository, IUnitOfWork unitOfWork, IRecursoProjetoRepository recursoProjetoRepository)
            : base(mapper, repository, unitOfWork)
        {
            _recursoProjetoRepository = recursoProjetoRepository;
        }

        public IEnumerable<RecursoProjetoViewModel> ListarPorProjeto(Guid idProjeto)
        {
            return _recursoProjetoRepository.ListarPorProjeto(idProjeto).ProjectTo<RecursoProjetoViewModel>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
