namespace Blog.Domain.Aggregates.PostAggregate.Reactions;

public sealed class LoveReaction : Reaction
{
    public override int Weight => 3;
    public override string Emoji => "❤️";
}
