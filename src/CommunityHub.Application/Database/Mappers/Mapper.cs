using System.Data;

namespace CommunityHub.Application.Database.Mappers
{
    public abstract class Mapper<T> where T : class
    {
        public abstract T ConvertToEntity(IDataReader reader);
        public abstract Dictionary<string, object?> ConvertToDictionary(T entity);
    }
}   
