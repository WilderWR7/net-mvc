# Controllers (Controladores)

## ¿Qué hacen?
Los controladores son el cerebro de la aplicación web en el patrón MVC. Se encargan de:
1. **Recibir peticiones HTTP** (GET, POST, PUT, DELETE) desde el navegador del usuario.
2. **Procesar la entrada**, como los datos de un formulario web o los parámetros de la URL.
3. **Orquestar la lógica de negocio**, llamando a servicios, repositorios o manipulando los Models.
4. **Devolver una respuesta**, que generalmente es la selección de una Vista (View) a renderizar junto con los datos necesarios, o un resultado JSON en caso de una API.

## Reglas de oro
- **Mantén los controladores "delgados"**: Evita poner lógica de negocio compleja aquí. Delega eso a las clases de Servicios o al dominio (Models).
- **Enfoque en el flujo**: Su responsabilidad principal es dirigir el tráfico, no hacer el trabajo pesado.

## Ejemplo típico
Un controlador `HomeController` podría tener un método `Index()` que consulta los últimos 5 artículos de un blog a través de un servicio, y luego pasa esa lista de artículos a la vista `Index.cshtml`.
