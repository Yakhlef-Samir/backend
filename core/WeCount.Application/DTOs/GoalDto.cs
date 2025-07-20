namespace WeCount.Application.DTOs
{
    public record GoalDto(
        Guid Id,
        string Name,
        decimal TargetAmount,
        decimal SavedAmount,
        DateTime Deadline,
        string Icon,
        Guid CoupleId
    );
}
