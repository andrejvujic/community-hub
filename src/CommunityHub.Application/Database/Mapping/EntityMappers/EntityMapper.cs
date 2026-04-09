using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public abstract class EntityMapper<T> where T : class
    {
        public abstract T? MapToEntity(IDataReader reader);
        public abstract Dictionary<string, object?> MapToQueryParameters(T entity);
    }
}   
