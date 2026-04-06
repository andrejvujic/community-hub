using CommunityHub.Application.Database.Mappers;
using CommunityHub.Application.Domain;

namespace CommunityHub.Application.Database.Repositories
{
    public class ApartmentAccessRequestDbRepository
    {
        public List<ApartmentAccessRequest> GetByApplicantId(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                r.id as request_id, r.status, r.comment, r.creation_date,
                u.id as user_id, u.username, u.password, u.name, u.surname, u.user_role,
                a.id as apartment_id, a.apartment_number, a.floor_number
                FROM apartment_access_requests r
                INNER JOIN users u ON r.applicant_id = u.id
                INNER JOIN apartments a ON r.apartment_id = a.id
                INNER JOIN buildings b ON a.building_id = b.id
                WHERE r.applicant_id = @id"
            )
            .AddParameter("id", id);
            return query.ExecuteVector(ApartmentAccessRequestMapper.Instance);
        }
    }
}
