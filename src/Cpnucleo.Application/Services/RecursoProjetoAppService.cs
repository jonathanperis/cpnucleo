using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    public class RecursoProjetoAppService : AppService<RecursoProjeto, RecursoProjetoViewModel>, IRecursoProjetoAppService
    {
        protected readonly IRecursoProjetoRepository _recursoProjetoRepository;

        public RecursoProjetoAppService(IMapper mapper, IRepository<RecursoProjeto> repository, IUnitOfWork unitOfWork, IRecursoProjetoRepository recursoProjetoRepository)
            : base(mapper, repository, unitOfWork)
        {
            _recursoProjetoRepository = recursoProjetoRepository;
        }

        public IEnumerable<RecursoProjetoViewModel> ListarPoridProjeto(Guid idProjeto)
        {
            return _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(_recursoProjetoRepository.ListarPoridProjeto(idProjeto));
        }
    }
}
