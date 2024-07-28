using AutoMapper;
using Blog.API.Dtos.V1.Post.Requests;
using Blog.API.Dtos.V1.Post.Responses;
using Blog.Application.Dtos.Post;
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
        CreateMap<Blog.Application.Dtos.Post.InteractionCount, Blog.API.Dtos.V1.Post.Responses.InteractionCount>();
        CreateMap<GetPostByIdDtoApp, GetPostByIdDtoRes>().ForMember(
            res => res.Id, 
            m => 
                m.MapFrom(app => app.Post.Id))
            .ForMember(
                res => res.UserProfile, 
                m => 
                    m.MapFrom(app => app.Post.UserProfile))
            .ForMember(
                res => res.Text, 
                m => 
                    m.MapFrom(app => app.Post.Text))
            .ForMember(
                res => res.Comments, 
                m => 
                    m.MapFrom(app => app.Post.Comments))
            .ForMember(
                res => res.CreatedAt, 
                m => 
                    m.MapFrom(app => app.Post.CreatedAt))
            .ForMember(
                res => res.UpdatedAt, 
                m => 
                    m.MapFrom(app => app.Post.UpdatedAt));
    }
}