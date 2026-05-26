using CommunityHub.Application.Domain.Buildings;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    public class Assembly
    {
        public long Id { get; private set; }

        public long BuildingId { get; init; }

        public int BuildingApartmentCount { get; init; }
        public AssemblyStatus Status { get; private set; }

        public DateTime StartsAt { get; init; }

        public List<AssemblyAttendance> Attendances { get; private set; } = [];

        public const double AttendancePercentageThreshold = 0.5;

        private Assembly() { }

        public static Assembly Create(Building building)
        {
            return new Assembly
            {
                BuildingId = building.Id,
                BuildingApartmentCount = building.ApartmentCount,
                Status = AssemblyStatus.Scheduled,
                StartsAt = DateTime.Now
            };
        } 

        public Assembly(
            long id,
            long buildingId,
            int buildingApartmentCount,
            DateTime startsAt,
            AssemblyStatus status,
            List<AssemblyAttendance>? attendances = null
        )
        {
            Id = id;
            BuildingId = buildingId;
            BuildingApartmentCount = buildingApartmentCount;
            StartsAt = startsAt;
            Status = status;
            Attendances = attendances ?? [];
        }

        public double GetAttendancePercentage()
        {
            if (BuildingApartmentCount > 0)
                return Attendances.Count / (double)BuildingApartmentCount;

            return 0.0;
        }

        public double? GetHoursLeftUntilStart()
        {
            var now = DateTime.Now;
            if (now > StartsAt) return null;

            var delta = StartsAt - now;
            return delta.TotalHours;
        }

        public bool HasSufficientAttendance => GetAttendancePercentage() >= AttendancePercentageThreshold;
        public bool CanBeConfirmed => Status is AssemblyStatus.Scheduled && HasSufficientAttendance;

        public bool ShouldBeCancelled
        {
            get
            {
                if (Status is not AssemblyStatus.Scheduled) return false;

                var hoursLeft = GetHoursLeftUntilStart();
                return (hoursLeft is null || hoursLeft < 24) && !HasSufficientAttendance;
            }
        }

        public void Confirm()
        {
            if (CanBeConfirmed)
            {
                Status = AssemblyStatus.Confirmed;
                return;
            }

            throw new InvalidOperationException(
                "Can't mark assembly as confirmed because of insufficent number of confirmed attendances"
            );
        }

        public void UndoConfirmation()
        {
            if (Status is AssemblyStatus.Confirmed)
                throw new InvalidOperationException(
                    "Only confirmed assemblies can have their confirmation undone"
                );

            Status = AssemblyStatus.Scheduled;
        }

        public void Cancel()
        {
            if (Status is AssemblyStatus.Cancelled)
                throw new InvalidOperationException("Assembly is already cancelled");

            Status = AssemblyStatus.Cancelled;
        }
    }
}
