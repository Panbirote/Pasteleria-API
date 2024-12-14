/* Materia prima */
-- Select
Create or Alter Procedure SelectInventarioMateriaPrimaById
    @IdMateriaPrima INT
AS
BEGIN
    SELECT IdMateriaPrima, Descripcion, Entradas, Salidas, StockActual, UnidadMedida
    FROM InventarioMateriaPrima
    WHERE IdMateriaPrima = @IdMateriaPrima;
END;
GO

--SelectAll
Create or Alter Procedure SelectAllInventarioMateriaPrima
AS
BEGIN
    SELECT IdMateriaPrima, Descripcion, Entradas, Salidas, StockActual, UnidadMedida
    FROM InventarioMateriaPrima;
END;
GO

-- Insert
Create or Alter Procedure InsertInventarioMateriaPrima
    @Descripcion VARCHAR(255),
    @Entradas INT,
    @Salidas INT,
    @StockActual INT,
    @UnidadMedida VARCHAR(50)
AS
BEGIN
    INSERT INTO InventarioMateriaPrima (Descripcion, Entradas, Salidas, StockActual, UnidadMedida)
    VALUES (@Descripcion, @Entradas, @Salidas, @StockActual, @UnidadMedida);
END;
GO

-- Update
Create or Alter Procedure UpdateInventarioMateriaPrima
    @IdMateriaPrima INT,
    @Descripcion VARCHAR(255),
    @Entradas INT,
    @Salidas INT,
    @StockActual INT,
    @UnidadMedida VARCHAR(50)
AS
BEGIN
    UPDATE InventarioMateriaPrima
    SET Descripcion = @Descripcion,
        Entradas = @Entradas,
        Salidas = @Salidas,
        StockActual = @StockActual,
        UnidadMedida = @UnidadMedida
    WHERE IdMateriaPrima = @IdMateriaPrima;
END;
GO

-- Delete 
Create or Alter Procedure DeleteInventarioMateriaPrima
    @IdMateriaPrima INT
AS
BEGIN
    DELETE FROM InventarioMateriaPrima
    WHERE IdMateriaPrima = @IdMateriaPrima;
END;
GO

/* Recetas */
-- Select
Create or Alter Procedure SelectRecetasById
    @IdReceta INT
AS
BEGIN
    SELECT 
        R.IdReceta, 
        R.NombreReceta, 
        R.Descripcion,
        RD.IdMateriaPrima, 
        RD.Cantidad, 
        IMP.Descripcion AS MateriaPrimaDescripcion, 
        IMP.UnidadMedida
    FROM Recetas R
    LEFT JOIN RecetasDetalle RD ON R.IdReceta = RD.IdReceta
    LEFT JOIN InventarioMateriaPrima IMP ON RD.IdMateriaPrima = IMP.IdMateriaPrima
    WHERE R.IdReceta = @IdReceta;
END;
GO

-- SelectAll
Create or Alter Procedure SelectAllRecetas
AS
BEGIN
    SELECT 
        R.IdReceta, 
        R.NombreReceta, 
        R.Descripcion,
        RD.IdMateriaPrima, 
        RD.Cantidad, 
        IMP.Descripcion AS MateriaPrimaDescripcion, 
        IMP.UnidadMedida
    FROM Recetas R
    LEFT JOIN RecetasDetalle RD ON R.IdReceta = RD.IdReceta
    LEFT JOIN InventarioMateriaPrima IMP ON RD.IdMateriaPrima = IMP.IdMateriaPrima;
END;
GO

-- Select Detalle By Id Receta
Create or Alter Procedure SelectRecetasDetalleByIdReceta
    @IdReceta INT
AS
BEGIN
    SELECT IdReceta, IdMateriaPrima, Cantidad
    FROM RecetasDetalle
    WHERE IdReceta = @IdReceta;
END;
GO

-- Tipo para receta (tabla para insertar en parámetro)
Create Type DetalleRecetaType AS TABLE
(
    IdMateriaPrima INT,
    Cantidad INT
);
GO

-- Insert
Create or Alter Procedure InsertRecetaConDetalle
    @NombreReceta VARCHAR(255),
    @Descripcion VARCHAR(500),
    @DetallesReceta DetalleRecetaType READONLY
