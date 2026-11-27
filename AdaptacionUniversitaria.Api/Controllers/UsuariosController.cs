using AdaptacionUniversitaria.Api.Response;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AdaptacionUniversitaria.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly IValidator<UsuarioCrearDto> _crearValidator;
        private readonly IValidator<UsuarioActualizarDto> _actualizarValidator;

        public UsuariosController(IUsuarioService usuarioService, IMapper mapper,
            IValidator<UsuarioCrearDto> crearValidator,
            IValidator<UsuarioActualizarDto> actualizarValidator)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        /// <summary>CU-08: listar los usuarios con su rol y estado.</summary>
        [HttpGet]
        public async Task<IActionResult> GetUsuarios(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId)
        {
            var usuarios = await _usuarioService.GetUsuariosAsync(usuarioId);
            var dtos = _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
            return Ok(new ApiResponse<IEnumerable<UsuarioDto>>(dtos));
        }

        /// <summary>CU-08: crear un usuario (la contraseña se guarda cifrada).</summary>
        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            [FromBody] UsuarioCrearDto dto)
        {
            await _crearValidator.ValidateAndThrowAsync(dto);

            var nuevo = _mapper.Map<Usuario>(dto);
            var creado = await _usuarioService.CrearAsync(usuarioId, nuevo, dto.Password);

            var response = new ApiResponse<UsuarioDto>(_mapper.Map<UsuarioDto>(creado), "Usuario creado");
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>CU-08: editar un usuario, cambiar su rol o activarlo/desactivarlo.</summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            int id,
            [FromBody] UsuarioActualizarDto dto)
        {
            await _actualizarValidator.ValidateAndThrowAsync(dto);

            var cambios = _mapper.Map<Usuario>(dto);
            var actualizado = await _usuarioService.ActualizarAsync(usuarioId, id, cambios, dto.Password);

            return Ok(new ApiResponse<UsuarioDto>(_mapper.Map<UsuarioDto>(actualizado), "Usuario actualizado"));
        }
    }
}
