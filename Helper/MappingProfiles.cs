using AutoMapper;
using SuperFreshAPI.Dto;
using SuperFreshAPI.Models;
namespace SuperFreshAPI.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles() 
        {
            
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<Categoria, CategoriaDto>().ReverseMap();

        }
    }
}
