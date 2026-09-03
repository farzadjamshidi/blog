namespace Blog.Domain.Aggregates.PostAggregate.Reactions;

public sealed class WowReaction : Reaction
{
    public override int Weight => 2;
    public override string Emoji => "😮";
}
