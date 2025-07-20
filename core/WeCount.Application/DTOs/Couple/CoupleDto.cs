namespace WeCount.Application.DTOs.Couple
{
    public record CoupleDto(Guid Id, string Name, List<CoupleUserDto> Members);
}
