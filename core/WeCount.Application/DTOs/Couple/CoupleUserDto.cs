using System;
using WeCount.Domain.Enums;

namespace WeCount.Application.DTOs.Couple
{
    public record CoupleUserDto(
        Guid Id,
        Guid UserId,
        Guid CoupleId,
        string Role,
        bool IsCreator,
        DateTime InvitationSentAt,
        DateTime? JoinedAt,
        CoupleUserStatus Status,
        string? Avatar,
        string? Email
    );
}
