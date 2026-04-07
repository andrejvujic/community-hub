using CommunityHub.Application.Database.EntityMappers;
using CommunityHub.Application.Domain;
using System.Data;
using System.Diagnostics;

namespace CommunityHub.Application.Database.Repositories
{
    public class ApartmentAccessRequestDbRepository
    {
        private readonly ApartmentAccessRequestMapper _apartmentAccessRequestMapper = new ApartmentAccessRequestMapper();
        public List<ApartmentAccessRequest> GetByApplicantId(long id)
        {
            var query = PostgresQuery.Create(
                @"SELECT
                r.id as request_id, r.status, r.comment, r.creation_date,
                u.id as applicant_user_id, u.username as applicant_username, u.password as applicant_password, u.name as applicant_name, u.surname as applicant_surname, u.user_role as applicant_user_role,
                a.id as apartment_id, a.apartment_number, a.floor_number
                FROM apartment_access_requests r
                INNER JOIN users u ON r.applicant_id = u.id
                INNER JOIN apartments a ON r.apartment_id = a.id
                INNER JOIN buildings b ON a.building_id = b.id
                WHERE r.applicant_id = @id"
            )
            .AddParameter("id", id);

            List<ApartmentAccessRequest> requests = new List<ApartmentAccessRequest>();
            query.ExecuteReader(
                    reader => requests.Add(
                        _apartmentAccessRequestMapper.MapToEntity(reader)
                    )
            );
            return requests;
        }
    }
}
