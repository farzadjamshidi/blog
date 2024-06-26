namespace Blog.Domain.Aggregates.UserProfileAggregate;

public class UserProfile
{
    private UserProfile()
    {
    }
    
    public Guid Id { get; private set; }
    public int IdentityId { get; private set; }
    public BasicInfo BasicInfo { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    public static UserProfile CreateUserProfile(int identityId, BasicInfo basicInfo)
    {
        //Here is for validations
        return new UserProfile()
        {
            IdentityId = identityId,
            BasicInfo = basicInfo,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void UpdateBasicInfo(BasicInfo newBasicInfo)
    {
        //Here is for validations
        BasicInfo = newBasicInfo;
        UpdatedAt = DateTime.UtcNow;
    }
}