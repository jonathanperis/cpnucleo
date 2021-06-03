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
    internal class RecursoTarefaAppService : IRecursoTarefaAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RecursoTarefaAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RecursoTarefaViewModel> AddAsync(RecursoTarefaViewModel viewModel)
        {
            RecursoTarefaViewModel response = _mapper.Map<RecursoTarefaViewModel>(await _unitOfWork.RecursoTarefaRepository.AddAsync(_mapper.Map<RecursoTarefa>(viewModel)));
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<RecursoTarefaViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.AllAsync(getDependencies));
        }

        public async Task<RecursoTarefaViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<RecursoTarefaViewModel>(await _unitOfWork.RecursoTarefaRepository.GetAsync(id));
        }

        public async Task<IEnumerable<RecursoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa)
        {
            return _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(await _unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(idTarefa));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.RecursoTarefaRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecursoTarefaViewModel viewModel)
        {
            _unitOfWork.RecursoTarefaRepository.Update(_mapper.Map<RecursoTarefa>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