AS
BEGIN
    BEGIN TRANSACTION;

    -- Insert tabla maestra
    INSERT INTO Recetas (NombreReceta, Descripcion)
    VALUES (@NombreReceta, @Descripcion);

    -- Id de la receta insertada
    DECLARE @NewIdReceta INT = SCOPE_IDENTITY();

    -- Insertar detalle en RecetasDetalle
    INSERT INTO RecetasDetalle (IdReceta, IdMateriaPrima, Cantidad)
    SELECT @NewIdReceta, IdMateriaPrima, Cantidad
    FROM @DetallesReceta;

    COMMIT TRANSACTION;
END;
GO

-- Update
Create or Alter Procedure UpdateRecetaConDetalle
    @IdReceta INT,
    @NombreReceta VARCHAR(255),
    @Descripcion VARCHAR(500),
    @DetallesReceta DetalleRecetaType READONLY
AS
BEGIN
    BEGIN TRANSACTION;

    -- Update tabla maestra
    UPDATE Recetas
    SET NombreReceta = @NombreReceta,
        Descripcion = @Descripcion
    WHERE IdReceta = @IdReceta;

    -- Borrar detalle
    DELETE FROM RecetasDetalle
    WHERE IdReceta = @IdReceta;

    -- Insertar detalle
    INSERT INTO RecetasDetalle (IdReceta, IdMateriaPrima, Cantidad)
    SELECT @IdReceta, IdMateriaPrima, Cantidad
    FROM @DetallesReceta;

    COMMIT TRANSACTION;
END;
GO

-- Delete
Create or Alter Procedure DeleteReceta
    @IdReceta INT
AS
BEGIN
    BEGIN TRANSACTION;

    -- Borrar detalle
    DELETE FROM RecetasDetalle
    WHERE IdReceta = @IdReceta;

    -- Borrar de tabla maestra
    DELETE FROM Recetas
    WHERE IdReceta = @IdReceta;

    COMMIT TRANSACTION;
END;
GO

/* Productos */
-- Select
Create or Alter Procedure SelectProductosById
    @IdProducto INT
AS
BEGIN
    SELECT IdProducto, IdReceta, Entradas, Salidas, PrecioUnitario, IVA, NombreProducto, Descripcion, StockActual
    FROM Productos
    WHERE IdProducto = @IdProducto;
END;
GO

-- SelectAll
Create or Alter Procedure SelectAllProductos
AS
BEGIN
    SELECT IdProducto, IdReceta, Entradas, Salidas, PrecioUnitario, IVA, NombreProducto, Descripcion, StockActual
    FROM Productos;
END;
GO

-- Insert
Create or Alter Procedure InsertProductos
    @IdReceta INT,
    @Entradas INT,
    @Salidas INT,
    @PrecioUnitario MONEY,
    @Iva MONEY,
    @NombreProducto VARCHAR(255),
    @Descripcion VARCHAR(500),
    @StockActual INT
AS
BEGIN
    INSERT INTO Productos (IdReceta, Entradas, Salidas, PrecioUnitario, Iva, NombreProducto, Descripcion, StockActual)
    VALUES (@IdReceta, @Entradas, @Salidas, @PrecioUnitario, @Iva, @NombreProducto, @Descripcion, @StockActual);
END;
GO

-- Update
Create or Alter Procedure UpdateProductos
    @IdProducto INT,
    @IdReceta INT,
    @Entradas INT,
    @Salidas INT,
    @PrecioUnitario MONEY,
    @Iva MONEY,
    @NombreProducto VARCHAR(255),
    @Descripcion VARCHAR(500),
    @StockActual INT
AS
BEGIN
    UPDATE Productos
    SET IdReceta = @IdReceta,
        Entradas = @Entradas,
        Salidas = @Salidas,
        PrecioUnitario = @PrecioUnitario,
        Iva = @Iva,
        NombreProducto = @NombreProducto,
        Descripcion = @Descripcion,
        StockActual = @StockActual
    WHERE IdProducto = @IdProducto;
END;
GO

-- Delete
Create or Alter Procedure DeleteProductos
    @IdProducto INT
AS
BEGIN
    DELETE FROM Productos
    WHERE IdProducto = @IdProducto;
END;
GO

/* Clientes */
-- Select
Create or Alter Procedure SelectClientesById
    @IdCliente INT
