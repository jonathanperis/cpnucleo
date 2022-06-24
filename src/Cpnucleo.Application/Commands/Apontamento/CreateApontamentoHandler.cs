﻿using Cpnucleo.Shared.Commands.Apontamento;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Application.Commands.Apontamento;

public class CreateApontamentoHandler : IRequestHandler<CreateApontamentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateApontamentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateApontamentoCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ApontamentoRepository.AddAsync(_mapper.Map<Domain.Entities.Apontamento>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
