using CommunityHub.Application.Services.Dtos.Addresses;
using CommunityHub.Application.Services.Dtos.Images;
using CommunityHub.Application.Services.Dtos.Users;

namespace CommunityHub.Application.Services.Dtos.Buildings
{
    public class BuildingDto
    {
        public long Id { get; set; }
        public int FloorCount { get; set; }
        public int ApartmentCount { get; set; }
        public UserDto Manager { get; set; }
        public AddressDto Address { get; set; }
        public string DisplayAddress { get; set; }
        public List<ImageDto> Images { get; set; } = new();
        public bool HasImages { get => Images.Count > 0; }
        public ImageDto? CoverImage { get => HasImages ? Images.First() : null; }

        public BuildingDto(
            long id,
            int floorCount,
            int apartmentCount,
            UserDto manager,
            AddressDto address,
            string displayAddress,
            List<ImageDto>? images = null
        )
        {
            Id = id;
            FloorCount = floorCount;
            ApartmentCount = apartmentCount;
            Manager = manager;
            Address = address;
            DisplayAddress = displayAddress;
            Images = images ?? new List<ImageDto>();
        }
    }
}