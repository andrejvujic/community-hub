using CommunityHub.Application;
using CommunityHub.Application.Modules.Buildings.API.Public.Ads.TimeSlots;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads;
using CommunityHub.Application.Modules.Buildings.Core.Domain.Ads.TimeSlots;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CommunityHub.Ui.ViewModels.Resident.Buildings.Ads
{
    public class MyAdTimeSlotsViewModel : INotifyPropertyChanged
    {
        private readonly IAdTimeSlotService _timeSlotService = Injector.CreateInstance<IAdTimeSlotService>();

        public ObservableCollection<AdReservation> Reservations { get; } = new();
        public bool HasReservations { get => Reservations.Count > 0; }

        public Ad Ad { get; }
        public MyAdTimeSlotsViewModel(Ad ad)
        {
            Ad = ad;
        }

        public void UpdateReservations()
        {
            var timeSlots = _timeSlotService.GetReservedTimeSlotsForAd(Ad.Id);
            var reservations = GroupTimeSlotsByAd(timeSlots);

            Reservations.Clear();
            foreach (var reservation in reservations)
                Reservations.Add(reservation);

            OnPropertyChanged(nameof(HasReservations));
        }   

        private List<AdTimeSlotGroup> GroupTimeSlotsByDay(List<AdTimeSlot> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => timeSlot.StartTime.Date);

            return groups.Select(timeSlots =>
                        new AdTimeSlotGroup(
                            timeSlots.First().StartTime.Date,
                            [.. timeSlots]
                        )
                    ).ToList();
        }

        private List<AdReservation> GroupTimeSlotsByAd(List<AdTimeSlot> timeSlots)
        {
            var groups = timeSlots.GroupBy(timeSlot => {
                var reservation = timeSlot.Reservation!;
                return Ad.Type is AdType.Offer ? reservation.RequestAd!.Id : reservation.OfferAd!.Id;
            });

            return groups.Select(timeSlots => {
                var reservation = timeSlots.First().Reservation!;

                return new AdReservation(
                    Ad.Type is AdType.Offer ? reservation.RequestAd! : reservation.OfferAd!,
                    GroupTimeSlotsByDay([.. timeSlots])
                );
            }).ToList();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
                   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
