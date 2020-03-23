using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class CrudAppService<TModel, TViewModel> : ICrudAppService<TViewModel> where TViewModel : BaseViewModel
    {
        protected readonly IMapper _mapper;
        protected readonly ICrudService<TModel> _service;

        public CrudAppService(IMapper mapper, ICrudService<TModel> service)
        {
            _mapper = mapper;
            _service = service;
        }

        public bool Incluir(TViewModel obj)
        {
            return _service.Incluir(_mapper.Map<TModel>(obj));
        }

        public IEnumerable<TViewModel> Listar()
        {
            return _service.Listar().ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public TViewModel Consultar(Guid id)
        {
            return _service.Consultar(id).ProjectTo<TViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();
        }

        public bool Remover(Guid id)
        {
            return _service.Remover(id);
        }

        public bool Alterar(TViewModel obj)
        {
            return _service.Alterar(_mapper.Map<TModel>(obj));
        }
    }
}
