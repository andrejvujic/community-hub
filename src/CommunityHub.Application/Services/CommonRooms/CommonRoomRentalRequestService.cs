using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Domain.CommonRooms.RepositoryInterfaces;
using CommunityHub.Application.MappersDto.CommonRooms;
using CommunityHub.Application.Services.Dtos.CommonRooms;

namespace CommunityHub.Application.Services.CommonRooms
{
    public enum RentalRequestValidationResult
    {
        Valid,
        PendingRequestExists
    }

    public class CommonRoomRentalRequestService
    {
        private readonly ICommonRoomRentalRequestRepository _rentalRequestRepository 
            = Injector.CreateInstance<ICommonRoomRentalRequestRepository>();

        private readonly CommonRoomRentalRequestMapper _rentalRequestMapper = new();

        public void Create(CommonRoomRentalRequest request)
            => _rentalRequestRepository.Create(request);

        public List<CommonRoomRentalRequest> GetByResidentAndCommonRoom(long residentId, long commonRoomId)
            => _rentalRequestRepository.GetByResidentAndCommonRoom(residentId, commonRoomId);

        public void Cancel(CommonRoomRentalRequest request)
        {
            if (!request.IsPending)
                throw new InvalidOperationException("Only pending requests can be cancelled.");

            _rentalRequestRepository.Delete(request.Id);
        }

        public CommonRoomRentalRequest AcceptProposedDateRange(CommonRoomRentalRequest request)
        {
            if (!request.HasProposedDateRange)
                throw new InvalidOperationException("Request doesn't have a proposed date range.");

            request.AcceptProposedDateRange();
            _rentalRequestRepository.Update(request);

            return request;
        }

        public RentalRequestValidationResult ValidateRentalRequest(long residentId, long commonRoomId)
        {
            if (HasPendingRequest(residentId, commonRoomId))
                return RentalRequestValidationResult.PendingRequestExists;

            return RentalRequestValidationResult.Valid;
        }

        public bool HasPendingRequest(long residentId, long commonRoomId)
        {
            var requests = _rentalRequestRepository.GetByResidentAndCommonRoom(
                residentId,
                commonRoomId
            );
            return requests.Any(request => request.IsPending);
        }

        public List<DateTime> GetReservedDates(long commonRoomId, long buildingId)
            => _rentalRequestRepository.GetReservedDates(commonRoomId, buildingId);

        public List<CommonRoomRentalRequestDto> GetAllByBuildingId(long buildingId)
        {
            var rentalRequests = _rentalRequestRepository.GetAllByBuildingId(buildingId);
            return rentalRequests.Select(_rentalRequestMapper.MapToDto).ToList();
        }

        public void CheckAndUpdateRentalDateStatuses(long buildingId)
        {
            var rentalRequests = _rentalRequestRepository.GetAllByBuildingId(buildingId);

            foreach (var request in rentalRequests)
            {
                if (request.Status != RentalRequestStatus.Waiting)
                    continue;

                var type = request.CommonRoom!.RentalType;

                switch (type)
                {
                    case RentalType.OneDay:
                        UpdateOneDayRequestStatus(request, buildingId);
                        break;
                    case RentalType.MultiDay:
                        UpdateMultiDayRequestStatus(request, buildingId);
                        break;
                }
            }
        }
        private void UpdateMultiDayRequestStatus(CommonRoomRentalRequest request, long buildingId)
        {
            var reservedDates = _rentalRequestRepository.GetReservedDates(request.CommonRoom!.Id, buildingId);

            bool hasConflict = reservedDates.Any(d => d >= request.RequestedStartDate && d <= request.RequestedEndDate);

            if (!hasConflict)
            {
                request = request.CopyWith(status: RentalRequestStatus.Approved);
                _rentalRequestRepository.Update(request);
            }
        }
        private void UpdateOneDayRequestStatus(CommonRoomRentalRequest request, long buildingId)
        {
            var reservedDates = _rentalRequestRepository.GetReservedDates(request.CommonRoom!.Id, buildingId);
            bool isAllReserved = true;

            DateTime current = request.RequestedStartDate;
            while (current <= request.RequestedEndDate)
            {
                if (!reservedDates.Contains(current))
                {
                    isAllReserved = false;
                    break;
                }
                current = current.AddDays(1);
            }

            if (isAllReserved)
            {
                request = request.CopyWith(status: RentalRequestStatus.Rejected);
                _rentalRequestRepository.Update(request);
            }
        }
        public List<DateTime> GetSuggestedOneDayDates(CommonRoomRentalRequestDto request, long buildingId)
        {
            var reservedDates = _rentalRequestRepository.GetReservedDates(request.CommonRoom!.Id, buildingId);
            var suggestedDates = new List<DateTime>();
            DateTime current = request.RequestedStartDate;

            while (current <= request.RequestedEndDate)
            {
                if (!reservedDates.Contains(current))
                    suggestedDates.Add(current);

                current = current.AddDays(1);
            }

            return suggestedDates;
        }
        public List<(DateTime StartDate, DateTime EndDate)> GetSuggestedMultiDayRanges(CommonRoomRentalRequestDto request, long buildingId)
        {
            var reservedDates = _rentalRequestRepository.GetReservedDates(request.CommonRoom!.Id, buildingId);
            var suggestedRanges = new List<(DateTime StartDate, DateTime EndDate)>();

            int duration = (request.RequestedEndDate - request.RequestedStartDate).Days + 1;
            int foundCount = 0;
            DateTime candidateStart = request.RequestedStartDate.AddDays(1);

            while (foundCount < 3)
            {
                DateTime candidateEnd = candidateStart.AddDays(duration - 1);
                bool isFree = true;
                DateTime day = candidateStart;

                while (day <= candidateEnd)
                {
                    if (reservedDates.Contains(day))
                    {
                        isFree = false;
                        break;
                    }
                    day = day.AddDays(1);
                }

                if (isFree)
                {
                    suggestedRanges.Add((candidateStart, candidateEnd));
                    foundCount++;
                }

                candidateStart = candidateStart.AddDays(1);
            }

            return suggestedRanges;
        }
        public void ApproveOneDaySuggestion(CommonRoomRentalRequestDto request, DateTime day)
        {
            var domainRequest = _rentalRequestMapper.MapToDomain(request);
            domainRequest = domainRequest.CopyWith(status: RentalRequestStatus.Approved, assignedDate: day);
            _rentalRequestRepository.Update(domainRequest);
        }
        public void AcceptMultiDaySuggestion(CommonRoomRentalRequestDto request, DateTime proposedStartDate, DateTime proposedEndDate)
        {
            var domainRequest = _rentalRequestMapper.MapToDomain(request);
            domainRequest = domainRequest.CopyWith(status: RentalRequestStatus.WaitingDateChange, proposedStartDate: proposedStartDate, proposedEndDate: proposedEndDate);
            _rentalRequestRepository.Update(domainRequest);
        }
    }
}