using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class CrudAppService<TModel, TViewModel> : ICrudAppService<TViewModel> where TViewModel : BaseViewModel
    {
        protected readonly IMapper _mapper;
        protected readonly ICrudRepository<TModel> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public CrudAppService(IMapper mapper, ICrudRepository<TModel> repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public bool Incluir(TViewModel obj)
        {
            _repository.Incluir(_mapper.Map<TModel>(obj));

            return _unitOfWork.Commit();
        }

        public IEnumerable<TViewModel> Listar()
        {
            return _repository.Listar().ProjectTo<TViewModel>(_mapper.ConfigurationProvider).ToList();
        }

        public TViewModel Consultar(Guid id)
        {
            return _repository.Consultar(id).ProjectTo<TViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();
        }

        public bool Remover(Guid id)
        {
            _repository.Remover(id);

            return _unitOfWork.Commit();
        }

        public bool Alterar(TViewModel obj)
        {
            _repository.Alterar(_mapper.Map<TModel>(obj));

            return _unitOfWork.Commit();
        }
    }
}
