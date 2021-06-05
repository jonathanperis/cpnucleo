using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            TarefaViewModel result = _mapper.Map<TarefaViewModel>(await _unitOfWork.TarefaRepository.AddAsync(_mapper.Map<Tarefa>(viewModel)));
            await _unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<TarefaViewModel>> AllAsync(bool getDependencies = false)
        {
            IEnumerable<TarefaViewModel> result = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.AllAsync(getDependencies));

            await PreencherDadosAdicionaisAsync(result);

            return result;
        }

        public async Task<TarefaViewModel> GetAsync(Guid id)
        {
            return _mapper.Map<TarefaViewModel>(await _unitOfWork.TarefaRepository.GetAsync(id));
        }

        public async Task<IEnumerable<TarefaViewModel>> GetByRecursoAsync(Guid idRecurso)
        {
            IEnumerable<TarefaViewModel> result = _mapper.Map<IEnumerable<TarefaViewModel>>(await _unitOfWork.TarefaRepository.GetByRecursoAsync(idRecurso));

            await PreencherDadosAdicionaisAsync(result);

            return result;
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.TarefaRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(TarefaViewModel viewModel)
        {
            _unitOfWork.TarefaRepository.Update(_mapper.Map<Tarefa>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        private async Task PreencherDadosAdicionaisAsync(IEnumerable<TarefaViewModel> lista)
        {
            int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

            foreach (TarefaViewModel item in lista)
            {
                item.Workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);

                item.HorasConsumidas = await _unitOfWork.ApontamentoRepository.GetTotalHorasByRecursoAsync(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                IEnumerable<ImpedimentoTarefa> impedimentos = await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(item.Id);

                if (impedimentos.Any())
                {
                    item.TipoTarefa.Element = "warning-element";
                }
                else if (DateTime.Now.Date >= item.DataInicio && DateTime.Now.Date <= item.DataTermino)
                {
                    item.TipoTarefa.Element = "success-element";
                }
                else if (DateTime.Now.Date > item.DataTermino)
                {
                    item.TipoTarefa.Element = "danger-element";
                }
                else
                {
                    item.TipoTarefa.Element = "info-element";
                }
            }
        }
    }
}
