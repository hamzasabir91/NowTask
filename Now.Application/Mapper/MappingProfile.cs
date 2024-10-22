using Application.Commands.User;
using AutoMapper;
using Now.Application.Dtos;
using Now.Domain.Entites;

namespace NowTask.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() {
            CreateMap<User, CreateUserAsync>().ReverseMap();
            CreateMap<User, AuthenticateDto>().ReverseMap();
        }
    }
}
