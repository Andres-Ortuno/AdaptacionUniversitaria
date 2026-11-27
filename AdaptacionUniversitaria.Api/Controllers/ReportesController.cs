using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AdaptacionUniversitaria.Api.Response;
using AdaptacionUniversitaria.Core.DTOs.Reporteria;
using AdaptacionUniversitaria.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AdaptacionUniversitaria.Core.CustomEntities;
using AdaptacionUniversitaria.Core.DTOs.CustomEntitiesDto;

namespace AdaptacionUniversitaria.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReportesService _reportesService;
        private readonly IMapper _mapper;

        public ReportesController(IReportesService reportesService, IMapper mapper)
        {
            _reportesService = reportesService;
            _mapper = mapper;
        }

        /// <summary>Estudiantes (con cita aprobada) cuyo promedio de ánimo es bajo en los últimos días.</summary>
        [HttpGet("dapper/estudiantes-animo-bajo")]
        public async Task<IActionResult> GetEstudiantesAnimoBajo(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId,
            [FromQuery] int dias = 7,
            [FromQuery] decimal umbral = 2.5m)
        {
            var datos = await _reportesService.GetEstudiantesAnimoBajoAsync(usuarioId, dias, umbral);
            var dtos = _mapper.Map<IEnumerable<EstudianteAnimoDto>>(datos);
            return Ok(new ApiResponse<IEnumerable<EstudianteAnimoDto>>(dtos));
        }

        [HttpGet("dapper/cantidad-citas-por-estado")]
        public async Task<IActionResult> GetCantidadCitasEstado()
        {
            var datos = await _reportesService.GetCantidadCitasEstadoAsyn();
            var datosResult = _mapper.Map<IEnumerable<CantidadCitasEstadoDto>>(datos);
            return Ok(new ApiResponse<IEnumerable<CantidadCitasEstadoDto>>(datosResult));
        }

        [HttpGet("dapper/promedio-animo-estudiante")]
        public async Task<IActionResult> GetPromedioAnimoEstudiante()
        {
            var datos = await _reportesService.GetPromedioAnimoEstudiantesAsync();
            var datosResult = _mapper.Map<IEnumerable<PromedioAnimoEstudianteDto>>(datos);
            return Ok(new ApiResponse<IEnumerable<PromedioAnimoEstudianteDto>>(datosResult));
        }
    }
}