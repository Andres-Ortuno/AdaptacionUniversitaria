using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Repositories;
using AdaptacionUniversitaria.Api.Response;
using AdaptacionUniversitaria.Services.Validators.Bitacora;

namespace AdaptacionUniversitaria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitacoraController: ControllerBase
    {
        private readonly IBitacoraRepository _bitacoraRepository;
        private readonly IMapper _mapper;
        private readonly CrearBitacoraDtoValidator _crearValidator;
        private readonly ActualizarBitacoraDtoValidator _actualizarValidator;
        public BitacoraController(IMapper mapper, IBitacoraRepository bitacoraRepository,
            CrearBitacoraDtoValidator crearValidator, ActualizarBitacoraDtoValidator actualizarValidator)
        {
            _bitacoraRepository = bitacoraRepository;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        #region Sin DTOs
        [HttpGet]
        public async Task<IActionResult> GetBitacora()
        {
            var bitacoras = await _bitacoraRepository.GetAllBitacorasAsync();
            return Ok(bitacoras);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBitacoraById(int id)
        {
            var bitacora = await _bitacoraRepository.GetBitacoraByIdAsync(id);
            return Ok(bitacora);
        }

        [HttpPost]
        public async Task<IActionResult> InsertBitacora(Bitacora newBitacora)
        {
            await _bitacoraRepository.InsertBitacora(newBitacora);
            return Created($"api/bitacora/{newBitacora.Id}", newBitacora);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBitacora(Bitacora bitacora)
        {
            await _bitacoraRepository.UpdateBitacora(bitacora);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBitacora(Bitacora bitacora)
        {
            await _bitacoraRepository.DeleteBitacora(bitacora);
            return NoContent();
        }
        #endregion

        #region Con DTOs
        [HttpGet("dto")]
        public async Task<IActionResult> GetBitacoraDto()
        {

            var bitacoras = await _bitacoraRepository.GetAllBitacorasAsync();
            var bitacoraDto = bitacoras.Select(b => new BitacoraDto
            {
                Id = b.Id,
                EstudianteId = b.EstudianteId,
                Fecha = b.Fecha.ToString("dd-MM-yyyy"),
                EstadoAnimo = b.EstadoAnimo,
                Sintomas = b.Sintomas,
                Comentario = b.Nota,
                CreadoEn = b.CreadoEn.ToString("dd-MM-yyyy")
            });
            return Ok(bitacoraDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<IActionResult> GetBitacoraByIdDto(int id)
        {
            var bitacora = await _bitacoraRepository.GetBitacoraByIdAsync(id);
            var bitacoraDto = new BitacoraDto
            {
                Id = bitacora.Id,
                EstudianteId = bitacora.EstudianteId,
                Fecha = bitacora.Fecha.ToString("dd-MM-yyyy"),
                EstadoAnimo = bitacora.EstadoAnimo,
                Sintomas = bitacora.Sintomas,
                Comentario = bitacora.Nota,
                CreadoEn = bitacora.CreadoEn.ToString("dd-MM-yyyy")
            };
            return Ok(bitacoraDto);
        }

        [HttpPost("dto")]
        public async Task<IActionResult> InsertBitacoraDto(Bitacora newBitacora)
        {
            var bitacora = new Bitacora
            {
                Id = newBitacora.Id,
                EstudianteId = newBitacora.EstudianteId,
                Fecha = Convert.ToDateTime(newBitacora.Fecha),
                EstadoAnimo = newBitacora.EstadoAnimo,
                Sintomas = newBitacora.Sintomas,
                Nota = newBitacora.Nota,
                CreadoEn = newBitacora.CreadoEn
            };
            await _bitacoraRepository.InsertBitacora(bitacora);
            return Created($"api/bitacora/{bitacora.Id}", bitacora);
        }

        [HttpPut("dto")]
        public async Task<IActionResult> UpdateBitacoraDto(int id, [FromBody] BitacoraDto bitacoraDto)
        {
            if (id != bitacoraDto.Id)
                return BadRequest("El ide del bitacora no coincide");

            var bitacora = await _bitacoraRepository.GetBitacoraByIdAsync(id);
            if (bitacora == null)
                return NotFound("Bitacora no encontrado");

            //Mapear valores DTO en la entidad

            bitacora.Id = bitacoraDto.Id;
            bitacora.EstudianteId = bitacoraDto.EstudianteId;
            bitacora.Fecha = Convert.ToDateTime(bitacoraDto.Fecha);
            bitacora.EstadoAnimo = bitacoraDto.EstadoAnimo;
            bitacora.Sintomas = bitacoraDto.Sintomas;
            bitacora.Nota = bitacoraDto.Comentario;
            bitacora.CreadoEn = Convert.ToDateTime(bitacoraDto.CreadoEn);

            await _bitacoraRepository.UpdateBitacora(bitacora);
            return Ok(bitacora);
        }

        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteBitacoraDto(int id)
        {
            var bitacora = await _bitacoraRepository.GetBitacoraByIdAsync(id);
            if (bitacora == null)
                return NotFound("Bitacora no encontrado.");

            await _bitacoraRepository.DeleteBitacora(bitacora);

            return NoContent(); // 204 sin contenido
        }
        #endregion

        #region Dto-AutoMapper
        [HttpGet("dto/mapper")]
        public async Task<IActionResult> GetBitacorasDtoMapper()
        {
            var bitacoras = await _bitacoraRepository.GetAllBitacorasAsync();
            var bitacoraDto = _mapper.Map<IEnumerable<BitacoraDto>>(bitacoras);
            //var bitacoraDto = bitacoras.Select(p => new BitacoraDto
            //{
            //    Id = p.Id,
            //    UserId = p.UserId,
            //    Date = p.Date,
            //    Description = p.Description,
            //    Imagen = p.Imagen
            //});

            var response = new ApiResponse<IEnumerable<BitacoraDto>>(bitacoraDto);

            return Ok(response);
        }

        [HttpGet("dto/mapper/{id}")]
        public async Task<IActionResult> GetBitacorasByIdDtoMapper(int id)
        {
            var bitacora = await _bitacoraRepository.GetBitacoraByIdAsync(id);
            var bitacoraDto = _mapper.Map<BitacoraDto>(bitacora);
            var response = new ApiResponse<BitacoraDto>(bitacoraDto);

            return Ok(response);
        }

        [HttpPost("dto/mapper/")]
        public async Task<IActionResult> InsertBitacoraDtoMapper(BitacoraDto bitacoraDto)
        {
            // Validar el DTO
            var validationResult = await _crearValidator.ValidateAsync(bitacoraDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            try
            {
                var bitacora = _mapper.Map<Bitacora>(bitacoraDto);
                await _bitacoraRepository.InsertBitacora(bitacora);

                var resultDto = _mapper.Map<BitacoraDto>(bitacora);

                var response = new ApiResponse<BitacoraDto>(resultDto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear el bitacora", error = ex.Message });
            }
        }

        [HttpPut("dto/mapper/{id}")]
        public async Task<IActionResult> UpdateBitacoraDtoMapper(int id, [FromBody] BitacoraDto bitacoraDto)
        {
            if (id != bitacoraDto.Id)
                return BadRequest("El ID del bitacora no coincide.");

            // Validar el DTO
            var validationResult = await _actualizarValidator.ValidateAsync(bitacoraDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    message = "Error de validación",
                    errors = validationResult.Errors.Select(e => new
                    {
                        field = e.PropertyName,
                        error = e.ErrorMessage
                    })
                });
            }

            var bitacora = await _bitacoraRepository.GetBitacoraByIdAsync(id);
            if (bitacora == null)
                return NotFound(new { message = "Bitacora no encontrado." });

            try
            {
                _mapper.Map(bitacoraDto, bitacora);
                await _bitacoraRepository.UpdateBitacora(bitacora);

                var resultDto = _mapper.Map<BitacoraDto>(bitacora);
                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el bitacora", error = ex.Message });
            }
        }

        [HttpDelete("dto/mapper/{id}")]
        public async Task<IActionResult> DeleteBitacoraDtoMapper(int id)
        {
            // Validar que el ID sea válido
            if (id <= 0)
                return BadRequest(new { message = "El ID debe ser mayor que cero." });

            var bitacora = await _bitacoraRepository.GetBitacoraByIdAsync(id);
            if (bitacora == null)
                return NotFound(new { message = "Bitacora no encontrado." });

            try
            {
                await _bitacoraRepository.DeleteBitacora(bitacora);
                return NoContent(); // 204 sin contenido
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el bitacora", error = ex.Message });
            }
        }

        #endregion
    }
}
