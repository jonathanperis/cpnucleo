using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Services
{
    internal class CrudService<TEntity> : ICrudService<TEntity> where TEntity : BaseEntity
    {
        protected readonly ICrudRepository<TEntity> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public CrudService(ICrudRepository<TEntity> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public Guid Incluir(TEntity obj)
        {
            if (obj.Id == Guid.Empty)
            {
                obj.Id = Guid.NewGuid();
            }

            obj.Ativo = true;
            obj.DataInclusao = DateTime.Now;

            _repository.Incluir(obj);

            if (!_unitOfWork.Commit())
            {
                return Guid.Empty;
            }

            return obj.Id;
        }

        public IEnumerable<TEntity> Listar(bool getDependencies = false)
        {
            return _repository.Listar(getDependencies);
        }

        public TEntity Consultar(Guid id)
        {
            return _repository.Consultar(id);
        }

        public bool Remover(Guid id)
        {
            TEntity obj = _repository.Consultar(id);

            obj.Ativo = false;
            obj.DataExclusao = DateTime.Now;

            _repository.Alterar(obj);

            return _unitOfWork.Commit();
        }

        public bool Alterar(TEntity obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _repository.Alterar(obj);

            return _unitOfWork.Commit();
        }
    }
}
