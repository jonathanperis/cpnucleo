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
    internal class ProjetoAppService : IProjetoAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjetoAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjetoViewModel> AddAsync(ProjetoViewModel viewModel)
        {
            ProjetoViewModel result = _mapper.Map<ProjetoViewModel>(await _unitOfWork.ProjetoRepository.AddAsync(_mapper.Map<Projeto>(viewModel)));
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<ProjetoViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<ProjetoViewModel>>(await _unitOfWork.ProjetoRepository.AllAsync(getDependencies));
        }

        public async Task<ProjetoViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<ProjetoViewModel>(await _unitOfWork.ProjetoRepository.GetAsync(id));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.ProjetoRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjetoViewModel viewModel)
        {
            _unitOfWork.ProjetoRepository.Update(_mapper.Map<Projeto>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
