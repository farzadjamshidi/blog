using Blog.Application.Dtos.Post;
using Blog.Application.Post.Queries;
using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Blog.Application.Post.QueryHandlers;

// Deliberately independent of DataContext/EF Core — the point of this
// handler is to demonstrate Dapper as an alternative data access approach,
// not a hybrid. It only shares the connection string *value* with the EF
// Core path (DbRegistrar.cs), not any EF Core object or type.
public class GetAllPostsDapperQueryHandler(IConfiguration configuration)
    : IRequestHandler<GetAllPostsDapperQuery, IEnumerable<PostSummaryDapperDto>>
{
    public async Task<IEnumerable<PostSummaryDapperDto>> Handle(
        GetAllPostsDapperQuery request, CancellationToken cancellationToken)
    {
        await using var connection = new SqlConnection(configuration.GetConnectionString("AzureSql"));

        var command = new CommandDefinition(
            "SELECT Id, Text, CreatedAt FROM Posts ORDER BY CreatedAt DESC",
            cancellationToken: cancellationToken);

        return await connection.QueryAsync<PostSummaryDapperDto>(command);
    }
}
