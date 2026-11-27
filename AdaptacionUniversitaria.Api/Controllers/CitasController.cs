using AdaptacionUniversitaria.Api.Response;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.QueryFilters;
using AdaptacionUniversitaria.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AdaptacionUniversitaria.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ICitaService _citaService;
        private readonly IMapper _mapper;
        private readonly IValidator<CitaCrearDto> _crearValidator;
        private readonly IValidator<CitaRechazarDto> _rechazarValidator;

        public CitasController(ICitaService citaService, IMapper mapper,
            IValidator<CitaCrearDto> crearValidator,
            IValidator<CitaRechazarDto> rechazarValidator)
        {
            _citaService = citaService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _rechazarValidator = rechazarValidator;
        }

        /// <summary>Citas propias (estudiante) o asignadas (consejero). Filtro opcional: ?estado=Pendiente</summary>
        [HttpGet]
        public async Task<IActionResult> GetCitas(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            [FromQuery] CitaQueryFilters filters)
        {
            var citas = await _citaService.GetCitasAsync(usuarioId, filters);
            var dtos = _mapper.Map<IEnumerable<CitaDto>>(citas);
            return Ok(new ApiResponse<IEnumerable<CitaDto>>(dtos));
        }

        /// <summary>CU-03: solicitar una cita de apoyo.</summary>
        [HttpPost]
        public async Task<IActionResult> Solicitar(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            [FromBody] CitaCrearDto dto)
        {
            await _crearValidator.ValidateAndThrowAsync(dto);

            var cita = _mapper.Map<Cita>(dto);
            var creada = await _citaService.SolicitarAsync(usuarioId, cita);

            var response = new ApiResponse<CitaDto>(_mapper.Map<CitaDto>(creada), "Cita solicitada");
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>CU-04: aprobar una cita pendiente.</summary>
        [HttpPut("{id:int}/aprobar")]
        public async Task<IActionResult> Aprobar(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            int id)
        {
            var cita = await _citaService.AprobarAsync(usuarioId, id);
            return Ok(new ApiResponse<CitaDto>(_mapper.Map<CitaDto>(cita), "Cita aprobada"));
        }

        /// <summary>CU-05: rechazar una cita pendiente indicando el motivo.</summary>
        [HttpPut("{id:int}/rechazar")]
        public async Task<IActionResult> Rechazar(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            int id,
            [FromBody] CitaRechazarDto dto)
        {
            await _rechazarValidator.ValidateAndThrowAsync(dto);

            var cita = await _citaService.RechazarAsync(usuarioId, id, dto.MotivoRechazo);
            return Ok(new ApiResponse<CitaDto>(_mapper.Map<CitaDto>(cita), "Cita rechazada"));
        }

        /// <summary>CU-07: cancelar una cita pendiente.</summary>
        [HttpPut("{id:int}/cancelar")]
        public async Task<IActionResult> Cancelar(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            int id)
        {
            var cita = await _citaService.CancelarAsync(usuarioId, id);
            return Ok(new ApiResponse<CitaDto>(_mapper.Map<CitaDto>(cita), "Cita cancelada"));
        }
    }
}
