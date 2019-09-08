using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class TipoTarefaService : IAppService<TipoTarefaViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TipoTarefa> _repository;

        public TipoTarefaService(IMapper mapper, IRepository<TipoTarefa> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public void Add(TipoTarefaViewModel obj)
        {
            obj.Id = new Guid();
            obj.DataInclusao = DateTime.Now;

            _repository.Add(_mapper.Map<TipoTarefa>(obj));
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public IQueryable<TipoTarefaViewModel> GetAll()
        {
            return _repository.GetAll().ProjectTo<TipoTarefaViewModel>(_mapper.ConfigurationProvider);
        }

        public TipoTarefaViewModel GetById(Guid id)
        {
            return _mapper.Map<TipoTarefaViewModel>(_repository.GetById(id));
        }

        public void Remove(Guid id)
        {
            _repository.Remove(id);
        }

        public void Update(TipoTarefaViewModel obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _repository.Update(_mapper.Map<TipoTarefa>(obj));
        }
    }
}
