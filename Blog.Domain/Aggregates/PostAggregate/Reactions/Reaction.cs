namespace Blog.Domain.Aggregates.PostAggregate.Reactions;

public abstract class Reaction
{
    public abstract int Weight { get; }
    public abstract string Emoji { get; }

    public static Reaction FromType(InteractionType type) => type switch
    {
        InteractionType.Like => new LikeReaction(),
        InteractionType.Wow => new WowReaction(),
        InteractionType.Love => new LoveReaction(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown interaction type")
    };
}
