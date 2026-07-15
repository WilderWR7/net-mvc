/* ==========================================================================================
   GUÍA PRÁCTICA DE SQL SERVER (T-SQL) - CONTEXTO DE LA APP HELLO-WORLD
   ==========================================================================================
   ESQUEMA DE TABLAS UTILIZADAS:
   1. Persons: (Id [Guid], Code [NVarChar(20)], FirstName [NVarChar(20)], LastName [NVarChar(20)], 
                Email [NVarChar(100)], PhoneNumber [NVarChar(7)], CreateAt [DateTime2], UpdatedAt [DateTime2])
   2. Todos:   (Id [Guid], Title [NVarChar(100)], Description [NVarChar(500)], IsCompleted [Bit], 
                UserId [NVarChar(450) FK -> AspNetUsers], CreateAt [DateTime2], UpdatedAt [DateTime2])
   3. AspNetUsers: (Id [NVarChar(450)], UserName, Email, etc. - Tabla de Identity de ASP.NET Core)
   ========================================================================================== */


/* ==========================================================================================
   NIVEL 1: FÁCIL (CONSULTAS BÁSICAS, INSERT, UPDATE, DELETE)
   ========================================================================================== */

-- ------------------------------------------------------------------------------------------
-- 1.1 INSERT SIMPLE (Insertar una persona en la tabla Persons)
-- ------------------------------------------------------------------------------------------
-- Utilizamos NEWID() para generar un GUID único para la columna Id.
-- CreateAt y UpdatedAt toman GETUTCDATE() por defecto, pero aquí los especificamos explícitamente.
INSERT INTO Persons (Id, Code, FirstName, LastName, Email, PhoneNumber, CreateAt, UpdatedAt)
VALUES (
    NEWID(), 
    'PER-001', 
    'Carlos', 
    'Mendoza', 
    'carlos.mendoza@email.com', 
    '5551234', 
    GETUTCDATE(), 
    GETUTCDATE()
);

-- ------------------------------------------------------------------------------------------
-- 1.2 INSERT MÚLTIPLE (Insertar varias tareas en la tabla Todos de un solo golpe)
-- ------------------------------------------------------------------------------------------
-- Nota: Asegúrate de que el UserId ('USR-DEMO-001') exista previamente en AspNetUsers, 
-- o insertémoslo temporalmente para que no falle la clave foránea (ForeignKey):
IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = 'USR-DEMO-001')
BEGIN
    INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
    VALUES ('USR-DEMO-001', 'usuario_demo@app.com', 'USUARIO_DEMO@APP.COM', 'usuario_demo@app.com', 'USUARIO_DEMO@APP.COM', 1, 0, 0, 0, 0);
END;

-- Ahora insertamos 3 tareas para el usuario demo:
INSERT INTO Todos (Id, Title, Description, IsCompleted, UserId, CreateAt, UpdatedAt)
VALUES 
    (NEWID(), 'Aprender SQL Server', 'Repasar comandos básicos de T-SQL', 0, 'USR-DEMO-001', GETUTCDATE(), GETUTCDATE()),
    (NEWID(), 'Configurar Entity Framework', 'Crear migraciones y actualizar base de datos', 1, 'USR-DEMO-001', GETUTCDATE(), GETUTCDATE()),
    (NEWID(), 'Diseñar UI en ASP.NET Core', 'Implementar vistas con Bootstrap y CSS limpio', 0, 'USR-DEMO-001', GETUTCDATE(), GETUTCDATE());

-- ------------------------------------------------------------------------------------------
-- 1.3 SELECT SIMPLE (Consultas con filtros WHERE, ordenamiento ORDER BY y top)
-- ------------------------------------------------------------------------------------------
-- Obtener las 10 últimas tareas pendientes, ordenadas de más reciente a más antigua
SELECT TOP 10 
    Id, 
    Title, 
    Description, 
    CreateAt
