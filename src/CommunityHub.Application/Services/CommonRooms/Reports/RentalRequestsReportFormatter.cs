using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.Dtos.CommonRooms;
using System.Globalization;

namespace CommunityHub.Application.Services.CommonRooms.Reports
{
    public class RentalRequestsReportFormatter
    {
        private static CultureInfo _culture = new("sr-Latn-RS");

        public string FormatDate(DateTime date)
            => date.ToString("dd.MM.yyyy.", _culture);

        public string FormatDateRange(DateTime from, DateTime to)
            => $"{FormatDate(from)} - {FormatDate(to)}";

        public string FormatProposedDateRange(CommonRoomRentalRequestDto request)
        {
            if (request.ProposedStartDate is null || request.ProposedEndDate is null)
                return "Nema";

            return FormatDateRange(request.ProposedStartDate.Value, request.ProposedEndDate.Value);
        }

        public string FormatAssignedDate(CommonRoomRentalRequestDto request, RentalType rentalType)
        {
            return request.AssignedDate is null || rentalType is RentalType.MultiDay
                ? "Nema"
                : FormatDate(request.AssignedDate.Value);
        }

        public string FormatResident(CommonRoomRentalRequestDto request)
        {
            if (request.Resident is null)
                return "/";

            return $"{request.Resident.Name} {request.Resident.Surname}";
        }

        public string FormatStatus(RentalRequestStatus status)
            => status switch
            {
                RentalRequestStatus.Approved => "Odobren",
                RentalRequestStatus.Waiting => "Na čekanju",
                RentalRequestStatus.Rejected => "Odbijen",
                RentalRequestStatus.WaitingDateChange => "Čeka promjenu datuma",
                _ => status.ToString()
            };

        public string FormatRentalType(RentalType type)
            => type switch
            {
                RentalType.OneDay => "Jednodnevno",
                RentalType.MultiDay => "Višednevno",
                _ => type.ToString()
            };
    }
}
