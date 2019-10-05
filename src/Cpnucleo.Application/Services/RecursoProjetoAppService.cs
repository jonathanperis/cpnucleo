using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    public class RecursoProjetoAppService : CrudAppService<RecursoProjeto, RecursoProjetoViewModel>, IRecursoProjetoAppService
    {
        protected readonly IRecursoProjetoRepository _recursoProjetoRepository;

        public RecursoProjetoAppService(IMapper mapper, ICrudRepository<RecursoProjeto> repository, IUnitOfWork unitOfWork, IRecursoProjetoRepository recursoProjetoRepository)
            : base(mapper, repository, unitOfWork)
        {
            _recursoProjetoRepository = recursoProjetoRepository;
        }

        public IEnumerable<RecursoProjetoViewModel> ListarPorProjeto(Guid idProjeto)
        {
            return _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(_recursoProjetoRepository.ListarPorProjeto(idProjeto));
        }
    }
}
