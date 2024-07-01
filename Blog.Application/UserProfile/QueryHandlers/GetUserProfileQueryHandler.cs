using Blog.Application.UserProfile.Queries;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserProfile.QueryHandlers;

public class GetUserProfileQueryHandler: IRequestHandler<GetUserProfileQuery, Domain.Aggregates.UserProfileAggregate.UserProfile?>
{
    private readonly DataContext _ctx;

    public GetUserProfileQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }
    public async Task<Domain.Aggregates.UserProfileAggregate.UserProfile?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.UserProfiles.FirstOrDefaultAsync(up => up.Id == request.Id
        , cancellationToken);
    }
}