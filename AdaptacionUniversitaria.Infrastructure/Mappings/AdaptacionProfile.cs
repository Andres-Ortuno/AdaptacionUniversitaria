using AdaptacionUniversitaria.Core.CustomEntities;
using AdaptacionUniversitaria.Core.CustomEntities.Reporteria;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Core.DTOs.CustomEntitiesDto;
using AdaptacionUniversitaria.Core.DTOs.Reporteria;
using AdaptacionUniversitaria.Core.Entities;
using AutoMapper;

namespace AdaptacionUniversitaria.Infrastructure.Mappings
{
    public class AdaptacionProfile : Profile
    {
        public AdaptacionProfile()
        {
            // Bitácoras
            CreateMap<Bitacora, BitacoraDto>();
            CreateMap<BitacoraGuardarDto, Bitacora>();
            CreateMap<EstudianteAnimo, EstudianteAnimoDto>();

            // Citas (Estudiante.Nombres -> EstudianteNombres se mapea por "flattening")
            CreateMap<Cita, CitaDto>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));
            CreateMap<CitaCrearDto, Cita>();

            // Usuarios (Rol.Nombre -> RolNombre por "flattening")
            CreateMap<Usuario, ConsejeroDto>();
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<UsuarioCrearDto, Usuario>();
            CreateMap<UsuarioActualizarDto, Usuario>();

            CreateMap<CantidadCitasEstado, CantidadCitasEstadoDto>();
            CreateMap<PromedioAnimoEstudiante, PromedioAnimoEstudianteDto>();
            CreateMap<EstudianteAnimoBajoSinCita, EstudianteAnimoBajoSinCitaDto>(); 
        }
    }
}
