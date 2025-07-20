using MediatR;
using MongoDB.Driver;
using WeCount.Domain.Common;

namespace WeCount.Application.Common.Handlers
{
    public class GetByIdQuery<TEntity, TDto> : IRequest<TDto?>
        where TEntity : EntityBase
    {
        public Guid Id { get; }
        public Func<TEntity, TDto> Map { get; }
        public IMongoCollection<TEntity> Collection { get; }

        public GetByIdQuery(Guid id, IMongoCollection<TEntity> collection, Func<TEntity, TDto> map)
        {
            Id = id;
            Collection = collection;
            Map = map;
        }
    }

    public class GetByIdQueryHandler<TEntity, TDto>
        : IRequestHandler<GetByIdQuery<TEntity, TDto>, TDto?>
        where TEntity : EntityBase
    {
        public async Task<TDto?> Handle(
            GetByIdQuery<TEntity, TDto> request,
            CancellationToken cancellationToken
        )
        {
            var entity = await request
                .Collection.Find(e => e.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            return entity is null ? default : request.Map(entity);
        }
    }
}
