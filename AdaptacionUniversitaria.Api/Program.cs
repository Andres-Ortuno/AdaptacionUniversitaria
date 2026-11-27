using AdaptacionUniversitaria.Api.Filters;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using AdaptacionUniversitaria.Infrastructure.Mappings;
using AdaptacionUniversitaria.Infrastructure.Repositories;
using AdaptacionUniversitaria.Services.Interfaces;
using AdaptacionUniversitaria.Services.Services;
using AdaptacionUniversitaria.Services.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AdaptacionUniversitaria.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configurar la BD MySql
            var connectionString = builder.Configuration.GetConnectionString("ConnectionMySql");
            // Versión fija (MySQL 8) para no abrir una conexión al arrancar la API.
            builder.Services.AddDbContext<AdaptacionUniversitariaContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 0))));
            #endregion

            #region Inyección de dependencias
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            builder.Services.AddScoped<IDapperContext, DapperContext>();

            builder.Services.AddScoped<IBitacoraService, BitacoraService>();
            builder.Services.AddScoped<ICitaService, CitaService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IReportesService, ReportesService>();
            #endregion

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                // Las validaciones las hace FluentValidation y las responde el middleware global
                options.SuppressModelStateInvalidFilter = true;
            });

            // OpenAPI + interfaz Swagger
            builder.Services.AddOpenApi();

            builder.Services.AddAutoMapper(typeof(AdaptacionProfile).Assembly);

            // Validadores de FluentValidation (todos los del proyecto Services)
            builder.Services.AddValidatorsFromAssemblyContaining<BitacoraGuardarDtoValidator>();

            // El frontend web consume esta API desde otro origen
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Swagger disponible también en Azure (Anexo B: URL pública de la API)
            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Adaptación Universitaria API v1");
                options.RoutePrefix = "swagger";
            });

            app.UseHttpsRedirection();
            app.UseCors();

            app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
            app.MapControllers();

            app.Run();
        }
    }
}
