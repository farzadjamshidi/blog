using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;

namespace Blog.API.Automapper;

public class UserMappingProfiles: Profile
{
    public UserMappingProfiles()
    {
        CreateMap<CreateUserDto, User>();
    }
}