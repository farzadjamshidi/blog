namespace Blog.API.Dtos.V1.Post.Requests;

public class CreatePostCommentDtoReq
{
    //In next steps we should get UserProfileId from JWT or any other authentication token
    public Guid UserProfileId { get; set; }
    public string Text { get; set; }
}