# Copilot Instructions

## Directrices del proyecto
Product es la implementación de referencia para los CRUD.

Para implementar otros CRUD:
- Mantener la arquitectura existente.

-  No rediseñar RepositoryBase.
- No introducir ViewModels.
- Utilizar los DTO existentes.
- Mantener el patrón del ProductService mostrado por el profesor.
- Los métodos Create, Update y Delete del Service pueden retornar true
  después del RestProvider, siguiendo ProductService.
- Usar RestProvider y JsonProvider como en ProductService.
- Mantener Controller Web -> Service -> RestProvider -> API -> Repository.
- Usar métodos async/await.
- Reutilizar Views/Shared/_CrudModal.cshtml.
- Reutilizar wwwroot/js/site.js y la clase .btn-crud.
- No crear un modal distinto para cada entidad.
- Create, Edit, Details y Delete deben ser Partial Views.
- Las Partial Views deben llamarse:
  _Create.cshtml
  _Edit.cshtml
  _Details.cshtml
  _Delete.cshtml.
- Index debe tener paginación de máximo 25 elementos.
- Usar ProductController, ProductService, Product API Controller y
  Views/Product como referencia.
- No modificar la base de datos ni inventar relaciones.
- No modificar Product salvo que se solicite expresamente.
- No implementar entidades AspNet*.