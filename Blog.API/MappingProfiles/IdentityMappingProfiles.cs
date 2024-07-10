using AutoMapper;
using Blog.API.Dtos.Identity.Requests;
using Blog.API.Dtos.Identity.Responses;
using Blog.Application.Identity.Commands;
using Blog.Application.Identity.Dtos;

namespace Blog.API.MappingProfiles;

public class IdentityMappingProfiles: Profile
{
    public IdentityMappingProfiles()
    {
        CreateMap<RegisterUserDtoReq, RegisterUserCommand>();
        CreateMap<RegisterUserCommandHandlerDto, RegisterUserDtoRes> ();
    }
}