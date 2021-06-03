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
    internal class ApontamentoAppService : IApontamentoAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApontamentoAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApontamentoViewModel> AddAsync(ApontamentoViewModel viewModel)
        {
            ApontamentoViewModel response = _mapper.Map<ApontamentoViewModel>(await _unitOfWork.ApontamentoRepository.AddAsync(_mapper.Map<Apontamento>(viewModel)));
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<ApontamentoViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.AllAsync(getDependencies));
        }

        public async Task<ApontamentoViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<ApontamentoViewModel>(await _unitOfWork.ApontamentoRepository.GetAsync(id));
        }

        public async Task<IEnumerable<ApontamentoViewModel>> GetByRecursoAsync(Guid idRecurso)
        {
            return _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(idRecurso));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.ApontamentoRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApontamentoViewModel viewModel)
        {
            _unitOfWork.ApontamentoRepository.Update(_mapper.Map<Apontamento>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
