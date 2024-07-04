namespace Blog.API;

public class Routes
{
    public const string Base = "api/v{version:apiVersion}/[controller]";

    public const string Entity = "{id}";

    public class UserProfile
    {
        public const string Entity = "{id}";
    }
    public class Post
    {
        public const string Entity = "{id}";
        public const string Comment = "{id}/Comment";
        public const string CommentEntity = "{id}/Comment/{commentId}";
    }
}