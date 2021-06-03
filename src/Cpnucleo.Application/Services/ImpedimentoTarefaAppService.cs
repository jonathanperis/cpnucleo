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
    internal class ImpedimentoTarefaAppService : IImpedimentoTarefaAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImpedimentoTarefaAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ImpedimentoTarefaViewModel> AddAsync(ImpedimentoTarefaViewModel viewModel)
        {
            ImpedimentoTarefaViewModel response = _mapper.Map<ImpedimentoTarefaViewModel>(await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(_mapper.Map<ImpedimentoTarefa>(viewModel)));
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<ImpedimentoTarefaViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.AllAsync(getDependencies));
        }

        public async Task<ImpedimentoTarefaViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<ImpedimentoTarefaViewModel>(await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(id));
        }

        public async Task<IEnumerable<ImpedimentoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa)
        {
            return _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(idTarefa));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImpedimentoTarefaViewModel viewModel)
        {
            _unitOfWork.ImpedimentoTarefaRepository.Update(_mapper.Map<ImpedimentoTarefa>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
