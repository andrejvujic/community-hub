using CommunityHub.Application.Domain;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class LeaveRequestMapper : EntityMapper<LeaveRequest>
    {
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();

        public override LeaveRequest? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("leave_request_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            ServiceProvider? provider = _providerMapper.MapToEntity(reader);
            if (provider is null) return null;

            DateOnly startDate = DateOnly.FromDateTime(
                reader.GetDateTime(reader.GetOrdinal("start_date")));
            DateOnly endDate = DateOnly.FromDateTime(
                reader.GetDateTime(reader.GetOrdinal("end_date")));

            LeaveRequestStatus status = (LeaveRequestStatus)reader.GetValue(
                reader.GetOrdinal("leave_request_status"));
            bool isEmergency = reader.GetBoolean(reader.GetOrdinal("is_emergency"));

            return new LeaveRequest(
                provider,
                startDate,
                endDate,
                status,
                isEmergency,
                id: id
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(LeaveRequest entity)
        {
            return new Dictionary<string, object?>
            {
                { "start_date", entity.StartDate.ToDateTime(TimeOnly.MinValue) },
                { "end_date", entity.EndDate.ToDateTime(TimeOnly.MinValue) },
                { "status", entity.Status },
                { "is_emergency", entity.IsEmergency },
                { "provider_id", entity.Provider.Id }
            };
        }
    }
}