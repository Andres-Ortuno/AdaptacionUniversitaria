
using AdaptacionUniversitaria.Api.Filters;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using AdaptacionUniversitaria.Infrastructure.Mappings;
using AdaptacionUniversitaria.Infrastructure.Repositories;
using AdaptacionUniversitaria.Services.Interfaces;
using AdaptacionUniversitaria.Services.Services;
using AdaptacionUniversitaria.Services.Validators.Bitacora;
using AdaptacionUniversitaria.Services.Validators.Cita;
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
            builder.Services.AddDbContext<AdaptacionUniversitariaContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            #endregion

            // Add services to the container.
            builder.Services.AddTransient<IBitacoraRepository, BitacoraRepository>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddTransient<ICitaService, CitaService>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddAutoMapper(typeof(BitacoraProfile).Assembly);

            //Registrar Validators de FluentValidator
            builder.Services.AddScoped<BitacoraDtoValidator>();
            builder.Services.AddScoped<CrearBitacoraDtoValidator>();
            builder.Services.AddScoped<ActualizarBitacoraDtoValidator>();
            builder.Services.AddScoped<CrearCitaDtoValidator>();
            builder.Services.AddScoped<ResponderCitaDtoValidator>();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
