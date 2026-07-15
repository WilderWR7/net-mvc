# Bienvenido a tu primer proyecto .NET (ASP.NET Core MVC) 🚀

Si es tu primera vez trabajando con .NET y el patrón MVC, ¡no te preocupes! Esta guía te servirá de mapa.

## ¿Qué es ASP.NET Core MVC?
Es un framework (un conjunto de herramientas preparadas) creado por Microsoft para construir aplicaciones web. Usa el lenguaje de programación **C#** y sigue una forma de organizar el código llamada **MVC (Model-View-Controller)**.

Para entender MVC, imagina un restaurante:
1. **Model (Modelo) - *La Cocina y los Ingredientes***: Son los datos de tu aplicación (ej. la información de un 'Usuario' o un 'Producto') y la forma de prepararlos.
2. **View (Vista) - *El Plato Presentado***: Es lo que el cliente final ve y con lo que interactúa. En la web, esto es el HTML, CSS y diseño de la página.
3. **Controller (Controlador) - *El Mesero***: Cuando el usuario hace una petición (ej. entra a una URL o envía un formulario), el Controlador la recibe. Va al Modelo a buscar la información necesaria y se la entrega a la Vista para que arme la página final que se le devuelve al usuario.

## Requisitos Previos
Antes de empezar, asegúrate de tener instalado en tu computadora:
- [Git](https://git-scm.com/downloads) (Para descargar el código fuente).
- [.NET SDK](https://dotnet.microsoft.com/download) (El motor que compila y corre el código. Asegúrate de instalar la versión correspondiente a este proyecto).
- Un editor de código, preferiblemente [Visual Studio](https://visualstudio.microsoft.com/) (ideal para principiantes) o [Visual Studio Code](https://code.visualstudio.com/).

## ¿Cómo descargar (clonar) y configurar el proyecto?

Si estás descargando el proyecto por primera vez para trabajar en él, sigue estos pasos:

### 1. Clonar el repositorio
Abre tu terminal (Símbolo del sistema, PowerShell o Git Bash) y ejecuta:
```bash
git clone <URL_DEL_REPOSITORIO>
```
*(Reemplaza `<URL_DEL_REPOSITORIO>` con la dirección real del proyecto en GitHub, GitLab o Azure DevOps).*

Luego, entra en la carpeta del proyecto:
```bash
cd nombre-del-repositorio
```

### 2. Restaurar dependencias
Los proyectos .NET usan paquetes externos (librerías como Newtonsoft.Json o Entity Framework). Estos paquetes no se suben a Git para ahorrar espacio. Para descargarlos todos automáticamente desde internet, ejecuta:
```bash
dotnet restore
```

### 3. Compilar el proyecto (Reconstruir)
Para verificar que todo el código fuente es válido y se traduce correctamente a lenguaje máquina sin errores, ejecuta:
```bash
dotnet build
```

### 4. Configurar la base de datos (Si aplica)
Si el proyecto se conecta a una base de datos local y usa "Entity Framework Core", debes crear las tablas iniciales ejecutando las migraciones:
```bash
dotnet ef database update
```
*(Nota: Revisa el archivo `appsettings.json` para asegurarte de que la cadena de conexión esté configurada para tu servidor local).*

## ¿Cómo ejecuto este proyecto?

**Opción A: Usando Visual Studio (Recomendado)**
1. Abre el archivo `.sln` (Solución) o `.csproj` (Proyecto) haciendo doble clic sobre él.
2. En la barra superior, busca un botón verde de "Play" (iniciar) que suele decir algo como "IIS Express" o "hello-world".
3. Haz clic en él o presiona la tecla **`F5`** en tu teclado.
4. Visual Studio levantará un servidor local y abrirá automáticamente tu navegador web mostrando tu aplicación.

**Opción B: Usando la Terminal (Línea de comandos)**
Si prefieres usar la terminal, ubícate en la carpeta donde está el archivo `.csproj` y ejecuta:
```bash
dotnet run
```
La terminal compilará el código y te mostrará una dirección web (generalmente `http://localhost:5000` o similar). Copia esa URL y pégala en tu navegador.

## Estructura de este proyecto
He colocado archivos `README.md` dentro de las carpetas más importantes del proyecto. Cuando no sepas para qué sirve una carpeta, ¡entra y lee su README!

Aquí tienes lo más importante que está **fuera** de las carpetas:

- **`Program.cs`**: Es el **corazón y punto de inicio** de tu aplicación. Cuando ejecutas el proyecto, el código de este archivo es el primero en correr. Aquí configuras cosas esenciales, como decirle a la app qué base de datos usar y cómo procesar las peticiones de los usuarios.
- **`appsettings.json`**: Es el archivo de **configuraciones globales**. Aquí guardas textos importantes como la "cadena de conexión" (la dirección y contraseña de tu base de datos) o variables que tu app necesita para funcionar.

## ¿Por dónde empiezo a leer el código?
Te sugiero este recorrido para entender cómo funciona la app por dentro:
1. Lee el README de la carpeta **`Controllers`** (para entender cómo entran las peticiones).
2. Lee el de **`Models`** (para ver cómo definimos qué datos existen).
3. Lee el de **`Views`** (para entender cómo se dibuja la pantalla).
