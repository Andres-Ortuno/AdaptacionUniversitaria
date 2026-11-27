using AdaptacionUniversitaria.Api.Response;
using AdaptacionUniversitaria.Core.DTOs;
using AdaptacionUniversitaria.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdaptacionUniversitaria.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsejerosController : ControllerBase
    {
        private readonly ICitaService _citaService;
        private readonly IMapper _mapper;

        public ConsejerosController(ICitaService citaService, IMapper mapper)
        {
            _citaService = citaService;
            _mapper = mapper;
        }

        /// <summary>CU-03: lista de consejeros activos para solicitar una cita.</summary>
        [HttpGet]
        public async Task<IActionResult> GetConsejeros(
            [FromHeader(Name = Cabeceras.UsuarioId)] int usuarioId)
        {
            var consejeros = await _citaService.GetConsejerosAsync(usuarioId);
            var dtos = _mapper.Map<IEnumerable<ConsejeroDto>>(consejeros);
            return Ok(new ApiResponse<IEnumerable<ConsejeroDto>>(dtos));
        }
    }
}
