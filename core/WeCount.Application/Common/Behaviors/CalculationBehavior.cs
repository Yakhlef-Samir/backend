using MediatR;

namespace WeCount.Application.Common.Behaviors
{
    public interface ICalculationAlgorithm
    {
        Task ExecuteAsync(object context);
    }

    public class CalculationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<ICalculationAlgorithm> _algorithms;

        public CalculationBehavior(IEnumerable<ICalculationAlgorithm> algorithms)
        {
            _algorithms = algorithms;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            foreach (var algo in _algorithms)
                await algo.ExecuteAsync(request);

            var response = await next();
            return response;
        }
    }
}
