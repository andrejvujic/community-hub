using System.Collections.Generic;
using CommunityHub.Application.Domain;
using CommunityHub.Application.Database.Repositories;

namespace CommunityHub.Application.Helpers.Matching
{
    public static class NeighborhoodAddressMatcher
    {
        public static bool AddressBelongsToNeighborhood(Address? address, IEnumerable<StreetSection> sections)
        {
            if (address == null || address.Street == null) return false;
            if (!int.TryParse(address.StreetNumber, out int streetNumber)) return false;

            foreach (var s in sections)
            {
                if (s.Street == null) continue;
                if (s.Street.Id != address.Street.Id) continue;
                if (streetNumber < s.StartNumber || streetNumber > s.EndNumber) continue;

                var side = s.Side;
                if (side == ParityType.Both) return true;
                if (side == ParityType.Even && (streetNumber % 2) == 0) return true;
                if (side == ParityType.Odd && (streetNumber % 2) != 0) return true;
            }

            return false;
        }
    }
}
