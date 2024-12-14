CREATE TABLE InventarioMateriaPrima (
	IdMateriaPrima INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Descripcion VARCHAR(255),
    Entradas INT DEFAULT 0,
    Salidas INT DEFAULT 0,
    StockActual INT DEFAULT 0,
    UnidadMedida VARCHAR(50));
GO

CREATE TABLE Recetas (
	IdReceta INT IDENTITY (1,1) PRIMARY KEY NOT NULL,
    NombreReceta VARCHAR(255),
    Descripcion VARCHAR(500));
GO

CREATE TABLE RecetasDetalle (
	IdReceta INT NOT NULL FOREIGN KEY REFERENCES Recetas(IdReceta),
    IdMateriaPrima INT NOT NULL FOREIGN KEY REFERENCES InventarioMateriaPrima(IdMateriaPrima),
    Cantidad INT NOT NULL,
	PRIMARY KEY (IdReceta, IdMateriaPrima));
GO

CREATE TABLE Productos (
	IdProducto INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    IdReceta INT NOT NULL FOREIGN KEY REFERENCES Recetas(IdReceta),
    Entradas INT DEFAULT 0,
    Salidas INT DEFAULT 0,
    PrecioUnitario MONEY NOT NULL,
    Iva MONEY NOT NULL,
    NombreProducto VARCHAR(255),
    Descripcion VARCHAR(500),
    StockActual INT DEFAULT 0);
GO

CREATE TABLE Clientes (
	IdCliente INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    IdUsuarioWeb VARCHAR(255),
    Nombre VARCHAR(255),
    Telefono VARCHAR(50),
    Email VARCHAR(255),
    Direccion VARCHAR(500),
    Rfc VARCHAR(50),
    CodigoPostal INT,
    Curp VARCHAR(50),
    Activo BIT DEFAULT 1,
    Municipio VARCHAR(255),
    Estado VARCHAR(255),
    Pais VARCHAR(255),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Notas VARCHAR(500));
GO

CREATE TABLE Pedidos (
	IdPedido INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    NumeroOrden INT NOT NULL,
    IdCliente INT NOT NULL FOREIGN KEY REFERENCES Clientes(IdCliente),
    FechaPedido DATETIME NOT NULL DEFAULT GETDATE(),
    DireccionEntrega VARCHAR(500),
    FechaEntrega DATETIME,
    Subtotal MONEY NOT NULL,
    IVA MONEY NOT NULL,
    Total MONEY NOT NULL,
    Entregado BIT DEFAULT 0,
    Notas VARCHAR(500));
GO

CREATE TABLE PedidosDetalle (
	IdPedido INT NOT NULL FOREIGN KEY REFERENCES Pedidos(IdPedido),
    IdProducto INT NOT NULL FOREIGN KEY REFERENCES Productos(IdProducto),
    Cantidad INT NOT NULL,
    Total MONEY NOT NULL,
    PrecioUnitario MONEY NOT NULL,
    PRIMARY KEY (IdPedido, IdProducto));
GO
