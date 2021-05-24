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
    internal class TarefaAppService : ITarefaAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TarefaAppService(IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<TarefaViewModel> AddAsync(TarefaViewModel viewModel)
        {
            return _mapper.Map<TarefaViewModel>(await _unitOfWork.TarefaRepository.AddAsync(_mapper.Map<Tarefa>(viewModel)));
        }

        public async Task<IEnumerable<TarefaViewModel>> AllAsync(bool getDependencies = false)
        {
            return _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.AllAsync(getDependencies));
        }

        public async Task<TarefaViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<TarefaViewModel>(await _unitOfWork.TarefaRepository.GetAsync(id));
        }

        public async Task<IEnumerable<TarefaViewModel>> GetByRecursoAsync(Guid idRecurso)
        {
            return _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.GetByRecursoAsync(idRecurso));
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.TarefaRepository.RemoveAsync(id);
        }

        public void Update(TarefaViewModel viewModel)
        {
            _unitOfWork.TarefaRepository.Update(_mapper.Map<Tarefa>(viewModel));
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
