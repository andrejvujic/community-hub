using CommunityHub.Application.Domain.ResidentAssemblies;
using CommunityHub.Application.MappersDto.Apartments;
using CommunityHub.Application.Services.Dtos.ResidentAssemblies;

namespace CommunityHub.Application.MappersDto.ResidentAssemblies
{
    public class AssemblyAttendanceMapper : IMapper<AssemblyAttendance, AssemblyAttendanceDto>
    {
        private readonly ApartmentMapper _apartmentMapper = new();

        public AssemblyAttendanceDto MapToDto(AssemblyAttendance attendance)
        {
            if (attendance is null) throw new ArgumentNullException(nameof(attendance));

            return new AssemblyAttendanceDto(
                id: attendance.Id,
                assemblyId: attendance.AssemblyId,
                apartmentId: attendance.ApartmentId,
                apartment: attendance.Apartment is not null
                    ? _apartmentMapper.MapToDto(attendance.Apartment)
                    : null
            );
        }

        public AssemblyAttendance MapToEntity(AssemblyAttendanceDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            return new AssemblyAttendance(
                assemblyId: dto.AssemblyId,
                apartmentId: dto.ApartmentId,
                apartment: dto.Apartment is not null
                    ? _apartmentMapper.MapToEntity(dto.Apartment)
                    : null,
                id: dto.Id
            );
        }
    }
}
