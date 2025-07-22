using MongoDB.Driver;
using WeCount.Domain.Common;

namespace WeCount.Infrastructure.Common
{
    /// <summary>
    /// Extension methods to provide generic CRUD operations for MongoDB collections.
    /// </summary>
    public static class CrudExtensions
    {
        /// <summary>
        /// Retrieves a document by its Id.
        /// </summary>
        public static Task<T> GetByIdAsync<T>(this IMongoCollection<T> collection, Guid id)
            where T : EntityBase
        {
            return collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserts a new document.
        /// </summary>
        public static async Task<T> CreateAsync<T>(this IMongoCollection<T> collection, T entity)
            where T : EntityBase
        {
            await collection.InsertOneAsync(entity);
            return entity;
        }

        /// <summary>
        /// Replaces an existing document.
        /// </summary>
        public static async Task<bool> UpdateAsync<T>(this IMongoCollection<T> collection, T entity)
            where T : EntityBase
        {
            ReplaceOneResult result = await collection.ReplaceOneAsync(
                e => e.Id == entity.Id,
                entity
            );
            return result.ModifiedCount > 0;
        }

        /// <summary>
        /// Deletes a document by its Id.
        /// </summary>
        public static async Task<bool> DeleteAsync<T>(this IMongoCollection<T> collection, Guid id)
            where T : EntityBase
        {
            DeleteResult result = await collection.DeleteOneAsync(e => e.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
