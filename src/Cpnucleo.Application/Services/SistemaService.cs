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
    public class SistemaService : AppService<Sistema, SistemaViewModel>, IAppService<SistemaViewModel>
    {
        public SistemaService(IMapper mapper, IRepository<Sistema> repository)
            : base(mapper, repository)
        {

        }

        public override void Add(SistemaViewModel obj)
        {
            base.Add(obj);

            _repository.Add(_mapper.Map<Sistema>(obj));
        }

        public override void Dispose()
        {
            _repository.Dispose();
        }

        public override IQueryable<SistemaViewModel> GetAll()
        {
            return _repository.GetAll().ProjectTo<SistemaViewModel>(_mapper.ConfigurationProvider);
        }

        public override SistemaViewModel GetById(Guid id)
        {
            return _mapper.Map<SistemaViewModel>(_repository.GetById(id));
        }

        public override void Remove(Guid id)
        {
            _repository.Remove(id);
        }

        public override void Update(SistemaViewModel obj)
        {
            base.Update(obj);

            _repository.Update(_mapper.Map<Sistema>(obj));
        }
    }
}
