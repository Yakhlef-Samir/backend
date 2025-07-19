namespace WeCount.Application.DTOs
{
    public record StatCardDataDto(
        string Id,
        string Amount,
        string Label,
        string Change,
        string Type,
        string Icon
    );
}
