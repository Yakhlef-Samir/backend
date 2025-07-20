using MediatR;
using WeCount.Application.Common.Interfaces.Repositories;
using WeCount.Domain.Entities.Couple;

namespace WeCount.Application.Couple.Commands
{
    public class SaveCoupleScoreCommandHandler : IRequestHandler<SaveCoupleScoreCommand, bool>
    {
        private readonly ICoupleRepository _coupleRepository;

        public SaveCoupleScoreCommandHandler(ICoupleRepository coupleRepository)
        {
            _coupleRepository = coupleRepository;
        }

        public async Task<bool> Handle(
            SaveCoupleScoreCommand request,
            CancellationToken cancellationToken
        )
        {
            // Verify couple exists
            var couple = await _coupleRepository.GetByIdAsync(request.CoupleId);
            if (couple == null)
            {
                throw new Exception($"Couple with ID {request.CoupleId} not found");
            }

            // Create new score history entry
            var scoreHistory = new CoupleScoreHistoryItem
            {
                CoupleId = request.CoupleId,
                Date = request.Date,
                Score = request.Score,
                BudgetScore = request.BudgetScore,
                GoalsScore = request.GoalsScore,
                DebtScore = request.DebtScore,
                SavingsScore = request.SavingsScore,
                TransactionsScore = request.TransactionsScore,
                Message = request.Message,
            };

            // Save to repository
            await _coupleRepository.AddAsync(scoreHistory);

            return true;
        }
    }
}
