﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    public class AppService<TModel, TViewModel> : IAppService<TViewModel> where TViewModel : BaseViewModel
    {
        protected readonly IMapper _mapper;
        protected readonly IRepository<TModel> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public AppService(IMapper mapper, IRepository<TModel> repository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public bool Incluir(TViewModel obj)
        {
            obj.Id = new Guid();
            obj.DataInclusao = DateTime.Now;

            _repository.Incluir(_mapper.Map<TModel>(obj));

            return _unitOfWork.Commit();
        }

        public IEnumerable<TViewModel> Listar()
        {
            return _repository.Listar().ProjectTo<TViewModel>(_mapper.ConfigurationProvider);
        }

        public TViewModel Consultar(Guid id)
        {
            return _mapper.Map<TViewModel>(_repository.Consultar(id));
        }

        public bool Remover(Guid id)
        {
            _repository.Remover(id);

            return _unitOfWork.Commit();
        }

        public bool Alterar(TViewModel obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _repository.Alterar(_mapper.Map<TModel>(obj));

            return _unitOfWork.Commit();
        }
    }
}