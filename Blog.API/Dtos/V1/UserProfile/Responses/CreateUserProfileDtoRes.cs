
namespace Blog.API.Dtos.V1.UserProfile.Responses;

public record CreateUserProfileDtoRes
{
    public Guid Id { get; set; }
    public BasicInfoDtoRes BasicInfo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}