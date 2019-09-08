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
    public class ProjetoService : IAppService<ProjetoViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Projeto> _repository;

        public ProjetoService(IMapper mapper, IRepository<Projeto> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public void Add(ProjetoViewModel obj)
        {
            obj.Id = new Guid();
            obj.DataInclusao = DateTime.Now;

            _repository.Add(_mapper.Map<Projeto>(obj));
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public IQueryable<ProjetoViewModel> GetAll()
        {
            return _repository.GetAll().ProjectTo<ProjetoViewModel>(_mapper.ConfigurationProvider);
        }

        public ProjetoViewModel GetById(Guid id)
        {
            return _mapper.Map<ProjetoViewModel>(_repository.GetById(id));
        }

        public void Remove(Guid id)
        {
            _repository.Remove(id);
        }

        public void Update(ProjetoViewModel obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _repository.Update(_mapper.Map<Projeto>(obj));
        }
    }
}
