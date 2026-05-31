using CommunityHub.Application.Domain.Buildings;

namespace CommunityHub.Application.Domain.ResidentAssemblies
{
    public class Assembly
    {
        private readonly List<AssemblyAttendance> _attendances = [];

        public long Id { get; private set; }

        public long BuildingId { get; init; }

        public int BuildingApartmentCount { get; init; }

        public AssemblyStatus Status { get; private set; }

        public DateTime StartsAt { get; init; }

        public IReadOnlyList<AssemblyAttendance> Attendances => _attendances.AsReadOnly();

        public const double AttendancePercentageThreshold = 0.5;

        private Assembly() { }

        public static Assembly Create(Building building, DateTime startsAt)
        {
            return new Assembly
            {
                BuildingId = building.Id,
                BuildingApartmentCount = building.ApartmentCount,
                Status = AssemblyStatus.Scheduled,
                StartsAt = startsAt
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

            _attendances = attendances ?? [];
        }

        public bool CanChange()
            => Status is not AssemblyStatus.Cancelled && StartsAt > DateTime.Now.AddHours(24);

        public bool HasRegisteredAttendance(long apartmentId)
            => _attendances.Any(attendance => attendance.ApartmentId == apartmentId);

        public void RegisterAttendance(AssemblyAttendance attendance)
        {
            if (HasRegisteredAttendance(attendance.ApartmentId))
                throw new InvalidOperationException("Attendance already registered");

            _attendances.Add(attendance);
        }

        public void UnregisterAttendance(AssemblyAttendance attendance)
        {
            if (!HasRegisteredAttendance(attendance.ApartmentId))
                throw new InvalidOperationException("Attendance wasn't registered");

            _attendances.Remove(attendance);
        }

        public void RefreshStatus()
        {
            if (CanBeConfirmed)
            {
                Confirm();
                return;
            }

            if (CanBeUnconfirmed)
            {
                Unconfirm();
                return;
            }

            if (CanBeCancelled)
                Cancel();
        }


        public double GetAttendancePercentage()
        {
            if (BuildingApartmentCount <= 0)
                return 0.0;

            return _attendances.Count / (double)BuildingApartmentCount;
        }

        public double? GetHoursLeftUntilStart()
        {
            var now = DateTime.Now;

            if (now > StartsAt)
                return null;

            return (StartsAt - now).TotalHours;
        }

        public bool HasSufficientAttendance =>
            GetAttendancePercentage() >= AttendancePercentageThreshold;

        public bool CanBeConfirmed =>
            Status is AssemblyStatus.Scheduled && HasSufficientAttendance;

        public bool CanBeUnconfirmed =>
            Status is AssemblyStatus.Confirmed && !HasSufficientAttendance;

        public bool CanBeCancelled
        {
            get
            {
                if (Status is not AssemblyStatus.Scheduled)
                    return false;

                var hoursLeft = GetHoursLeftUntilStart();

                return (hoursLeft is null || hoursLeft < 24) && !HasSufficientAttendance;
            }
        }

        public void Confirm()
        {
            if (!CanBeConfirmed)
                throw new InvalidOperationException(
                    "Can't confirm assembly because there is an insufficient number of confirmed attendances"
                );

            Status = AssemblyStatus.Confirmed;
        }

        public void Unconfirm()
        {
            if (!CanBeUnconfirmed)
                throw new InvalidOperationException(
                    "Only confirmed assemblies with insufficient attendance can have their confirmation undone"
                );

            Status = AssemblyStatus.Scheduled;
        }

        public void Cancel()
        {
            if (!CanBeCancelled)
                throw new InvalidOperationException("Assembly cannot be cancelled");

            Status = AssemblyStatus.Cancelled;
        }
    }
}
