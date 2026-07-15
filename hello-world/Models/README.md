# Models (Modelos)

## ¿Qué son?
Los modelos representan la estructura de los datos, el estado de la aplicación y las reglas de negocio fundamentales. 

Se dividen típicamente en:
1. **Modelos de Dominio/Entidades**: Representan conceptos reales de tu negocio (ej. `Usuario`, `Producto`, `Factura`). A menudo, estas son las clases que Entity Framework Core mapea directamente a las tablas de tu base de datos.
2. **ViewModels (Modelos de Vista)**: Clases creadas específicamente para transportar la información exacta que necesita una Vista en particular. Ayudan a mantener las vistas limpias y evitan enviar datos sensibles que están en los modelos de dominio pero que no deben mostrarse.

## Reglas de oro
- **Ricos en comportamiento**: Los modelos de dominio deben contener lógica relacionada con sus propios datos (validaciones intrínsecas, cálculos).
- **No dependen de la UI ni de la base de datos**: Los modelos puros no deben saber si se muestran en una página web o si se guardan en SQL Server. (Aunque en proyectos sencillos a veces se mezclan con anotaciones de Entity Framework).

## Ejemplo típico
Un modelo `Producto` podría tener propiedades como `Id`, `Nombre`, `Precio`, e `Inventario`, junto con un método `AplicarDescuento(porcentaje)`.