AS
BEGIN
    SELECT IdCliente, IdUsuarioWeb, Nombre, Telefono, Email, Direccion, Rfc,
           CodigoPostal, Curp, Activo, Municipio, Estado, Pais, FechaRegistro, Notas
    FROM Clientes
    WHERE IdCliente = @IdCliente;
END;
GO

-- SelectAll
Create or Alter Procedure SelectAllClientes
AS
BEGIN
    SELECT IdCliente, IdUsuarioWeb, Nombre, Telefono, Email, Direccion, Rfc,
           CodigoPostal, Curp, Activo, Municipio, Estado, Pais, FechaRegistro, Notas
    FROM Clientes;
END;
GO

-- Insert
Create or Alter Procedure InsertClientes
    @IdUsuarioWeb VARCHAR(255),
    @Nombre VARCHAR(255),
    @Telefono VARCHAR(50),
    @Email VARCHAR(255),
    @Direccion VARCHAR(500),
    @Rfc VARCHAR(50),
    @CodigoPostal INT,
    @Curp VARCHAR(50),
    @Activo BIT,
    @Municipio VARCHAR(255),
    @Estado VARCHAR(255),
    @Pais VARCHAR(255),
    @Notas VARCHAR(500)
AS
BEGIN
    INSERT INTO Clientes (IdUsuarioWeb, Nombre, Telefono, Email, Direccion, Rfc, CodigoPostal, Curp, Activo, Municipio, Estado, Pais, Notas)
    VALUES (@IdUsuarioWeb, @Nombre, @Telefono, @Email, @Direccion, @Rfc, @CodigoPostal, @Curp, @Activo, @Municipio, @Estado, @Pais, @Notas);
END;
GO

-- Update
Create or Alter Procedure UpdateClientes
    @IdCliente INT,
    @IdUsuarioWeb VARCHAR(255),
    @Nombre VARCHAR(255),
    @Telefono VARCHAR(50),
    @Email VARCHAR(255),
    @Direccion VARCHAR(500),
    @Rfc VARCHAR(50),
    @CodigoPostal INT,
    @Curp VARCHAR(50),
    @Activo BIT,
    @Municipio VARCHAR(255),
    @Estado VARCHAR(255),
    @Pais VARCHAR(255),
    @Notas VARCHAR(500)
AS
BEGIN
    UPDATE Clientes
    SET IdUsuarioWeb = @IdUsuarioWeb,
        Nombre = @Nombre,
        Telefono = @Telefono,
        Email = @Email,
        Direccion = @Direccion,
        Rfc = @Rfc,
        CodigoPostal = @CodigoPostal,
        Curp = @Curp,
        Activo = @Activo,
        Municipio = @Municipio,
        Estado = @Estado,
        Pais = @Pais,
        Notas = @Notas
    WHERE IdCliente = @IdCliente;
END;
GO

-- Delete
Create or Alter Procedure DeleteClientes
    @IdCliente INT
AS
BEGIN
    DELETE FROM Clientes
    WHERE IdCliente = @IdCliente;
END;
GO

/* Pedidos */
-- Select
Create or Alter Procedure SelectPedidosById
    @IdPedido INT
AS
BEGIN
    SELECT IdPedido, NumeroOrden, IdCliente, FechaPedido, DireccionEntrega,
           FechaEntrega, Subtotal, IVA, Total, Entregado, Notas
    FROM Pedidos
    WHERE IdPedido = @IdPedido;
END;
GO

-- SelectAll
Create or Alter Procedure SelectAllPedidos
AS
BEGIN
    SELECT IdPedido, NumeroOrden, IdCliente, FechaPedido, DireccionEntrega,
           FechaEntrega, Subtotal, IVA, Total, Entregado, Notas
    FROM Pedidos;
END;
GO

-- SelectDetalleByIdPedido
Create or Alter Procedure SelectPedidosDetalleByIdPedido
    @IdPedido INT
AS
BEGIN
    SELECT IdPedido, IdProducto, Cantidad, Total, PrecioUnitario
    FROM PedidosDetalle
    WHERE IdPedido = @IdPedido;
END;
GO

-- SelectPedidosByIdCliente
Create or Alter Procedure SelectPedidosByIdCliente
    @IdCliente INT
