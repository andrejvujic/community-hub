using CommunityHub.Application.Domain.Addresses;
using CommunityHub.Application.Domain.Users;

namespace CommunityHub.Application.Domain
{
    public class ServiceProvider : User
    {
        public City City { get; private set; }
        public Country Country { get; private set; }

        public List<JobListing> JobListings { get; private set; } = new List<JobListing>();
        public List<LeaveRequest> LeaveRequests { get; private set; } = new List<LeaveRequest>();

        public ServiceProvider(
            long id,
            string username,
            string password,
            string name,
            string surname,
            City city,
            Country country,
            List<JobListing>? jobListings = null,
            List<LeaveRequest>? leaveRequests = null
        ) : base(id, username, password, name, surname, Role.NeighborhoodServiceProvider)
        {
            City = city;
            Country = country;
            JobListings = jobListings ?? new List<JobListing>();
            LeaveRequests = leaveRequests ?? new List<LeaveRequest>();
        }

        public ServiceProvider CopyWith(
            long? id = null,
            string? username = null,
            string? password = null,
            string? name = null,
            string? surname = null,
            City? city = null,
            Country? country = null,
            List<JobListing>? jobListings = null,
            List<LeaveRequest>? leaveRequests = null
        )
        {
            return new ServiceProvider(
                id ?? Id,
                username ?? Username,
                password ?? Password,
                name ?? Name,
                surname ?? Surname,
                city ?? City,
                country ?? Country,
                jobListings ?? JobListings,
                leaveRequests ?? LeaveRequests
            );
        }
    }
}