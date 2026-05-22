using CommunityHub.Application.MappersDto;

namespace CommunityHub.Application.Services
{
    public class BaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IMapper<TEntity, TDto> _mapper;

        public BaseService(IMapper<TEntity, TDto> mapper)
        {
            _mapper = mapper;
        }

        public TEntity MapToEntity(TDto dto) => _mapper.MapToDomain(dto);

        public List<TEntity> MapToEntity(List<TDto> dtos)
            => dtos
                .Select(_mapper.MapToDomain)
                .ToList();

        public TDto MapToDto(TEntity entity) => _mapper.MapToDto(entity);

        public List<TDto> MapToDto(List<TEntity> entities)
            => entities
                .Select(_mapper.MapToDto)
                .ToList();
    }
}
