
using Xceed.Wpf.AvalonDock.Properties;

namespace CommunityHub.Application.MappersDto
{
    public interface IMapper<TDomain, TDto>
    {
        TDto MapToDto(TDomain domain);
        TDomain MapToDomain(TDto dto);

        IEnumerable<TDto> MapToDto(IEnumerable<TDomain>? domains)
        {
            if (domains == null)
                throw new ArgumentNullException(nameof(domains));

            return domains.Select(MapToDto).ToList();
        }
        IEnumerable<TDomain> MapToDomain(IEnumerable<TDto>? dtos)
        {
            if (dtos == null)
                throw new ArgumentNullException(nameof(dtos));

            return dtos.Select(MapToDomain).ToList();
        }
    }
}