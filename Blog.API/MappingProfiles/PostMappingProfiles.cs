using AutoMapper;
using Blog.API.Dtos.V1.Post.Requests;
using Blog.API.Dtos.V1.Post.Responses;
using Blog.Application.Post.Commands;
using Blog.Domain.Aggregates.PostAggregate;

namespace Blog.API.MappingProfiles;

public class PostMappingProfiles: Profile
{
    public PostMappingProfiles()
    {
        CreateMap<Post, CreatePostDtoRes>();
        CreateMap<UpdatePostDtoReq, UpdatePostCommand>();
        CreateMap<PostComment, CreatePostCommentDtoRes>();
        CreateMap<PostInteraction, CreatePostInteractionDtoRes>();
    }
}