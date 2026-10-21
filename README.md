# SC-701 — Programación Avanzada en Web

Proyecto desarrollado para el curso **SC-701 Programación Avanzada en Web** de la Universidad Fidélitas.

## Descripción

La aplicación fue desarrollada utilizando **ASP.NET Core .NET 8** y aplica los principales conceptos vistos durante el curso, incluyendo:

- ASP.NET Core MVC
- Web API
- Entity Framework Core
- Repository Pattern
- DTOs
- Services
- Dependency Injection
- Razor Views
- Partial Views
- CRUD
- Paginación
- Async / Await
- REST y JSON

## Estructura del proyecto

La solución está dividida en diferentes proyectos:

- `PAW.Models`
- `PAW.DataAccess`
- `PAW.Architecture`
- `PAW.API`
- `PAW.Web`

Esta separación permite mantener organizadas las responsabilidades de acceso a datos, lógica, servicios, API e interfaz web.

## Funcionalidades principales

La aplicación permite gestionar diferentes entidades mediante operaciones CRUD, utilizando una arquitectura reutilizable entre los distintos módulos.

Las operaciones de creación, edición, detalle y eliminación utilizan **Partial Views** y componentes compartidos para mantener una interfaz consistente.

Los listados incluyen paginación con un máximo de **25 registros por página**.

## Tecnologías utilizadas

- C#
- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQL Server Express
- Razor
- Bootstrap
- JavaScript

## Ejecución

1. Abrir la solución en Visual Studio.
2. Configurar la conexión a SQL Server.
3. Ejecutar `PAW.API`.
4. Ejecutar `PAW.Web`.
5. Utilizar Swagger para probar los endpoints del API.

## Autor

**Kenny Salazar**  
Ingeniería en Sistemas  
Universidad Fidélitas

**Curso:** SC-701 — Programación Avanzada en Web
