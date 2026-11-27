# Adaptación Universitaria — API

Sistema de Acompañamiento, Bienestar Mental y Adaptación Universitaria (TecWeb1, Grupo 11).
Misma arquitectura en capas que `SocialMediaStudent`:

| Proyecto | Contenido |
|---|---|
| `AdaptacionUniversitaria.Core` | Entidades, DTOs, interfaces, excepciones, helpers |
| `AdaptacionUniversitaria.Infrastructure` | EF Core + Pomelo (MySQL), Dapper, repositorios, UnitOfWork, AutoMapper |
| `AdaptacionUniversitaria.Services` | Reglas de negocio (RN-01 a RN-11) y validadores FluentValidation |
| `AdaptacionUniversitaria.Api` | Controllers, middleware de errores, Swagger |

## Cómo correrlo

1. Ejecutar en MySQL `Database/01_crear_bd_y_datos_semilla.sql` y luego (una sola vez) `Database/02_datos_de_prueba_adicionales.sql`.
2. Poner la cadena de conexión **sin subirla a Git**:
   ```
   cd AdaptacionUniversitaria.Api
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:ConnectionMySql" "server=...;port=3306;database=dbadaptacionuniversitaria;uid=...;pwd=...;"
   ```
   (En Azure: variable de entorno `ConnectionStrings__ConnectionMySql`.)
3. `dotnet run --project AdaptacionUniversitaria.Api` y abrir `http://localhost:5233/swagger`.

## Identificación (sin login)

El documento indica que el sistema no tiene inicio de sesión. Cada request manda el header
`X-Usuario-Id` con el id del usuario (1 = estudiante, 2 = consejero, 3 = administrador en los datos semilla).
La API valida que exista, esté activa y tenga el rol permitido para cada operación.

## Endpoints (sección 3.4 del documento)

| Método | Ruta | CU | Rol |
|---|---|---|---|
| GET | `/api/bitacora/dto/mapper` | CU-01 | Estudiante, Consejero |
| POST | `/api/bitacora/dto/mapper` | CU-02 | Estudiante |
| PUT | `/api/bitacora/dto/mapper/{id}` | CU-06 | Estudiante |
| GET | `/api/consejeros` | CU-03 | Estudiante |
| POST | `/api/citas` | CU-03 | Estudiante |
| GET | `/api/citas` | CU-04/05/07 | Estudiante, Consejero |
| PUT | `/api/citas/{id}/aprobar` | CU-04 | Consejero |
| PUT | `/api/citas/{id}/rechazar` | CU-05 | Consejero |
| PUT | `/api/citas/{id}/cancelar` | CU-07 | Estudiante |
| GET | `/api/usuarios` | CU-08 | Administrador |
| POST | `/api/usuarios` | CU-08 | Administrador |
| PUT | `/api/usuarios/{id}` | CU-08 | Administrador |

Importar `Postman/AdaptacionUniversitaria.postman_collection.json` para probarlos (Anexo C).

## Códigos de error

`400` validación · `401` falta o no existe `X-Usuario-Id` · `403` rol/permiso (RN-05, RN-10) ·
`404` no encontrado · `409` conflicto de regla (RN-01, RN-03, RN-07, RN-09, RN-11).
Formato: `{ status, title, message, errors, traceId }`.