AS
BEGIN
    SELECT IdPedido, NumeroOrden, IdCliente, FechaPedido, DireccionEntrega,
           FechaEntrega, Subtotal, IVA, Total, Entregado, Notas
    FROM Pedidos
    WHERE IdCliente = @IdCliente;
END;
GO

-- Tipo de tabla para insert
Create Type PedidosDetalleType AS TABLE
(
    IdProducto INT,
    Cantidad INT,
    PrecioUnitario MONEY,
    Total MONEY
);
GO

-- Insert
Create or Alter Procedure InsertPedidosConDetalle
    @NumeroOrden INT,
    @IdCliente INT,
    @FechaPedido DATETIME,
    @DireccionEntrega VARCHAR(500),
    @FechaEntrega DATETIME,
    @Subtotal MONEY,
    @IVA MONEY,
    @Total MONEY,
    @Entregado BIT,
	@Notas VARCHAR(500),
    @DetalleProductos PedidosDetalleType READONLY
AS
BEGIN
    BEGIN TRANSACTION;

    -- Insert en tabla Pedidos
    INSERT INTO Pedidos (NumeroOrden, IdCliente, FechaPedido, DireccionEntrega, FechaEntrega, Subtotal, IVA, Total, Entregado, Notas)
    VALUES (@NumeroOrden, @IdCliente, @FechaPedido, @DireccionEntrega, @FechaEntrega, @Subtotal, @IVA, @Total, @Entregado, @Notas);

    DECLARE @IdPedido INT = SCOPE_IDENTITY();

    -- Insert en tabla PedidosDetalle
    INSERT INTO PedidosDetalle (IdPedido, IdProducto, Cantidad, PrecioUnitario, Total)
    SELECT @IdPedido, IdProducto, Cantidad, PrecioUnitario, Total
    FROM @DetalleProductos;

    COMMIT TRANSACTION;
END;
GO

--Update
Create or Alter Procedure UpdatePedidosConDetalle
    @IdPedido INT,
    @NumeroOrden INT,
    @IdCliente INT,
    @FechaPedido DATETIME,
    @DireccionEntrega VARCHAR(500),
    @FechaEntrega DATETIME,
    @Subtotal MONEY,
    @IVA MONEY,
    @Total MONEY,
    @Entregado BIT,
	@Notas VARCHAR(500),
    @UpdatedDetalleProductos PedidosDetalleType READONLY
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Update Pedidos (Master)
        UPDATE Pedidos
        SET NumeroOrden = @NumeroOrden,
            IdCliente = @IdCliente,
            FechaPedido = @FechaPedido,
            DireccionEntrega = @DireccionEntrega,
            FechaEntrega = @FechaEntrega,
            Subtotal = @Subtotal,
            IVA = @IVA,
            Total = @Total,
            Entregado = @Entregado,
			Notas = @Notas
        WHERE IdPedido = @IdPedido;

        -- Remueve datos no encontrados en @UpdatedDetalleProductos
        DELETE FROM PedidosDetalle
        WHERE IdPedido = @IdPedido
          AND IdProducto NOT IN (SELECT IdProducto FROM @UpdatedDetalleProductos);

        -- Inserta o actualiza en PedidosDetalle basado en los datos de @UpdatedDetalleProductos
        MERGE PedidosDetalle AS Target
        USING @UpdatedDetalleProductos AS Source
        ON Target.IdPedido = @IdPedido AND Target.IdProducto = Source.IdProducto
        WHEN MATCHED THEN
            UPDATE SET
                Cantidad = Source.Cantidad,
                PrecioUnitario = Source.PrecioUnitario,
                Total = Source.Total
        WHEN NOT MATCHED BY TARGET THEN
            INSERT (IdPedido, IdProducto, Cantidad, PrecioUnitario, Total)
            VALUES (@IdPedido, IdProducto, Cantidad, PrecioUnitario, Total);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

--Delete
Create or Alter Procedure DeletePedidosConDetalle
    @IdPedido INT
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Borrar primero de tabla detalle
        DELETE FROM PedidosDetalle
        WHERE IdPedido = @IdPedido;

        -- Borrar de tabla maestra
        DELETE FROM Pedidos
        WHERE IdPedido = @IdPedido;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO
