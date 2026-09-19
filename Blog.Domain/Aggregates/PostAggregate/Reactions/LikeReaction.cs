namespace Blog.Domain.Aggregates.PostAggregate.Reactions;

public sealed class LikeReaction : Reaction
{
    public override int Weight => 1;
    public override string Emoji => "👍";
}
