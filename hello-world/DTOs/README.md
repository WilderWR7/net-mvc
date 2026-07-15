# DTOs (Data Transfer Objects - Objetos de Transferencia de Datos)

## ¿Para qué sirven?
Los DTOs son clases simples (generalmente solo contienen propiedades, sin lógica de negocio) que se utilizan para transportar datos entre diferentes capas de tu aplicación, o entre tu aplicación web y aplicaciones externas (como una API móvil).

## ¿Por qué usarlos en lugar de los Models directamente?
1. **Seguridad (Over-posting)**: Evitan que un usuario malintencionado pueda actualizar campos protegidos de la base de datos al enviar datos extra en un formulario.
2. **Encapsulamiento**: Ocultan la estructura interna de tu base de datos a los clientes externos.
3. **Optimización**: Permiten enviar solo la información estrictamente necesaria, reduciendo el peso de la petición.

## Reglas de oro
- **Sin comportamiento**: No deben tener métodos que hagan cálculos o validaciones complejas. Son simples portadores de datos.
- **Mapeo**: Frecuentemente se usan librerías como *AutoMapper* para copiar automáticamente los datos entre un `Model` y un `DTO`.

## Ejemplo típico
Tienes un modelo `Usuario` con `Id`, `Nombre`, `Email` y `PasswordHash`. Para la API de "Ver Perfil", creas un `UsuarioPerfilDTO` que solo tiene `Nombre` y `Email`. De esta forma, garantizas que el `PasswordHash` nunca se exponga accidentalmente.
