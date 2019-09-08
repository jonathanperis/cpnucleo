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
    public class ImpedimentoService : IAppService<ImpedimentoViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Impedimento> _repository;

        public ImpedimentoService(IMapper mapper, IRepository<Impedimento> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public void Add(ImpedimentoViewModel obj)
        {
            obj.Id = new Guid();
            obj.DataInclusao = DateTime.Now;

            _repository.Add(_mapper.Map<Impedimento>(obj));
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public IQueryable<ImpedimentoViewModel> GetAll()
        {
            return _repository.GetAll().ProjectTo<ImpedimentoViewModel>(_mapper.ConfigurationProvider);
        }

        public ImpedimentoViewModel GetById(Guid id)
        {
            return _mapper.Map<ImpedimentoViewModel>(_repository.GetById(id));
        }

        public void Remove(Guid id)
        {
            _repository.Remove(id);
        }

        public void Update(ImpedimentoViewModel obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _repository.Update(_mapper.Map<Impedimento>(obj));
        }
    }
}
