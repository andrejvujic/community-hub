namespace CommunityHub.Application.MappersDto
{
    public interface IMapper<TDomain, TDto>
    {
        TDto MapToDto(TDomain domain);
        TDomain MapToEntity(TDto dto);
    }
}