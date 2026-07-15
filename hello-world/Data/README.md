# Data (Datos)

## ¿Qué contiene?
Esta carpeta es el hogar de todo lo relacionado con la persistencia de la información de tu aplicación (generalmente, la conexión con la base de datos). 

Aquí encontrarás:
1. **ApplicationDbContext**: La clase principal de Entity Framework Core que coordina la funcionalidad de la base de datos para tus modelos. Representa una sesión con la base de datos.
2. **Repositorios (opcional)**: Clases que encapsulan la lógica de acceso a datos (`Select`, `Insert`, `Update`, `Delete`), ocultando los detalles de Entity Framework o ADO.NET del resto de la aplicación.
3. **Migrations (Migraciones)**: Una subcarpeta generada automáticamente que contiene archivos C# para crear o actualizar el esquema de la base de datos para que coincida con tus clases de Modelos.

## Reglas de oro
- **Aislamiento**: Ninguna otra parte de la aplicación (como los Controladores) debería comunicarse directamente con la base de datos sin pasar por los componentes de esta carpeta.
- **Configuraciones**: Las cadenas de conexión y otras configuraciones se inyectan aquí, pero no se guardan directamente en el código fuente (sino en `appsettings.json` o variables de entorno).
