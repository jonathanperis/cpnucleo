using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Application.Services
{
    internal class RecursoAppService : IRecursoAppService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICryptographyManager _cryptographyManager;

        public RecursoAppService(IUnitOfWork unitOfWork, IMapper mapper, ICryptographyManager cryptographyManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cryptographyManager = cryptographyManager;
        }

        public async Task<RecursoViewModel> AddAsync(RecursoViewModel viewModel)
        {
            _cryptographyManager.CryptPbkdf2(viewModel.Senha, out string senhaCrypt, out string salt);

            viewModel.Senha = senhaCrypt;
            viewModel.Salt = salt;

            RecursoViewModel response = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.AddAsync(_mapper.Map<Recurso>(viewModel)));
            await _unitOfWork.SaveChangesAsync();

            return response;
        }

        public async Task<IEnumerable<RecursoViewModel>> AllAsync(bool getDependencies = false)
        {
            IEnumerable<RecursoViewModel> response = _mapper.Map<IEnumerable<RecursoViewModel>>(await _unitOfWork.RecursoRepository.AllAsync(getDependencies));

            foreach (RecursoViewModel item in response)
            {
                item.Senha = null;
                item.Salt = null;
            }

            return response;
        }

        public async Task<RecursoViewModel> GetAsync(Guid id)
        {
            RecursoViewModel response = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetAsync(id));

            response.Senha = null;
            response.Salt = null;

            return response;
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.RecursoRepository.RemoveAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateAsync(RecursoViewModel viewModel)
        {
            _cryptographyManager.CryptPbkdf2(viewModel.Senha, out string senhaCrypt, out string salt);

            viewModel.Senha = senhaCrypt;
            viewModel.Salt = salt;

            _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(viewModel));
            await _unitOfWork.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