FROM Todos
WHERE IsCompleted = 0
ORDER BY CreateAt DESC;

-- Obtener personas cuyo correo pertenezca a un dominio específico usando LIKE
SELECT Code, FirstName, LastName, Email, PhoneNumber
FROM Persons
WHERE Email LIKE '%@email.com';

-- ------------------------------------------------------------------------------------------
-- 1.4 UPDATE SIMPLE (Actualizar un registro por su Code)
-- ------------------------------------------------------------------------------------------
-- Actualizamos el teléfono y la fecha de última modificación de la persona 'PER-001'
UPDATE Persons
SET 
    PhoneNumber = '7779999',
    UpdatedAt = GETUTCDATE()
WHERE Code = 'PER-001';

-- ------------------------------------------------------------------------------------------
-- 1.5 DELETE SIMPLE (Eliminar registros cumpliendo una condición)
-- ------------------------------------------------------------------------------------------
-- Eliminar tareas completadas que tengan más de 30 días de antigüedad
DELETE FROM Todos
WHERE IsCompleted = 1 
  AND CreateAt < DATEADD(DAY, -30, GETUTCDATE());


/* ==========================================================================================
   NIVEL 2: MEDIO (JOINs, AGREGACIONES, SUBCONSULTAS, CTEs)
   ========================================================================================== */

-- ------------------------------------------------------------------------------------------
-- 2.1 INNER JOIN & LEFT JOIN (Relacionar tareas con usuarios de Identity)
-- ------------------------------------------------------------------------------------------
-- Consultamos el título de la tarea, su estado, y el correo del usuario asignado
SELECT 
    t.Title AS Tarea,
    t.Description AS Descripcion,
    CASE WHEN t.IsCompleted = 1 THEN 'Completada' ELSE 'Pendiente' END AS Estado,
    u.UserName AS UsuarioAsignado,
    u.Email AS CorreoUsuario
FROM Todos t
INNER JOIN AspNetUsers u ON t.UserId = u.Id
ORDER BY u.UserName, t.CreateAt DESC;

-- ------------------------------------------------------------------------------------------
-- 2.2 AGREGACIONES Y GROUP BY (Estadísticas por usuario)
-- ------------------------------------------------------------------------------------------
-- Contar cuántas tareas tiene cada usuario, separando completadas y pendientes
SELECT 
    u.UserName,
    COUNT(t.Id) AS TotalTareas,
    SUM(CASE WHEN t.IsCompleted = 1 THEN 1 ELSE 0 END) AS TareasCompletadas,
    SUM(CASE WHEN t.IsCompleted = 0 THEN 1 ELSE 0 END) AS TareasPendientes,
    -- Calcular el porcentaje de avance (manejando división por cero con NULLIF)
    ROUND(
        CAST(SUM(CASE WHEN t.IsCompleted = 1 THEN 1 ELSE 0 END) AS FLOAT) / 
        NULLIF(COUNT(t.Id), 0) * 100, 2
    ) AS PorcentajeAvance
FROM AspNetUsers u
LEFT JOIN Todos t ON u.Id = t.UserId
GROUP BY u.UserName
HAVING COUNT(t.Id) > 0; -- Solo mostrar usuarios que tengan al menos 1 tarea

-- ------------------------------------------------------------------------------------------
-- 2.3 SUBCONSULTA CORRELACIONADA EN UPDATE
-- ------------------------------------------------------------------------------------------
-- Imagina que queremos marcar como completadas las tareas del usuario con email 'usuario_demo@app.com'
UPDATE Todos
SET 
    IsCompleted = 1,
    UpdatedAt = GETUTCDATE()
WHERE UserId IN (
    SELECT Id 
    FROM AspNetUsers 
    WHERE Email = 'usuario_demo@app.com'
);

