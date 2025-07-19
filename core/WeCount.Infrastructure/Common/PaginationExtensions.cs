using MongoDB.Driver;

namespace WeCount.Infrastructure.Common
{
    /// <summary>
    /// Extension methods to add pagination support for MongoDB queries.
    /// </summary>
    public static class PaginationExtensions
    {
        /// <summary>
        /// Applies skip and limit to the query based on page number and page size.
        /// </summary>
        public static IFindFluent<TDocument, TDocument> Paginate<TDocument>(
            this IFindFluent<TDocument, TDocument> query,
            int page,
            int pageSize
        )
        {
            var skip = (page - 1) * pageSize;
            return query.Skip(skip).Limit(pageSize);
        }

        /// <summary>
        /// Executes the paginated query and returns a list of documents.
        /// </summary>
        public static Task<List<TDocument>> ToPaginatedListAsync<TDocument>(
            this IFindFluent<TDocument, TDocument> query,
            int page,
            int pageSize
        ) => query.Paginate(page, pageSize).ToListAsync();
    }
}
