# wwwroot (Raíz Web)

## ¿Qué es?
Es la carpeta pública de tu aplicación. Es el único lugar dentro del proyecto donde el servidor web (Kestrel/IIS) permitirá a los navegadores descargar archivos directamente sin pasar por la lógica de tus Controladores.

## ¿Qué debe ir aquí?
Solo archivos estáticos (archivos que no necesitan ser procesados por C# antes de enviarse):
- **CSS**: Hojas de estilo para el diseño visual.
- **JS**: Archivos JavaScript para la interactividad en el cliente.
- **Imágenes**: Logotipos, iconos, fotos (`.png`, `.jpg`, `.svg`).
- **Librerías externas**: Bootstrap, jQuery, FontAwesome (aunque hoy en día a menudo se manejan con bundlers o gestores de paquetes frontend).
- **Favicon**: El icono que aparece en la pestaña del navegador.

## Reglas de oro
- **Archivos seguros**: Nunca coloques archivos con código fuente C#, configuraciones (`.json`), o secretos en esta carpeta. Cualquier cosa aquí es de acceso público por defecto.
- **Rutas**: Cuando referencies un archivo desde una Vista (`.cshtml`), la ruta base `/` apunta directamente al contenido de `wwwroot`. Por ejemplo, `<img src="/images/logo.png" />` buscará en `wwwroot/images/logo.png`.
