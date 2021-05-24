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
    internal class SistemaAppService : ISistemaAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SistemaAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SistemaViewModel> AddAsync(SistemaViewModel viewModel)
        {
            return _mapper.Map<SistemaViewModel>(await _unitOfWork.SistemaRepository.AddAsync(_mapper.Map<Sistema>(viewModel)));
        }

        public async Task<IEnumerable<SistemaViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<SistemaViewModel>>(await _unitOfWork.SistemaRepository.AllAsync(getDependencies));
        }

        public async Task<SistemaViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<SistemaViewModel>(await _unitOfWork.SistemaRepository.GetAsync(id));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.SistemaRepository.RemoveAsync(id);
        }

        public void Update(SistemaViewModel viewModel)
        {
            _unitOfWork.SistemaRepository.Update(_mapper.Map<Sistema>(viewModel));
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
