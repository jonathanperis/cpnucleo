using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    public class CrudService<TModel> : ICrudService<TModel> where TModel : BaseModel
    {
        protected readonly ICrudRepository<TModel> _repository;
        protected readonly IUnitOfWork _unitOfWork;

        public CrudService(ICrudRepository<TModel> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public bool Incluir(TModel obj)
        {
            if (obj.Id == Guid.Empty)
            {
                obj.Id = Guid.NewGuid();
            }

            obj.Ativo = true;
            obj.DataInclusao = DateTime.Now;

            _repository.Incluir(obj);

            return _unitOfWork.Commit();
        }

        public IQueryable<TModel> Listar()
        {
            return _repository.Listar();
        }

        public IQueryable<TModel> Consultar(Guid id)
        {
            return _repository.Consultar(id);
        }

        public bool Remover(Guid id)
        {
            TModel obj = _repository.Consultar(id).FirstOrDefault();

            obj.Ativo = false;
            obj.DataExclusao = DateTime.Now;

            _repository.Alterar(obj);

            return _unitOfWork.Commit();
        }

        public bool Alterar(TModel obj)
        {
            obj.DataAlteracao = DateTime.Now;

            _repository.Alterar(obj);

            return _unitOfWork.Commit();
        }
    }
}
