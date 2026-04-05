using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Repositories
{
    public class AppartmentAccessRequestDbRepository
    {
        public List<AppartmentAccessRequest> GetByUserId(long id)
        {
            using IDbConnection connection = PostgresConnection.CreateConnection();

            IDbCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT
                    r.id, r.status, r.comment, r.creation_date,
                    u.id, u.username, u.password, u.name, u.surname, u.user_role,
                    a.id, a.appartment_number, a.floor_number
                    FROM appartment_access_requests r
                    INNER JOIN users u ON r.applicant_id = u.id
                    INNER JOIN appartments a ON r.appartment_id = a.id
                    WHERE u.id = @id";

            // TODO: Join with buildings table and streets table to get building address.

            IDbDataParameter applicantIdParameter = command.CreateParameter();
            applicantIdParameter.ParameterName = "@id";
            applicantIdParameter.Value = id;
            command.Parameters.Add(applicantIdParameter);

            using IDataReader reader = command.ExecuteReader();

            List<AppartmentAccessRequest> requests = new List<AppartmentAccessRequest>();
            User? applicant = null;

            while (reader.Read())
            {
                long requestId = reader.GetInt64(0);
                Status status = (Status)reader.GetValue(1);
                string? comment = reader.IsDBNull(2) ? null : reader.GetString(2);
                DateTime creationDate = reader.GetDateTime(3);

                if (applicant == null)
                {
                    long userId = reader.GetInt64(4);
                    string username = reader.GetString(5);
                    string password = reader.GetString(6);
                    string name = reader.GetString(7);
                    string surname = reader.GetString(8);
                    Role role = (Role)reader.GetValue(9);
                    applicant = new User(userId, username, password, name, surname, role);
                }

                long appartmentId = reader.GetInt64(10);
                int number = reader.GetInt32(11);
                int floorNumber = reader.GetInt32(12);

                requests.Add(
                    new AppartmentAccessRequest(
                        requestId,
                        applicant,
                        new Appartment(
                            appartmentId,
                            number,
                            floorNumber
                        ),
                        comment,
                        status,
                        creationDate
                    )
                );
            }

            return requests;
        }
    }
}
