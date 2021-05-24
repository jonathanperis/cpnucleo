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
            return _mapper.Map<WorkflowViewModel>(await _unitOfWork.WorkflowRepository.AddAsync(_mapper.Map<Workflow>(viewModel)));
        }

        public async Task<IEnumerable<WorkflowViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<WorkflowViewModel>>(await _unitOfWork.WorkflowRepository.AllAsync(getDependencies));
        }

        public async Task<WorkflowViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<WorkflowViewModel>(await _unitOfWork.WorkflowRepository.GetAsync(id));
        }

        public async Task<int> GetQuantidadeColunasAsync()
        {
            return await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();
        }

        public string GetTamanhoColuna(int colunas)
        {
            return _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.WorkflowRepository.RemoveAsync(id);
        }

        public void Update(WorkflowViewModel viewModel)
        {
            _unitOfWork.WorkflowRepository.Update(_mapper.Map<Workflow>(viewModel));
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
