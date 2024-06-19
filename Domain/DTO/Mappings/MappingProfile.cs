
using AutoMapper;
using Domain.Entities;

namespace Domain.DTO.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Usuario, UsuarioDto>().ReverseMap();
    }
}
