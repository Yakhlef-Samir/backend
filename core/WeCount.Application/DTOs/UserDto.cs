namespace WeCount.Application.DTOs
{
    public record UserDto(
        Guid Id,
        string Email,
        string Avatar,
        string Name,
        string? PhoneNumber,
        string? Address,
        string? City,
        string? ZipCode,
        string? Country,
        Guid CoupleId
    );
}
