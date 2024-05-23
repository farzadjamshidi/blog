using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;

namespace Blog.API.Automapper;

public class PostMappingProfiles: Profile
{
    public PostMappingProfiles()
    {
        CreateMap<CreatePostDto, Post>();
        CreateMap<Post, GetByIdPostDto>();
    }
}