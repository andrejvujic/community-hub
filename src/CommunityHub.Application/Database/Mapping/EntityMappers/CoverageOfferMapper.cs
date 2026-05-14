using CommunityHub.Application.Domain;
using CommunityHub.Application.Domain.ServiceBookings;
using CommunityHub.Application.Domain.Users;
using System.Data;

namespace CommunityHub.Application.Database.Mapping.EntityMappers
{
    public class CoverageOfferMapper : EntityMapper<CoverageOffer>
    {
        private readonly LeaveRequestMapper _leaveRequestMapper = new LeaveRequestMapper();
        private readonly BookedJobMapper _bookedJobMapper = new BookedJobMapper();
        private readonly ServiceProviderMapper _providerMapper = new ServiceProviderMapper();

        public override CoverageOffer? MapToEntity(IDataReader reader)
        {
            int idOrdinal = reader.GetOrdinal("coverage_offer_id");
            if (reader.IsDBNull(idOrdinal)) return null;
            long id = reader.GetInt64(idOrdinal);

            LeaveRequest? leaveRequest = _leaveRequestMapper.MapToEntity(reader);
            BookedJob? jobToCover = _bookedJobMapper.MapToEntity(reader);
            if (leaveRequest is null || jobToCover is null) return null;

            ServiceProvider? coveringProvider =
                _providerMapper.MapToEntity(reader.WithPrefix("covering"));
            if (coveringProvider is null) return null;

            Status status = (Status)reader.GetValue(reader.GetOrdinal("coverage_offer_status"));

            return new CoverageOffer(
                leaveRequest,
                jobToCover,
                coveringProvider,
                id: id,
                status: status
            );
        }

        public override Dictionary<string, object?> MapToQueryParameters(CoverageOffer entity)
        {
            return new Dictionary<string, object?>
            {
                { "status", entity.Status },
                { "leave_request_id", entity.LeaveRequest.Id },
                { "booked_job_id", entity.JobToCover.Id },
                { "covering_provider_id", entity.CoveringProvider.Id }
            };
        }
    }
}