-- ------------------------------------------------------------------------------------------
-- 2.4 COMMON TABLE EXPRESSION (CTE) SIMPLE
-- ------------------------------------------------------------------------------------------
-- Identificar personas que comparten el mismo primer nombre y numerarlas
WITH PersonasPorNombre AS (
    SELECT 
        FirstName,
        LastName,
        Email,
        ROW_NUMBER() OVER(PARTITION BY FirstName ORDER BY CreateAt ASC) AS NumeroInstancia
    FROM Persons
)
SELECT * FROM PersonasPorNombre
WHERE NumeroInstancia > 1; -- Muestra solo los homónimos


/* ==========================================================================================
   NIVEL 3: COMPLICADO (TRANSACCIONES, OUTPUT, CTEs RECURSIVAS O AVANZADAS, MERGE)
   ========================================================================================== */

-- ------------------------------------------------------------------------------------------
-- 3.1 TRANSACCIÓN CON CONTROL DE ERRORES (TRY / CATCH) Y CLÁUSULA OUTPUT
-- ------------------------------------------------------------------------------------------
-- Aquí insertamos una persona y, si tiene éxito, le asignamos automáticamente una tarea inicial.
-- Todo dentro de una transacción transaccional atómica (si algo falla, se revierte todo).
BEGIN TRY
    BEGIN TRANSACTION;

    -- Tabla temporal de memoria para capturar el GUID generado en el INSERT
    DECLARE @NuevoPersonId TABLE (Id UNIQUEIDENTIFIER, Code NVARCHAR(20));
    DECLARE @PersonGuid UNIQUEIDENTIFIER = NEWID();

    -- 1. Insertar persona capturando su ID en la variable tabla usando OUTPUT
    INSERT INTO Persons (Id, Code, FirstName, LastName, Email, PhoneNumber, CreateAt, UpdatedAt)
    OUTPUT INSERTED.Id, INSERTED.Code INTO @NuevoPersonId
    VALUES (@PersonGuid, 'PER-999', 'Ana', 'García', 'ana.garcia@empresa.com', '5558888', GETUTCDATE(), GETUTCDATE());

    -- 2. Simular que la persona está asociada a un usuario en AspNetUsers y crearle su tarea inicial
    DECLARE @TargetUserId NVARCHAR(450) = 'USR-DEMO-001';

    INSERT INTO Todos (Id, Title, Description, IsCompleted, UserId, CreateAt, UpdatedAt)
    VALUES (
        NEWID(),
        'Bienvenida: Revisar perfil de ' + (SELECT Code FROM @NuevoPersonId),
        'Tarea automática generada tras el alta del empleado en el módulo de Persons.',
        0,
        @TargetUserId,
        GETUTCDATE(),
        GETUTCDATE()
    );

    COMMIT TRANSACTION;
    PRINT 'Transacción completada exitosamente. Persona y Tarea generadas.';
END TRY
BEGIN CATCH
    -- Si ocurre cualquier error de validación o clave, revertimos los cambios
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    -- Mostrar detalles técnicos del error
    DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrSeverity INT = ERROR_SEVERITY();
    RAISERROR('Error en la transacción: %s', @ErrSeverity, 1, @ErrMsg);
END CATCH;

-- ------------------------------------------------------------------------------------------
-- 3.2 WINDOW FUNCTIONS (Funciones de Ventana para Análisis)
-- ------------------------------------------------------------------------------------------
-- Analizar el historial de creación de tareas: diferencia en horas entre una tarea y la anterior
SELECT 
    t.Title,
    u.UserName,
    t.CreateAt,
    LAG(t.CreateAt, 1) OVER (PARTITION BY t.UserId ORDER BY t.CreateAt ASC) AS FechaTareaAnterior,
    DATEDIFF(
        MINUTE, 
        LAG(t.CreateAt, 1) OVER (PARTITION BY t.UserId ORDER BY t.CreateAt ASC), 
        t.CreateAt
    ) AS MinutosDesdeAnteriorTarea
FROM Todos t
INNER JOIN AspNetUsers u ON t.UserId = u.Id;


