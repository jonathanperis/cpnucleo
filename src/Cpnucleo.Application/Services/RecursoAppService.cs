using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Services
{
    internal class RecursoAppService : IRecursoAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecursoAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RecursoViewModel> AddAsync(RecursoViewModel viewModel)
        {
            return _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.AddAsync(_mapper.Map<Recurso>(viewModel)));
        }

        public async Task<IEnumerable<RecursoViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<RecursoViewModel>>(await _unitOfWork.RecursoRepository.AllAsync(getDependencies));
        }

        public async Task<RecursoViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetAsync(id));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.RecursoRepository.RemoveAsync(id);
        }

        public void Update(RecursoViewModel viewModel)
        {
            _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(viewModel));
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
