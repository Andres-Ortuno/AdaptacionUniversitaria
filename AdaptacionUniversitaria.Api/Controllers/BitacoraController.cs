using AdaptacionUniversitaria.Api.Response;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.QueryFilters;
using AdaptacionUniversitaria.Services.Interfaces;
using AdaptacionUniversitaria.Services.Validators;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AdaptacionUniversitaria.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        private readonly IBitacoraService _bitacoraService;
        private readonly IMapper _mapper;
        private readonly IValidator<BitacoraGuardarDto> _validator;

        public BitacoraController(IBitacoraService bitacoraService, IMapper mapper,
            IValidator<BitacoraGuardarDto> validator)
        {
            _bitacoraService = bitacoraService;
            _mapper = mapper;
            _validator = validator;
        }

        /// <summary>CU-01: historial de bitácoras (propias, o de un estudiante con cita aprobada).</summary>
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetHistorial(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            [FromQuery] BitacoraQueryFilters filters)
        {
            var bitacoras = await _bitacoraService.GetHistorialAsync(usuarioId, filters);
            var dtos = _mapper.Map<IEnumerable<BitacoraDto>>(bitacoras).ToList();

            var mensaje = dtos.Count == 0 ? "No hay registros para mostrar" : null;
            return Ok(new ApiResponse<IEnumerable<BitacoraDto>>(dtos, mensaje));
        }

        /// <summary>CU-02: registrar la bitácora del día.</summary>
        [HttpPost("dto/mapper")]
        public async Task<IActionResult> Registrar(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            [FromBody] BitacoraGuardarDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);

            var bitacora = _mapper.Map<Bitacora>(dto);
            var creada = await _bitacoraService.RegistrarAsync(usuarioId, bitacora);

            var response = new ApiResponse<BitacoraDto>(_mapper.Map<BitacoraDto>(creada), "Bitácora guardada");
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>CU-06: editar la bitácora del día.</summary>
        [HttpPut("dto/mapper/{id:int}")]
        public async Task<IActionResult> Editar(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            int id,
            [FromBody] BitacoraGuardarDto dto)
        {
            await _validator.ValidateAndThrowAsync(dto);

            var cambios = _mapper.Map<Bitacora>(dto);
            var actualizada = await _bitacoraService.EditarAsync(usuarioId, id, cambios);

            var response = new ApiResponse<BitacoraDto>(_mapper.Map<BitacoraDto>(actualizada), "Bitácora actualizada");
            return Ok(response);
        }
    }
}