/* ==========================================================================================
   ALGO BIEN EXPLICADO SOBRE MERGE (UPSERT: UPDATE + INSERT + DELETE EN UN SOLO PASO)
   ==========================================================================================
   ¿QUÉ ES MERGE?
   La instrucción MERGE en SQL Server compara dos conjuntos de datos: 
   - Una tabla DESTINO (Target): Nuestra tabla de base de datos (ej. Persons).
   - Una tabla/consulta ORIGEN (Source): Datos que recibimos desde una API, archivo Excel o tabla temporal.

   SEGÚN EL RESULTADO DE LA COMPARACIÓN (ON Target.Key = Source.Key), HACE 3 COSAS:
   1. WHEN MATCHED: El registro existe en ambos -> Lo ACTUALIZA (UPDATE).
   2. WHEN NOT MATCHED BY TARGET: El registro está en el Origen pero no en la BD -> Lo INSERTA (INSERT).
   3. WHEN NOT MATCHED BY SOURCE: El registro está en la BD pero ya no en el Origen -> Lo ELIMINA o INACTIVA (DELETE/UPDATE).
   
   ¡IMPORTANTE!: Toda sentencia MERGE DEBE terminar obligatoriamente con un punto y coma (;).
   ========================================================================================== */

-- Paso A: Creamos una tabla temporal o variable tipo tabla que simula un "lote" de datos que llega de la app o un Excel
DECLARE @PersonsSyncBatch TABLE (
    Code NVARCHAR(20) NOT NULL PRIMARY KEY,
    FirstName NVARCHAR(20) NOT NULL,
    LastName NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(7) NOT NULL
);

-- Insertamos datos en la tabla ORIGEN (Source):
-- 1. 'PER-001' -> Ya existe en Persons, pero le pondremos un nombre actualizado ('Carlos Alberto'). (Debe disparar UPDATE)
-- 2. 'PER-777' -> No existe en Persons. (Debe disparar INSERT)
-- Nota: 'PER-999' (que insertamos en el paso anterior) NO está en este lote, por lo que podríamos eliminarlo si quisiéramos sincronicidad total.
INSERT INTO @PersonsSyncBatch (Code, FirstName, LastName, Email, PhoneNumber)
VALUES 
    ('PER-001', 'Carlos Alb.', 'Mendoza', 'carlos.mendoza@email.com', '5551234'),
    ('PER-777', 'Lucía', 'Fernández', 'lucia.fernandez@email.com', '6663333');

-- Paso B: Ejecutamos el MERGE contra nuestra tabla real Persons (Target)
MERGE INTO Persons AS Target
USING @PersonsSyncBatch AS Source
ON (Target.Code = Source.Code) -- Criterio de comparación: El código único

-- CASO 1: El registro existe tanto en Persons como en el lote recibido -> ACTUALIZAMOS
WHEN MATCHED THEN
    UPDATE SET 
        Target.FirstName = Source.FirstName,
        Target.LastName = Source.LastName,
        Target.Email = Source.Email,
        Target.PhoneNumber = Source.PhoneNumber,
        Target.UpdatedAt = GETUTCDATE()

-- CASO 2: El registro está en el lote (Source) pero no existe en Persons (Target) -> INSERTAMOS
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, Code, FirstName, LastName, Email, PhoneNumber, CreateAt, UpdatedAt)
    VALUES (
        NEWID(), 
        Source.Code, 
        Source.FirstName, 
        Source.LastName, 
        Source.Email, 
        Source.PhoneNumber, 
        GETUTCDATE(), 
        GETUTCDATE()
    )

-- CASO 3 (Opcional): Si un registro existe en Persons pero ya no vino en el lote -> ELIMINAR (o desactivar)
-- Descomenta las 2 líneas siguientes si deseas un "borrado espejo" estricto:
-- WHEN NOT MATCHED BY SOURCE AND Target.Code LIKE 'PER-%' THEN
--     DELETE

