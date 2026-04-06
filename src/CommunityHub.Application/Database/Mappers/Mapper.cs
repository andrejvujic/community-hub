using System.Data;

namespace CommunityHub.Application.Database.Mappers
{
    public abstract class Mapper<T> where T : class
    {
        public abstract T ToModel(IDataReader reader);
    }
}   
