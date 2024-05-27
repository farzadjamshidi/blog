using Microsoft.AspNetCore.Identity;

namespace Blog.Domain;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole(string Name): base(Name)
    {
        
    }
}