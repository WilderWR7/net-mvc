# Areas (Áreas)

## ¿Para qué sirven?
Las Áreas son una característica de ASP.NET Core MVC utilizada para organizar aplicaciones web muy grandes o complejas en grupos lógicos más pequeños y manejables. Básicamente, permiten crear "mini-aplicaciones" MVC dentro de tu aplicación principal.

## Estructura
Dentro de esta carpeta, cada Área tendrá su propia estructura típica de MVC:
```
/Areas
  /Admin
    /Controllers
    /Models
    /Views
  /Facturacion
    /Controllers
    /Models
    /Views
```

## ¿Cuándo usarlas?
- **Proyectos monolíticos grandes**: Cuando tienes docenas de controladores y vistas, y la carpeta raíz se vuelve inmanejable.
- **Separación de roles**: Es muy común tener un área `Admin` para el panel de administración, separada totalmente del flujo de usuarios normales.

## Reglas de oro
- **Enrutamiento**: Las rutas a los controladores de un área deben configurarse explícitamente en el `Program.cs` e incluir el atributo `[Area("NombreDelArea")]` en los controladores para evitar conflictos de nombres.
- **No abusar**: Para proyectos pequeños o medianos, usar áreas suele añadir una complejidad innecesaria. Es mejor empezar sin ellas y refactorizar más adelante si el proyecto crece demasiado.
