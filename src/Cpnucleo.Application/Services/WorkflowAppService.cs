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
    internal class WorkflowAppService : IWorkflowAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkflowAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WorkflowViewModel> AddAsync(WorkflowViewModel viewModel)
        {
            WorkflowViewModel response = _mapper.Map<WorkflowViewModel>(await _unitOfWork.WorkflowRepository.AddAsync(_mapper.Map<Workflow>(viewModel)));
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<WorkflowViewModel>> AllAsync(bool getDependencies = false)
        {
            IEnumerable<WorkflowViewModel> response = _mapper.Map<IEnumerable<WorkflowViewModel>>(await _unitOfWork.WorkflowRepository.AllAsync(getDependencies));

            await PreencherDadosAdicionaisAsync(response);

            return response;
        }

        public async Task<WorkflowViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<WorkflowViewModel>(await _unitOfWork.WorkflowRepository.GetAsync(id));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.WorkflowRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkflowViewModel viewModel)
        {
            _unitOfWork.WorkflowRepository.Update(_mapper.Map<Workflow>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async Task PreencherDadosAdicionaisAsync(IEnumerable<WorkflowViewModel> lista)
        {
            int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

            foreach (WorkflowViewModel item in lista)
            {
                item.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
            }
        }
    }
}
