using AutoMapper;
using Blog.API.Dtos.V1.UserProfile.Requests;
using Blog.API.Dtos.V1.UserProfile.Responses;
using Blog.Application.UserProfile.Commands;
using Blog.Domain.Aggregates.UserProfileAggregate;

namespace Blog.API.MappingProfiles;

public class UserProfileMappingProfiles : Profile
{
    public UserProfileMappingProfiles()
    {
        CreateMap<CreateUserProfileDtoReq, CreateUserProfileCommand>();
        CreateMap<CreateUserProfileDtoReq, UpdateUserProfileCommand>();
        CreateMap<UserProfile, CreateUserProfileDtoRes>();
        CreateMap<BasicInfo, BasicInfoDtoRes>();
    }
}