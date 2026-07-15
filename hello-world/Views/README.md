# Views (Vistas)

## ¿Qué son?
Las vistas son los componentes de la interfaz de usuario. En ASP.NET Core MVC, estas son generalmente archivos `.cshtml` que utilizan el motor de plantillas **Razor**. Razor permite mezclar código HTML estándar con código C# para generar páginas web dinámicas.

## ¿Cómo funcionan?
1. El Controlador recopila los datos necesarios (Model) y se los pasa a la Vista.
2. La Vista usa el código Razor para iterar sobre los datos y generar el HTML final.
3. El HTML resultante se envía al navegador del usuario.

## Reglas de oro
- **Cero lógica de negocio**: Las Vistas deben ser lo más "tontas" posibles. Solo deben contener lógica relacionada con la presentación (ej. `if` para mostrar u ocultar un botón, `foreach` para listar elementos de una tabla).
- **Tipado Fuerte**: Usa la directiva `@model` en la parte superior del archivo para definir qué tipo de datos espera la vista. Esto te da autocompletado y validación de errores en tiempo de compilación.

## Ejemplo típico
Una vista `Detalle.cshtml` recibe un `ProductoViewModel` y muestra `<h1>@Model.Nombre</h1>` y `<p>Precio: @Model.Precio</p>`.
