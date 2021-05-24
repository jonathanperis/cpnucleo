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
    internal class TipoTarefaAppService : ITipoTarefaAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TipoTarefaAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TipoTarefaViewModel> AddAsync(TipoTarefaViewModel viewModel)
        {
            return _mapper.Map<TipoTarefaViewModel>(await _unitOfWork.TipoTarefaRepository.AddAsync(_mapper.Map<TipoTarefa>(viewModel)));
        }

        public async Task<IEnumerable<TipoTarefaViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<TipoTarefaViewModel>>(await _unitOfWork.TipoTarefaRepository.AllAsync(getDependencies));
        }

        public async Task<TipoTarefaViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<TipoTarefaViewModel>(await _unitOfWork.TipoTarefaRepository.GetAsync(id));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.TipoTarefaRepository.RemoveAsync(id);
        }

        public void Update(TipoTarefaViewModel viewModel)
        {
            _unitOfWork.TipoTarefaRepository.Update(_mapper.Map<TipoTarefa>(viewModel));
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
