using MediatR;
using WeCount.Application.DTOs.Couple;
using WeCount.Domain.Entities.Couple;
using WeCount.Application.Common.Interfaces.Repositories;

namespace WeCount.Application.Couple.Queries
{
    public class GetCoupleScoreHistoryQueryHandler
        : IRequestHandler<GetCoupleScoreHistoryQuery, CoupleScoreHistoryDto>
    {
        private readonly ICoupleRepository _coupleRepository;

        public GetCoupleScoreHistoryQueryHandler(ICoupleRepository coupleRepository)
        {
            _coupleRepository = coupleRepository;
        }

        public async Task<CoupleScoreHistoryDto> Handle(
            GetCoupleScoreHistoryQuery request,
            CancellationToken cancellationToken
        )
        {
            // Verify couple exists
            var couple = await _coupleRepository.GetByIdAsync(request.CoupleId);
            if (couple == null)
            {
                throw new Exception($"Couple with ID {request.CoupleId} not found");
            }

            // Get score history
            var allScoreHistory = await _coupleRepository.GetScoreHistoryAsync(request.CoupleId);

            // Filter by months if specified
            DateTime startDate = DateTime.MinValue;
            if (request.Months.HasValue)
            {
                startDate = DateTime.UtcNow.AddMonths(-request.Months.Value);
                allScoreHistory = allScoreHistory.Where(s => s.Date >= startDate).ToList();
            }

            // Order by date
            var orderedHistory = allScoreHistory.OrderBy(s => s.Date).ToList();

            // Map to DTOs
            var historyItems = orderedHistory
                .Select(s => new ScoreHistoryItemDto(Date: s.Date, Score: s.Score))
                .ToList();

            // Calculate statistics
            int currentScore = historyItems.Any() ? historyItems.Last().Score : 0;
            int lowestScore = historyItems.Any() ? historyItems.Min(h => h.Score) : 0;
            int highestScore = historyItems.Any() ? historyItems.Max(h => h.Score) : 0;

            DateTime historyStartDate = historyItems.Any()
                ? historyItems.First().Date
                : DateTime.UtcNow;
            DateTime historyEndDate = historyItems.Any()
                ? historyItems.Last().Date
                : DateTime.UtcNow;

            return new CoupleScoreHistoryDto(
                History: historyItems,
                CurrentScore: currentScore,
                LowestScore: lowestScore,
                HighestScore: highestScore,
                StartDate: historyStartDate,
                EndDate: historyEndDate
            );
        }
    }
}