-- Cláusula OUTPUT para ver exactamente qué acción tomó SQL Server para cada fila
OUTPUT 
    $action AS AccionRealizada, 
    COALESCE(INSERTED.Code, DELETED.Code) AS CodigoPersona,
    DELETED.FirstName AS NombreAnterior,
    INSERTED.FirstName AS NombreNuevo; -- OBLIGATORIO TERMINAR MERGE CON PUNTO AND COMA (;)


/* ==========================================================================================
   PROCEDURES (PROCEDIMIENTOS ALMACENADOS)
   ========================================================================================== */

-- ------------------------------------------------------------------------------------------
-- 4.1 PROCEDIMIENTO 1: Crear o actualizar una tarea con validación y retorno del GUID
-- ------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_SaveTodoTask', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_SaveTodoTask;
GO

CREATE PROCEDURE dbo.sp_SaveTodoTask
    @TodoId UNIQUEIDENTIFIER = NULL OUTPUT, -- Si es NULL crea, si viene con valor actualiza
    @Title NVARCHAR(100),
    @Description NVARCHAR(500),
    @UserId NVARCHAR(450)
AS
BEGIN
    SET NOCOUNT ON; -- Mejora el rendimiento evitando mensajes de filas afectadas

    -- Validar que el título no esté vacío
    IF LTRIM(RTRIM(@Title)) = ''
    BEGIN
        RAISERROR('El título de la tarea no puede estar vacío.', 16, 1);
        RETURN -1;
    END

    -- Validar que el usuario exista
    IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE Id = @UserId)
    BEGIN
        RAISERROR('El usuario especificado no existe en el sistema.', 16, 1);
        RETURN -2;
    END

    -- Lógica de Upsert (Crear o Actualizar)
    IF @TodoId IS NULL OR NOT EXISTS (SELECT 1 FROM Todos WHERE Id = @TodoId)
    BEGIN
        -- CREACIÓN
        SET @TodoId = ISNULL(@TodoId, NEWID());
        
        INSERT INTO Todos (Id, Title, Description, IsCompleted, UserId, CreateAt, UpdatedAt)
        VALUES (@TodoId, LTRIM(RTRIM(@Title)), ISNULL(LTRIM(RTRIM(@Description)), ''), 0, @UserId, GETUTCDATE(), GETUTCDATE());
        
        PRINT 'Tarea creada exitosamente.';
    END
    ELSE
    BEGIN
        -- ACTUALIZACIÓN
        UPDATE Todos
        SET Title = LTRIM(RTRIM(@Title)),
            Description = ISNULL(LTRIM(RTRIM(@Description)), ''),
            UpdatedAt = GETUTCDATE()
        WHERE Id = @TodoId;
        
        PRINT 'Tarea actualizada exitosamente.';
    END

    RETURN 0; -- Código de éxito
END;
GO

-- EJEMPLO DE USO DEL PROCEDIMIENTO ALMACENADO:
/*
DECLARE @IdGenerado UNIQUEIDENTIFIER;
EXEC dbo.sp_SaveTodoTask 
    @TodoId = @IdGenerado OUTPUT,
    @Title = 'Probar Stored Procedure',
    @Description = 'Verificar que sp_SaveTodoTask inserte correctamente en la tabla Todos',
    @UserId = 'USR-DEMO-001';

SELECT @IdGenerado AS IdGenerado;
*/


