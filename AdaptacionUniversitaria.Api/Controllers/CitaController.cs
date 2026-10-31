using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using AdaptacionUniversitaria.Api.Response;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Exceptions;
using AdaptacionUniversitaria.Services.Interfaces;
using AdaptacionUniversitaria.Services.Validators.Cita;
using System.Net;

namespace AdaptacionUniversitaria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly ICitaService _citaService;
        private readonly IMapper _mapper;
        private readonly CrearCitaDtoValidator _crearValidator;
        private readonly ResponderCitaDtoValidator _responderValidator;

        public CitaController(IMapper mapper, ICitaService citaService,
            CrearCitaDtoValidator crearValidator,
            ResponderCitaDtoValidator responderValidator)
        {
            _citaService = citaService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _responderValidator = responderValidator;
        }

        #region Dto-AutoMapper

        // Listar citas. Ejemplos: ?consejeroId=2&estado=Pendiente  |  ?estudianteId=1
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetCitasDtoMapper(
            [FromQuery] int? estudianteId,
            [FromQuery] int? consejeroId,
            [FromQuery] string? estado)
        {
            var citas = await _citaService.GetCitasAsync(estudianteId, consejeroId, estado);
            var citasDto = _mapper.Map<IEnumerable<CitaDto>>(citas);
            var response = new ApiResponse<IEnumerable<CitaDto>>(citasDto);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetCitaByIdDtoMapper(int id)
        {
            var cita = await _citaService.GetCitaByIdAsync(id);
            if (cita == null)
                throw new BusinessException("Cita no encontrada.", HttpStatusCode.NotFound);

            var citaDto = _mapper.Map<CitaDto>(cita);
            var response = new ApiResponse<CitaDto>(citaDto);

            return Ok(response);
        }

        // CU-03: solicitar cita de apoyo
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> SolicitarCitaDtoMapper(CitaDto citaDto)
        {
            LanzarSiNoEsValido(await _crearValidator.ValidateAsync(citaDto));

            var cita = _mapper.Map<Cita>(citaDto);
            await _citaService.SolicitarCita(cita);

            var resultDto = _mapper.Map<CitaDto>(cita);
            var response = new ApiResponse<CitaDto>(resultDto);

            return CreatedAtAction(nameof(GetCitaByIdDtoMapper), new { id = cita.Id }, response);
        }

        // CU-04: aprobar cita (el consejero se identifica con su ID en el cuerpo)
        [HttpPut("dto/mapper/{id}/aprobar")]
        public async Task<IActionResult> AprobarCitaDtoMapper(int id, [FromBody] ResponderCitaDto dto)
        {
            LanzarSiNoEsValido(await _responderValidator.ValidateAsync(dto));

            var cita = await _citaService.AprobarCita(id, dto.ConsejeroId);

            var resultDto = _mapper.Map<CitaDto>(cita);
            return Ok(new ApiResponse<CitaDto>(resultDto));
        }

        #endregion

        // El middleware convierte la excepción en una respuesta 400 con el detalle de los campos
        private static void LanzarSiNoEsValido(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                throw new BusinessException(message: "Error de validación", details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }
        }
    }
}
