using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mappers
{
    public class ApartmentMapper : Mapper<Apartment>
    {
        private ApartmentMapper() { }
        
        public static ApartmentMapper Instance { get => new ApartmentMapper(); }

        public override Apartment ConvertToEntity(IDataReader reader)
        {
            long id = reader.GetInt64(reader.GetOrdinal("apartment_id"));
            int number = reader.GetInt32(reader.GetOrdinal("apartment_number"));
            int floorNumber = reader.GetInt32(reader.GetOrdinal("floor_number"));
            return new Apartment(id, number, floorNumber);
        }
    }
}
