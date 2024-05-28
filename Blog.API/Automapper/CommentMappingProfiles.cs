using AutoMapper;
using Blog.API.Dtos;
using Blog.Domain.Entities;

namespace Blog.API.Automapper;

public class CommentMappingProfiles: Profile
{
    public CommentMappingProfiles()
    {
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Comment, GetByIdCommentDto>();
    }
}