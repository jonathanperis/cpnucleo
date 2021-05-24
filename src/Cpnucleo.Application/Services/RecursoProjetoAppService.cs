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
    internal class RecursoProjetoAppService : IRecursoProjetoAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecursoProjetoAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RecursoProjetoViewModel> AddAsync(RecursoProjetoViewModel viewModel)
        {
            return _mapper.Map<RecursoProjetoViewModel>(await _unitOfWork.RecursoProjetoRepository.AddAsync(_mapper.Map<RecursoProjeto>(viewModel)));
        }

        public async Task<IEnumerable<RecursoProjetoViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.AllAsync(getDependencies));
        }

        public async Task<RecursoProjetoViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<RecursoProjetoViewModel>(await _unitOfWork.RecursoProjetoRepository.GetAsync(id));
        }

        public async Task<IEnumerable<RecursoProjetoViewModel>> GetByProjetoAsync(Guid idProjeto)
        {
            return _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(idProjeto));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.RecursoProjetoRepository.RemoveAsync(id);
        }

        public void Update(RecursoProjetoViewModel viewModel)
        {
            _unitOfWork.RecursoProjetoRepository.Update(_mapper.Map<RecursoProjeto>(viewModel));
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
