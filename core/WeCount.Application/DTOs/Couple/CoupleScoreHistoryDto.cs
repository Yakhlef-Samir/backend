using System.Collections.Generic;

namespace WeCount.Application.DTOs.Couple
{
    public record ScoreHistoryItemDto(
        DateTime Date,
        int Score
    );

    public record CoupleScoreHistoryDto(
        List<ScoreHistoryItemDto> History,
        int CurrentScore,
        int LowestScore,
        int HighestScore,
        DateTime StartDate,
        DateTime EndDate
    );
}
