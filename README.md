# SC-701 — Programación Avanzada en Web

## Proyecto PAW

Proyecto desarrollado para el curso **SC-701 Programación Avanzada en Web** de la Universidad Fidélitas.

El objetivo del proyecto es aplicar los conceptos estudiados durante el curso mediante una aplicación desarrollada con **ASP.NET Core .NET 8**, utilizando MVC, Web API, Entity Framework Core, Repository Pattern, DTOs, Services, Partial Views, Dependency Injection y consumo de servicios REST.

---

## Tecnologías utilizadas

- C#
- .NET 8
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core
- Razor Views
- SQL Server Express
- Bootstrap 5
- JavaScript
- REST / JSON
- Dependency Injection

---

## Estructura de la solución

La solución se encuentra dividida en diferentes proyectos con responsabilidades específicas:

```text
PAW.Models
PAW.DataAccess
PAW.Architecture
PAW.API
PAW.Web
```

### PAW.Models

Contiene las entidades y DTOs utilizados por la aplicación.

### PAW.DataAccess

Contiene:

- `ProductDbContext`
- Repositories
- Acceso a datos mediante Entity Framework Core

### PAW.Architecture

Contiene componentes reutilizables como:

- `RepositoryBase<T>`
- `RestProvider`
- `JsonProvider`
- Clases base para Services

### PAW.API

Contiene los Controllers que exponen los endpoints REST utilizados para realizar las operaciones CRUD.

### PAW.Web

Contiene:

- MVC Controllers
- Services
- Razor Views
- Partial Views
- JavaScript
- Interfaz de usuario

---

## Arquitectura utilizada

El flujo principal de la aplicación es:

```text
Usuario
   ↓
PAW.Web
   ↓
MVC Controller
   ↓
Service
   ↓
RestProvider
   ↓
PAW.API
   ↓
API Controller
   ↓
Repository
   ↓
RepositoryBase
   ↓
Entity Framework Core
   ↓
SQL Server
```

La información es enviada entre el Web y el API utilizando JSON.

---

## Repository Pattern

Se utiliza `RepositoryBase<T>` para centralizar las operaciones comunes de acceso a datos.

Entre las operaciones disponibles se encuentran:

```text
CreateAsync
ReadAsync
FindAsync
UpdateAsync
DeleteAsync
ExistsAsync
SaveChangesAsync
```

Los repositories específicos heredan de esta implementación.

Ejemplo:

```csharp
public class ProductRepository :
    RepositoryBase<Product>,
    IProductRepository
{
}
```

---

## DTOs

Los DTOs son utilizados para transportar información entre las diferentes capas de la aplicación.

Ejemplo del flujo:

```text
Entity
  ↓
DTO
  ↓
JSON
  ↓
API
  ↓
Service
  ↓
MVC
  ↓
Razor View
```

---

## CRUD implementados

Se implementaron las operaciones:

- Create
- Read
- Update
- Delete
- Details

para las siguientes entidades:

| Entidad | CRUD |
|---|---|
| Product | ✅ Completo |
| Category | ✅ Completo |
| Supplier | ✅ Completo |
| Inventory | ✅ Completo |
| Role | ✅ Completo |
| Task | ✅ Completo |
| Notification | ✅ Completo |
| User | ✅ Completo |
| Component | ✅ Completo |

---

## Product

`Product` fue utilizado como implementación principal de referencia para posteriormente aplicar el mismo patrón al resto de entidades.

Además del CRUD, en Details se muestran los datos relacionados con:

- Category
- Supplier
- Inventory

---

## Partial Views

Las operaciones Create, Edit, Details y Delete se implementaron mediante Partial Views.

Ejemplo:

```text
Views/Product/
├── Index.cshtml
├── _Create.cshtml
├── _Edit.cshtml
├── _Details.cshtml
└── _Delete.cshtml
```

Se reutiliza un modal común:

```text
Views/Shared/_CrudModal.cshtml
```

y la lógica JavaScript compartida ubicada en:

```text
wwwroot/js/site.js
```

Esto permite que las diferentes entidades utilicen el mismo modal para cargar dinámicamente sus Partial Views.

---

## Paginación

Los listados cuentan con paginación de máximo:

```text
25 registros por página
```

La navegación incluye:

```text
First
Previous
1
...
página anterior
página actual
página siguiente
...
última página
Next
Last
```

La página actual se muestra resaltada.

---

## User

El CRUD de User incluye algunas consideraciones adicionales.

### Role

El Role del usuario se selecciona mediante un dropdown utilizando la información obtenida desde `RoleService`.

### Password

La contraseña ingresada durante la creación o modificación de un usuario no se almacena directamente.

El API utiliza `PasswordHasher<User>` para generar el valor almacenado en:

```text
PasswordHash
```

Flujo:

```text
Password
   ↓
PasswordHasher
   ↓
PasswordHash
   ↓
Base de datos
```

Al modificar un usuario:

- Si se ingresa una nueva contraseña, se genera un nuevo hash.
- Si la contraseña se deja vacía, se conserva el hash existente.

El `PasswordHash` no se muestra en las vistas.

---

## Component

La entidad `Component` tiene una diferencia respecto a las demás entidades.

Su identificador es:

```csharp
decimal Id
```

mientras que el método genérico `FindAsync` de `RepositoryBase` utiliza un identificador `int`.

Para evitar modificar la implementación genérica utilizada por las demás entidades, `ComponentRepository` implementa una búsqueda específica:

```csharp
FindByIdAsync(decimal id)
```

De esta forma el CRUD de Component utiliza el tipo definido originalmente en el modelo.

---

## UserAction y UserRole

Estas entidades se mantienen sin CRUD completo.

En el modelo actual de Entity Framework Core ambas están configuradas como entidades sin clave mediante:

```csharp
entity.HasNoKey();
```

Al no poseer una clave primaria identificable, Entity Framework Core no puede aplicar de la misma forma las operaciones normales de actualización y eliminación utilizadas por `RepositoryBase`.

Por esta razón no se modificó la estructura original de la base de datos únicamente para implementar estas operaciones.

---

## Dependency Injection

Los Repositories y Services son registrados mediante Dependency Injection.

Ejemplo:

```csharp
builder.Services.AddScoped<
    I
