using System.Collections.Generic;

namespace WeCount.Application.DTOs.Financial
{
    public record FinancialReportDto(
        List<FinancialPredictionDto> Predictions,
        FinancialTotalsDto Totals
    );
}
