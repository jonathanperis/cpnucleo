using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using System;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class AppService<TModel, TViewModel> : IAppService<TViewModel> where TViewModel : BaseViewModel
    {
        protected readonly IMapper _mapper;
        protected readonly IRepository<TModel> _repository;

        public AppService(IMapper mapper, IRepository<TModel> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public void Incluir(TViewModel obj)
        {
            obj.Id = new Guid();
            obj.DataInclusao = DateTime.Now;

            _repository.Incluir(_mapper.Map<TModel>(obj));
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public IQueryable<TViewModel> Listar()
        {
            return _repository.Listar().ProjectTo<TViewModel>(_mapper.ConfigurationProvider);
        }

        public TViewModel Consultar(Guid id)
        {
            return _mapper.Map<TViewModel>(_repository.Consultar(id));
        }

        public void Remover(Guid id)
        {
            _repository.Remover(id);
        }

        public void Alterar(TViewModel obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _repository.Alterar(_mapper.Map<TModel>(obj));
        }
    }
}
