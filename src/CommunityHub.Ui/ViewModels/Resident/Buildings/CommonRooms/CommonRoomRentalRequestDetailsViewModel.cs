using CommunityHub.Application.Domain.CommonRooms.RentalRequests;
using CommunityHub.Application.Services.CommonRooms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.CommonRooms
{
    public class CommonRoomRentalRequestDetailsViewModel : INotifyPropertyChanged
    {
        private readonly CommonRoomRentalRequestService _commonRentalRequestService = new();

        public CommonRoomRentalRequest Request { get; private set; }

        public CommonRoomRentalRequestDetailsViewModel(CommonRoomRentalRequest request)
        {
            Request = request;
        }

        public void CancelRequest()
            => _commonRentalRequestService.Cancel(Request);

        public void AcceptProposedRequestDateRange()
        {
            Request = _commonRentalRequestService.AcceptProposedDateRange(Request);
            OnPropertyChanged(nameof(Request));
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
