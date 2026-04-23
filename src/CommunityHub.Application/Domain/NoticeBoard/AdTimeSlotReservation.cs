using System;
using System.Collections.Generic;
using System.Text;

namespace CommunityHub.Application.Domain.NoticeBoard
{
    public class AdTimeSlotReservation
    {
        public long Id { get; }
        public long FirstTimeSlotId { get;  }
        public long SecondTimeSlotId { get; }

        public AdTimeSlotReservation(
            long firstTimeSlotId,
            long secondTimeSlotId,
            long id = 0
        )
        {
            FirstTimeSlotId = firstTimeSlotId;  
            SecondTimeSlotId = secondTimeSlotId;
            Id = id;
        }

        public AdTimeSlotReservation CopyWith(
            long? id = null,
            long? firstTimeSlotId = null,
            long? secondTimeSlotId = null
        ) => new AdTimeSlotReservation(
                firstTimeSlotId ?? FirstTimeSlotId,
                secondTimeSlotId ?? SecondTimeSlotId,
                id ?? Id
            );
    }
}
