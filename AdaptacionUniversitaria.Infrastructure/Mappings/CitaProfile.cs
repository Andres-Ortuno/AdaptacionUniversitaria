using AutoMapper;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Infrastructure.Mappings
{
    public class CitaProfile : Profile
    {
        public CitaProfile()
        {
            // Cita <-> CitaDto: los nombres coinciden, no hace falta configurar campos
            CreateMap<Cita, CitaDto>().ReverseMap();
        }
    }
}
