[README.md](https://github.com/user-attachments/files/33786108/README.md)
# Adaptación Universitaria

Sistema de acompañamiento, bienestar mental y adaptación universitaria. Es una API (con aplicación web) que permite a los estudiantes registrar su estado de ánimo y solicitar citas de apoyo, y a los consejeros gestionar esas solicitudes y dar seguimiento.

> El sistema es un apoyo complementario: no reemplaza la atención profesional ni la atención de emergencias.

Proyecto de la materia Tecnologías Web, Carrera de Ingeniería de Sistemas, Universidad Católica Boliviana (Cochabamba).

## Integrantes

- Andres Santiago Ortuño Paredes
- Benjamin Soto Cabellos
- Sofia Rojas Peñaranda

**Docente:** Mg. Richard Canoa Cachi

## Funcionalidades

| Rol | Qué puede hacer |
|---|---|
| Estudiante | Registrar su bitácora emocional y síntomas, consultar su historial, editar la bitácora del día, solicitar citas de apoyo y cancelar las pendientes |
| Consejero | Aprobar o rechazar las citas que se le asignan y consultar el historial de los estudiantes con cita aprobada |
| Administrador | Crear y editar usuarios, asignar roles y activar o desactivar cuentas |

Casos de uso: CU-01 Consultar historial de bitácoras, CU-02 Registrar bitácora emocional, CU-03 Solicitar cita de apoyo, CU-04 Aprobar cita, CU-05 Rechazar cita, CU-06 Editar bitácora del día, CU-07 Cancelar cita pendiente, CU-08 Gestionar usuarios.

El detalle completo (reglas de negocio, modelo de datos y endpoints) está en la documentación del proyecto: **[COMPLETAR: enlace al documento]**.

## Tecnologías

- ASP.NET Core Web API (C#)
- Entity Framework Core con MySQL
- AutoMapper (mapeo entre entidades y DTOs)
- FluentValidation (validación de DTOs)
- Swagger / OpenAPI
- Postman (pruebas de la API)

## Estructura del proyecto

```
AdaptacionUniversitaria
├── AdaptacionUniversitaria.Api              # Controladores y configuración de la API
├── AdaptacionUniversitaria.Core             # Entidades, DTOs e interfaces
├── AdaptacionUniversitaria.Infrastructure   # Repositorios y acceso a datos
└── AdaptacionUniversitaria.Services         # Validadores y lógica de servicios
```

## Requisitos

- [.NET SDK](https://dotnet.microsoft.com/download) (la versión que usa el proyecto: **[COMPLETAR]**)
- MySQL 8.0 o superior (el script usa restricciones `CHECK`)
- Visual Studio 2022 o Visual Studio Code
- Postman (opcional, para probar la API)

## Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/Andres-Ortuno/AdaptacionUniversitaria.git
cd AdaptacionUniversitaria
```

### 2. Crear la base de datos

Ejecutar los scripts SQL en este orden sobre su servidor MySQL:

1. **Script 1:** crea la base `dbadaptacionuniversitaria`, las tablas (`roles`, `usuarios`, `bitacoras`, `citas`) y los datos semilla (un usuario por rol).
2. **Script 2 (opcional):** agrega usuarios, bitácoras y citas de prueba. Se ejecuta una sola vez, después del Script 1.

Los scripts están en **[COMPLETAR: carpeta del repositorio, por ejemplo `/Database`]** y también en la sección 3.3 de la documentación.

### 3. Configurar la cadena de conexión

En `appsettings.json` del proyecto Api, ajustar la conexión a su MySQL:

```json
{
  "ConnectionStrings": {
    "ConnectionMySql": "server=localhost;port=3306;database=dbadaptacionuniversitaria;user=TU_USUARIO;password=TU_CONTRASEÑA"
  }
}
```

> El nombre de la clave (`ConnectionMySql`) debe coincidir con el que lee el proyecto en `Program.cs`. Ajústenlo si es distinto.

### 4. Ejecutar la API

```bash
cd AdaptacionUniversitaria.Api
dotnet restore
dotnet run
```

Por defecto la API queda en `https://localhost:44345` (el puerto puede cambiar según `launchSettings.json`).

## Documentación de la API (Swagger)

Con la API en ejecución, abrir `https://localhost:44345/swagger`.

URL pública: **[COMPLETAR: URL de la API desplegada]**

## Ejemplos de uso

### Consultar bitácoras

```http
GET /api/bitacora/dto/mapper
```

### Registrar una bitácora

```http
POST /api/bitacora/dto/mapper
Content-Type: application/json
```

```json
{
  "estudianteId": 1,
  "fecha": "2026-10-17",
  "estadoAnimo": 4,
  "sintomas": "Cansancio leve",
  "comentario": "Día tranquilo en clases"
}
```

Reglas de validación principales:

- `estudianteId` debe corresponder a un usuario existente.
- `estadoAnimo` va de 1 a 5.
- `fecha` no puede ser futura, y un estudiante solo puede tener una bitácora por día.
- `sintomas` admite hasta 255 caracteres y `comentario` entre 5 y 500 (ambos opcionales).

### Otros endpoints de bitácora

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/bitacora/dto/mapper/{id}` | Consultar una bitácora por id |
| PUT | `/api/bitacora/dto/mapper/{id}` | Actualizar una bitácora |
| DELETE | `/api/bitacora/dto/mapper/{id}` | Eliminar una bitácora |

> Los endpoints de citas y usuarios están diseñados en la documentación (sección 3.4) y se van incorporando por evaluación.

## Usuarios de prueba

Datos ficticios del script semilla. El sistema no usa inicio de sesión.

| Rol | Correo |
|---|---|
| Estudiante | estudiante@ucb.test |
| Consejero | consejero@ucb.test |
| Administrador | admin@ucb.test |

## Convención de commits

- `feat:` qué se agregó
- `fix:` qué se arregló

Rama principal: `main` (código estable).

## Autores

Grupo 11 — Tecnologías Web, Ingeniería de Sistemas, Universidad Católica Boliviana, Cochabamba, Bolivia.
