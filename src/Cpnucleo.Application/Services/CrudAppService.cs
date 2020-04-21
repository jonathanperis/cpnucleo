using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    internal class CrudAppService<TEntity, TViewModel> : ICrudAppService<TViewModel> where TViewModel : BaseViewModel
    {
        protected readonly IMapper _mapper;
        protected readonly ICrudService<TEntity> _service;

        public CrudAppService(IMapper mapper, ICrudService<TEntity> service)
        {
            _mapper = mapper;
            _service = service;
        }

        public Guid Incluir(TViewModel obj)
        {
            return _service.Incluir(_mapper.Map<TEntity>(obj));
        }

        public IEnumerable<TViewModel> Listar(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<TViewModel>>(_service.Listar(getDependencies));
        }

        public TViewModel Consultar(Guid id)
        {
            return _mapper.Map<TViewModel>(_service.Consultar(id));
        }

        public bool Remover(Guid id)
        {
            return _service.Remover(id);
        }

        public bool Alterar(TViewModel obj)
        {
            return _service.Alterar(_mapper.Map<TEntity>(obj));
        }
    }
}
