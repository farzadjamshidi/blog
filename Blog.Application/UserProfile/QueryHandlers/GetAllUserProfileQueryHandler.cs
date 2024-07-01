using Blog.Application.UserProfile.Queries;
using Blog.DAL;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.UserProfile.QueryHandlers;

public class GetAllUserProfileQueryHandler: IRequestHandler<GetAllUserProfileQuery, IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>>
{
    private readonly DataContext _ctx;

    public GetAllUserProfileQueryHandler(DataContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IEnumerable<Domain.Aggregates.UserProfileAggregate.UserProfile>> Handle(GetAllUserProfileQuery request, CancellationToken cancellationToken)
    {
        return await _ctx.UserProfiles.ToListAsync(cancellationToken);
    }
}