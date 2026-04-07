using System.Data;

namespace CommunityHub.Application.Database.Mappers
{
    public abstract class DatabaseEntityMapper<T> where T : class
    {
        public abstract T MapToEntity(IDataReader reader);
        public abstract Dictionary<string, object?> MapToQueryParameters(T entity);
    }
}   
