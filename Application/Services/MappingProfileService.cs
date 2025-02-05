using Application.Dtos;
using AutoMapper;
using Domain.Models;

namespace Application.Services
{
    public class MappingProfileService : Profile
    {
        public MappingProfileService()
        {
            CreateMap<ClientAccount, ClientAccountDto>()
                .ReverseMap();
        }
    }
}
