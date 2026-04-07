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
                request.id as request_id,
                request.status,
                request.comment,
                request.creation_date,

                applicant.id as applicant_user_id,
                applicant.username as applicant_username,
                applicant.password as applicant_password,
                applicant.name as applicant_name,
                applicant.surname as applicant_surname,
                applicant.user_role as applicant_user_role,

                apartment.id as apartment_id,
                apartment.apartment_number,
                apartment.floor_number,
                
                building.id as building_id,
                building.floor_count,
                
                building_manager.id as building_manager_user_id,
                building_manager.username as building_manager_username,
                building_manager.password as building_manager_password,
                building_manager.name as building_manager_name,
                building_manager.surname as building_manager_surname,
                building_manager.user_role as building_manager_user_role,
                
                address.id as address_id,
                address.street_number,
                address.neighborhood,
                
                street.id as street_id,
                street.name as street_name,

                city.id as city_id,
                city.name as city_name,

                country.id as country_id,
                country.name as country_name

                FROM apartment_access_requests request
                INNER JOIN users applicant ON request.applicant_id = applicant.id
                INNER JOIN apartments apartment ON request.apartment_id = apartment.id
                INNER JOIN buildings building ON apartment.building_id = building.id
                INNER JOIN users building_manager ON building.building_manager_id = building_manager.id
                INNER JOIN addresses address ON building.address_id = address.id
                INNER JOIN streets street ON address.street_id = street.id
                INNER JOIN cities city ON address.city_id = city.id
                INNER JOIN countries country ON city.country_id = country.id
                WHERE request.applicant_id = @id"
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