-- ------------------------------------------------------------------------------------------
-- 4.2 PROCEDIMIENTO 2: Reporte resumido paginado y dinámico
-- ------------------------------------------------------------------------------------------
IF OBJECT_ID('dbo.sp_GetPersonsPaged', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_GetPersonsPaged;
GO

CREATE PROCEDURE dbo.sp_GetPersonsPaged
    @SearchTerm NVARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @TotalRecords INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Obtener el total de registros que coinciden con el filtro
    SELECT @TotalRecords = COUNT(*)
    FROM Persons
    WHERE (@SearchTerm IS NULL OR 
           FirstName LIKE '%' + @SearchTerm + '%' OR 
           LastName LIKE '%' + @SearchTerm + '%' OR 
           Code LIKE '%' + @SearchTerm + '%');

    -- 2. Obtener la página solicitada usando OFFSET / FETCH (Estándar moderno en SQL Server)
    SELECT 
        Id,
        Code,
        FirstName + ' ' + LastName AS FullName,
        Email,
        PhoneNumber,
        CreateAt
    FROM Persons
    WHERE (@SearchTerm IS NULL OR 
           FirstName LIKE '%' + @SearchTerm + '%' OR 
           LastName LIKE '%' + @SearchTerm + '%' OR 
           Code LIKE '%' + @SearchTerm + '%')
    ORDER BY CreateAt DESC, Code ASC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO


/* ==========================================================================================
   TRIGGERS (DISPARADORES AUTOMÁTICOS)
   ========================================================================================== */

-- ------------------------------------------------------------------------------------------
-- 5.1 TRIGGER DE ACTUALIZACIÓN AUTOMÁTICA DE FECHA (AFTER UPDATE)
-- ------------------------------------------------------------------------------------------
-- Aunque en ApplicationDbContext de C# tienes el método `UpdateTimestamps()` antes del SaveChanges,
-- este Trigger asegura que si alguien modifica la tabla Persons directamente por SQL (en SSMS),
-- la columna UpdatedAt se actualice automáticamente con la hora actual UTC.
IF OBJECT_ID('dbo.trg_Persons_UpdateTimestamp', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_Persons_UpdateTimestamp;
GO

CREATE TRIGGER dbo.trg_Persons_UpdateTimestamp
ON dbo.Persons
AFTER UPDATE
AS
BEGIN
    -- Evitar recursividad y no disparar si no hay filas afectadas o si se está alterando UpdatedAt explícitamente y es lo único
    IF IF(ROWCOUNT) = 0 RETURN;
    SET NOCOUNT ON;

    -- Si la actualización ya modificó UpdatedAt manualmente en la consulta, podemos permitirlo, 
    -- pero para garantizar consistencia absoluta, lo forzamos para los IDs modificados en DELETED/INSERTED:
    UPDATE p
    SET UpdatedAt = GETUTCDATE()
    FROM dbo.Persons p
    INNER JOIN INSERTED i ON p.Id = i.Id
    -- Solo actualizar si no fue actualizado explícitamente en el mismo UPDATE para evitar loops
    WHERE NOT UPDATE(UpdatedAt);
END;
GO

-- ------------------------------------------------------------------------------------------
-- 5.2 TRIGGER DE AUDITORÍA Y PREVENCIÓN DE BORRADO (INSTEAD OF DELETE)
-- ------------------------------------------------------------------------------------------
-- Imagina que queremos evitar que se eliminen permanentemente las tareas (o auditar antes de borrar).
-- Un trigger INSTEAD OF intercepta la orden DELETE de SQL y nos permite reescribir la lógica.
IF OBJECT_ID('dbo.trg_Todos_SoftDeleteOrAudit', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_Todos_SoftDeleteOrAudit;
GO

CREATE TRIGGER dbo.trg_Todos_SoftDeleteOrAudit
ON dbo.Todos
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Ejemplo: En lugar de borrar físicamente si la tarea no está completada, bloqueamos la acción.
    -- O si está completada, permitimos el borrado físico y registramos en la consola.
    IF EXISTS (SELECT 1 FROM DELETED WHERE IsCompleted = 0)
    BEGIN
        RAISERROR('Alerta de seguridad: No está permitido eliminar tareas pendientes. Debe completarlas primero.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Si todas las tareas a borrar ya estaban completadas, procedemos con la eliminación real:
    DELETE FROM dbo.Todos
    WHERE Id IN (SELECT Id FROM DELETED);

    PRINT 'Se han eliminado correctamente las tareas completadas.';
END;
GO
