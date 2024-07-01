namespace Blog.API.Dtos.V1.UserProfile.Responses;

public record BasicInfoDtoRes
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
}