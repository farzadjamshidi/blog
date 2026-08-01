using Blog.Application.Post.CommandHandlers;
using Blog.Application.Post.Commands;
using Blog.DAL;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Blog.Application.Tests.Post.CommandHandlers;

public class CreatePostCommandHandlerTests
{
    // Each test gets its own isolated in-memory database so tests
    // can't leak state into one another.
    private static DataContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new DataContext(options);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsPostWithExpectedValues()
    {
        // Arrange
        await using var ctx = CreateInMemoryContext();
        var handler = new CreatePostCommandHandler(ctx);
        var command = new CreatePostCommand
        {
            UserProfileId = Guid.NewGuid(),
            Text = "My first post"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Text, result.Text);
        Assert.Equal(command.UserProfileId, result.UserProfileId);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task Handle_ValidCommand_PersistsPostToDatabase()
    {
        // Arrange
        await using var ctx = CreateInMemoryContext();
        var handler = new CreatePostCommandHandler(ctx);
        var command = new CreatePostCommand
        {
            UserProfileId = Guid.NewGuid(),
            Text = "Persisted post"
        };

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert — read back from the context to prove SaveChangesAsync
        // actually committed the entity, not just returned it in memory.
        var savedPost = await ctx.Posts.FindAsync(result.Id);
        Assert.NotNull(savedPost);
        Assert.Equal(command.Text, savedPost!.Text);
    }

    [Fact]
    public async Task Handle_SetsCreatedAtAndUpdatedAtToSameUtcTimestamp()
    {
        // Arrange
        await using var ctx = CreateInMemoryContext();
        var handler = new CreatePostCommandHandler(ctx);
        var before = DateTime.UtcNow;

        // Act
        var result = await handler.Handle(new CreatePostCommand
        {
            UserProfileId = Guid.NewGuid(),
            Text = "Timestamp check"
        }, CancellationToken.None);

        var after = DateTime.UtcNow;

        // Assert
        Assert.InRange(result.CreatedAt, before, after);
        Assert.Equal(result.CreatedAt, result.UpdatedAt);
    }

    // --- Edge case that documents a real gap in the current code ---
    //
    // Post.CreatePost() has a comment "//Here is for validations" but no
    // actual validation logic. This test isn't asserting desired behavior;
    // it's documenting *current* behavior so the gap is visible and
    // intentional rather than accidental. Worth raising in code review.
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task Handle_EmptyOrNullText_IsCurrentlyAcceptedWithoutValidation(string? text)
    {
        // Arrange
        await using var ctx = CreateInMemoryContext();
        var handler = new CreatePostCommandHandler(ctx);

        // Act
        var result = await handler.Handle(new CreatePostCommand
        {
            UserProfileId = Guid.NewGuid(),
            Text = text!
        }, CancellationToken.None);

        // Assert — this currently succeeds. If validation is added to
        // Post.CreatePost later, this test should be updated to expect
        // a thrown exception or a validation result instead.
        Assert.Equal(text, result.Text);
    }
}