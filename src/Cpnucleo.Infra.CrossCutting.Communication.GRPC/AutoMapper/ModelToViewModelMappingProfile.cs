using AutoMapper;
using Cpnucleo.GRPC;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;

namespace Cpnucleo.Infra.CrossCutting.Communication.GRPC.AutoMapper
{
    internal class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<SistemaModel, SistemaViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.DataInclusao, opt => opt.MapFrom(src => DateTime.Parse(src.DataInclusao)))
                .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => DateTime.Parse(src.DataAlteracao)));
        }
    }
}
