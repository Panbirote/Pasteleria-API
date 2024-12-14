using Core.Interfaces;
using Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Pasteleria.Repositories
{
    public class PasteleriaRepository(IConexionDBRepository conexionDBRepository) : IPasteleriaRepository
    {
        private readonly IConexionDBRepository _BDconnection = conexionDBRepository;

        #region Materias primas
        public async Task<MateriaPrimaModel> SelectMateriaPrima(int id)
        {
            MateriaPrimaModel materiaPrima = new();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectInventarioMateriaPrimaById";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@IdMateriaPrima", SqlDbType = SqlDbType.Int, SqlValue = id });

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        materiaPrima = new MateriaPrimaModel
                        {
                            IdMateriaPrima = reader.GetInt32(reader.GetOrdinal("IdMateriaPrima")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            Entradas = reader.GetInt32(reader.GetOrdinal("Entradas")),
                            Salidas = reader.GetInt32(reader.GetOrdinal("Salidas")),
                            StockActual = reader.GetInt32(reader.GetOrdinal("StockActual")),
                            UnidadMedida = reader.GetString(reader.GetOrdinal("UnidadMedida"))
                        };
                    }
                }
            });

            return materiaPrima;
        }

        public async Task<List<MateriaPrimaModel>> SelectAllMateriasPrimas()
        {
            var materiasPrimas = new List<MateriaPrimaModel>();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectAllInventarioMateriaPrima";
                cmd.Parameters.Clear();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        materiasPrimas.Add(new MateriaPrimaModel
                        {
                            IdMateriaPrima = reader.GetInt32(reader.GetOrdinal("IdMateriaPrima")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            Entradas = reader.GetInt32(reader.GetOrdinal("Entradas")),
                            Salidas = reader.GetInt32(reader.GetOrdinal("Salidas")),
                            StockActual = reader.GetInt32(reader.GetOrdinal("StockActual")),
                            UnidadMedida = reader.GetString(reader.GetOrdinal("UnidadMedida"))
                        });
                    }
                }
            });

            return materiasPrimas;
        }

        public async Task<bool> InsertMateriaPrima(MateriaPrimaModel materiaPrima)
        {
            bool result = false;

            //Inicia transaccion para ejecutar stored procedure
            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "InsertInventarioMateriaPrima";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Descripcion", SqlDbType = SqlDbType.VarChar, SqlValue = materiaPrima.Descripcion });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Entradas", SqlDbType = SqlDbType.Int, SqlValue = materiaPrima.Entradas });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Salidas", SqlDbType = SqlDbType.Int, SqlValue = materiaPrima.Salidas });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StockActual", SqlDbType = SqlDbType.Int, SqlValue = materiaPrima.StockActual });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@UnidadMedida", SqlDbType = SqlDbType.VarChar, SqlValue = materiaPrima.UnidadMedida });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> UpdateMateriaPrima(MateriaPrimaModel materiaPrima)
        {
            bool result = false;

            //Inicia transaccion para ejecutar stored procedure
            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "UpdateInventarioMateriaPrima";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@IdMateriaPrima", SqlDbType = SqlDbType.Int, SqlValue = materiaPrima.IdMateriaPrima });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Descripcion", SqlDbType = SqlDbType.VarChar, SqlValue = materiaPrima.Descripcion });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Entradas", SqlDbType = SqlDbType.Int, SqlValue = materiaPrima.Entradas });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Salidas", SqlDbType = SqlDbType.Int, SqlValue = materiaPrima.Salidas });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@StockActual", SqlDbType = SqlDbType.Int, SqlValue = materiaPrima.StockActual });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@UnidadMedida", SqlDbType = SqlDbType.VarChar, SqlValue = materiaPrima.UnidadMedida });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> DeleteMateriaPrima(int id)
        {
            bool result = false;

            //Inicia transaccion para ejecutar stored procedure
            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "DeleteInventarioMateriaPrima";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@IdMateriaPrima", SqlDbType = SqlDbType.Int, SqlValue = id });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        #endregion

        #region Recetas
        public async Task<RecetaModel> SelectReceta(int id)
        {
            RecetaModel receta = new();
            var detalles = new List<RecetaDetalleModel>();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectRecetasById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@IdReceta", SqlDbType.Int) { Value = id });

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        if (receta.IdReceta <= 0)
                        {
                            receta = new RecetaModel
                            {
                                IdReceta = reader.GetInt32(reader.GetOrdinal("IdReceta")),
                                NombreReceta = reader.GetString(reader.GetOrdinal("NombreReceta")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                DetalleReceta = new List<RecetaDetalleModel>()
                            };
                        }

                        receta.DetalleReceta.Add(new RecetaDetalleModel
                        {
                            IdReceta = receta.IdReceta,
                            IdMateriaPrima = reader.GetInt32(reader.GetOrdinal("IdMateriaPrima")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                            Descripcion = reader.GetString(reader.GetOrdinal("MateriaPrimaDescripcion")),
                            UnidadMedida = reader.GetString(reader.GetOrdinal("UnidadMedida"))
                        });
                    }
                }
            });

            return receta;
        }

        public async Task<List<RecetaModel>> SelectAllRecetas()
        {
            // Diccionario para agrupar recetas
            var recetas = new Dictionary<int, RecetaModel>();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectAllRecetas";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int idReceta = reader.GetInt32(reader.GetOrdinal("IdReceta"));

                        if (!recetas.ContainsKey(idReceta))
                        {
                            recetas[idReceta] = new RecetaModel
                            {
                                IdReceta = idReceta,
                                NombreReceta = reader.GetString(reader.GetOrdinal("NombreReceta")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                DetalleReceta = new List<RecetaDetalleModel>()
                            };
                        }

                        recetas[idReceta].DetalleReceta.Add(new RecetaDetalleModel
                        {
                            IdReceta = idReceta,
                            IdMateriaPrima = reader.GetInt32(reader.GetOrdinal("IdMateriaPrima")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                            Descripcion = reader.GetString(reader.GetOrdinal("MateriaPrimaDescripcion")),
                            UnidadMedida = reader.GetString(reader.GetOrdinal("UnidadMedida"))
                        });
                    }
                }
            });

            return [.. recetas.Values];
        }

        public async Task<bool> InsertReceta(RecetaModel receta)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "InsertRecetaConDetalle";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter { ParameterName = "@NombreReceta", SqlDbType = SqlDbType.VarChar, Value = receta.NombreReceta });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Descripcion", SqlDbType = SqlDbType.VarChar, Value = receta.Descripcion });

                // Se crea tabla detalle para insertar en BD
                var detalleTable = new DataTable();
                detalleTable.Columns.Add("IdMateriaPrima", typeof(int));
                detalleTable.Columns.Add("Cantidad", typeof(int));

                foreach (var detalle in receta.DetalleReceta)
                {
                    detalleTable.Rows.Add(detalle.IdMateriaPrima, detalle.Cantidad);
                }

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DetallesReceta",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "DetalleRecetaType",
                    Value = detalleTable
                });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> UpdateReceta(RecetaModel receta)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "UpdateRecetaConDetalle";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter { ParameterName = "@IdReceta", SqlDbType = SqlDbType.Int, Value = receta.IdReceta });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@NombreReceta", SqlDbType = SqlDbType.VarChar, Value = receta.NombreReceta });
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@Descripcion", SqlDbType = SqlDbType.VarChar, Value = receta.Descripcion });

                // Se crea tabla para insertar detalle en BD
                var detalleTable = new DataTable();
                detalleTable.Columns.Add("IdMateriaPrima", typeof(int));
                detalleTable.Columns.Add("Cantidad", typeof(int));

                foreach (var detalle in receta.DetalleReceta)
                {
                    detalleTable.Rows.Add(detalle.IdMateriaPrima, detalle.Cantidad);
                }

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DetallesReceta",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "DetalleRecetaType",
                    Value = detalleTable
                });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> DeleteReceta(int id)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "DeleteReceta";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter { ParameterName = "@IdReceta", SqlDbType = SqlDbType.Int, Value = id });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        #endregion

        #region Productos
        public async Task<ProductosModel> SelectProducto(int id)
        {
            ProductosModel producto = new();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectProductosById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.Int) { Value = id });

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        producto = new ProductosModel
                        {
                            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                            IdReceta = reader.GetInt32(reader.GetOrdinal("IdReceta")),
                            Entradas = reader.GetInt32(reader.GetOrdinal("Entradas")),
                            Salidas = reader.GetInt32(reader.GetOrdinal("Salidas")),
                            PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                            IVA = reader.GetDecimal(reader.GetOrdinal("IVA")),
                            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            StockActual = reader.GetInt32(reader.GetOrdinal("StockActual"))
                        };
                    }
                }
            });

            if (producto != null && producto.IdReceta > 0)
                producto.Receta = await SelectReceta(producto.IdReceta);

            return producto;
        }

        public async Task<List<ProductosModel>> SelectAllProductos()
        {
            var productos = new List<ProductosModel>();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectAllProductos";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var producto = new ProductosModel
                        {
                            IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                            IdReceta = reader.GetInt32(reader.GetOrdinal("IdReceta")),
                            Entradas = reader.GetInt32(reader.GetOrdinal("Entradas")),
                            Salidas = reader.GetInt32(reader.GetOrdinal("Salidas")),
                            PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                            IVA = reader.GetDecimal(reader.GetOrdinal("IVA")),
                            NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                            StockActual = reader.GetInt32(reader.GetOrdinal("StockActual"))
                        };

                        productos.Add(producto);
                    }
                }
            });

            // Se popula la lista de recetas en parelelo (multiples operaciones a la vez)
            var peticiones = productos.Where(p => p.IdReceta > 0).Select(async producto =>
            {
                producto.Receta = await SelectReceta(producto.IdReceta);
            });
            await Task.WhenAll(peticiones);

            return productos;
        }

        public async Task<bool> InsertProducto(ProductosModel producto)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "InsertProductos";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter("@IdReceta", SqlDbType.Int) { Value = producto.IdReceta });
                cmd.Parameters.Add(new SqlParameter("@Entradas", SqlDbType.Int) { Value = producto.Entradas });
                cmd.Parameters.Add(new SqlParameter("@Salidas", SqlDbType.Int) { Value = producto.Salidas });
                cmd.Parameters.Add(new SqlParameter("@PrecioUnitario", SqlDbType.Money) { Value = producto.PrecioUnitario });
                cmd.Parameters.Add(new SqlParameter("@Iva", SqlDbType.Money) { Value = producto.IVA });
                cmd.Parameters.Add(new SqlParameter("@NombreProducto", SqlDbType.VarChar, 255) { Value = producto.NombreProducto });
                cmd.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.VarChar, 500) { Value = producto.Descripcion });
                cmd.Parameters.Add(new SqlParameter("@StockActual", SqlDbType.Int) { Value = producto.StockActual });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> UpdateProducto(ProductosModel producto)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "UpdateProductos";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.Int) { Value = producto.IdProducto });
                cmd.Parameters.Add(new SqlParameter("@IdReceta", SqlDbType.Int) { Value = producto.IdReceta });
                cmd.Parameters.Add(new SqlParameter("@Entradas", SqlDbType.Int) { Value = producto.Entradas });
                cmd.Parameters.Add(new SqlParameter("@Salidas", SqlDbType.Int) { Value = producto.Salidas });
                cmd.Parameters.Add(new SqlParameter("@PrecioUnitario", SqlDbType.Money) { Value = producto.PrecioUnitario });
                cmd.Parameters.Add(new SqlParameter("@Iva", SqlDbType.Money) { Value = producto.IVA });
                cmd.Parameters.Add(new SqlParameter("@NombreProducto", SqlDbType.VarChar, 255) { Value = producto.NombreProducto });
                cmd.Parameters.Add(new SqlParameter("@Descripcion", SqlDbType.VarChar, 500) { Value = producto.Descripcion });
                cmd.Parameters.Add(new SqlParameter("@StockActual", SqlDbType.Int) { Value = producto.StockActual });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> DeleteProducto(int id)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "DeleteProductos";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@IdProducto", SqlDbType.Int) { Value = id });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        #endregion

        #region Clientes
        public async Task<ClientesModel> SelectCliente(int id)
        {
            ClientesModel cliente = new();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectClientesById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int) { Value = id });

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    cliente = new ClientesModel
                    {
                        IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                        IdUsuarioWeb = reader.GetString(reader.GetOrdinal("IdUsuarioWeb")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
                        RFC = reader.GetString(reader.GetOrdinal("Rfc")),
                        CodigoPostal = reader.GetInt32(reader.GetOrdinal("CodigoPostal")),
                        CURP = reader.GetString(reader.GetOrdinal("Curp")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                        Municipio = reader.GetString(reader.GetOrdinal("Municipio")),
                        Estado = reader.GetString(reader.GetOrdinal("Estado")),
                        Pais = reader.GetString(reader.GetOrdinal("Pais")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        Notas = reader.GetString(reader.GetOrdinal("Notas"))
                    };
                }
            });

            return cliente;
        }

        public async Task<List<ClientesModel>> SelectAllClientes()
        {
            List<ClientesModel> clientes = [];

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectAllClientes";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    clientes.Add(new ClientesModel
                    {
                        IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                        IdUsuarioWeb = reader.GetString(reader.GetOrdinal("IdUsuarioWeb")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Direccion = reader.GetString(reader.GetOrdinal("Direccion")),
                        RFC = reader.GetString(reader.GetOrdinal("Rfc")),
                        CodigoPostal = reader.GetInt32(reader.GetOrdinal("CodigoPostal")),
                        CURP = reader.GetString(reader.GetOrdinal("Curp")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                        Municipio = reader.GetString(reader.GetOrdinal("Municipio")),
                        Estado = reader.GetString(reader.GetOrdinal("Estado")),
                        Pais = reader.GetString(reader.GetOrdinal("Pais")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                        Notas = reader.GetString(reader.GetOrdinal("Notas"))
                    });
                }
            });

            return clientes;
        }

        public async Task<bool> InsertCliente(ClientesModel cliente)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "InsertClientes";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter("@IdUsuarioWeb", SqlDbType.VarChar, 255) { Value = cliente.IdUsuarioWeb });
                cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar, 255) { Value = cliente.Nombre });
                cmd.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.VarChar, 50) { Value = cliente.Telefono });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 255) { Value = cliente.Email });
                cmd.Parameters.Add(new SqlParameter("@Direccion", SqlDbType.VarChar, 500) { Value = cliente.Direccion });
                cmd.Parameters.Add(new SqlParameter("@Rfc", SqlDbType.VarChar, 50) { Value = cliente.RFC });
                cmd.Parameters.Add(new SqlParameter("@CodigoPostal", SqlDbType.Int) { Value = cliente.CodigoPostal });
                cmd.Parameters.Add(new SqlParameter("@Curp", SqlDbType.VarChar, 50) { Value = cliente.CURP });
                cmd.Parameters.Add(new SqlParameter("@Activo", SqlDbType.Bit) { Value = cliente.Activo });
                cmd.Parameters.Add(new SqlParameter("@Municipio", SqlDbType.VarChar, 255) { Value = cliente.Municipio });
                cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.VarChar, 255) { Value = cliente.Estado });
                cmd.Parameters.Add(new SqlParameter("@Pais", SqlDbType.VarChar, 255) { Value = cliente.Pais });
                cmd.Parameters.Add(new SqlParameter("@Notas", SqlDbType.VarChar, 500) { Value = cliente.Notas });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> UpdateCliente(ClientesModel cliente)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "UpdateClientes";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int) { Value = cliente.IdCliente });
                cmd.Parameters.Add(new SqlParameter("@IdUsuarioWeb", SqlDbType.VarChar, 255) { Value = cliente.IdUsuarioWeb });
                cmd.Parameters.Add(new SqlParameter("@Nombre", SqlDbType.VarChar, 255) { Value = cliente.Nombre });
                cmd.Parameters.Add(new SqlParameter("@Telefono", SqlDbType.VarChar, 50) { Value = cliente.Telefono });
                cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 255) { Value = cliente.Email });
                cmd.Parameters.Add(new SqlParameter("@Direccion", SqlDbType.VarChar, 500) { Value = cliente.Direccion });
                cmd.Parameters.Add(new SqlParameter("@Rfc", SqlDbType.VarChar, 50) { Value = cliente.RFC });
                cmd.Parameters.Add(new SqlParameter("@CodigoPostal", SqlDbType.Int) { Value = cliente.CodigoPostal });
                cmd.Parameters.Add(new SqlParameter("@Curp", SqlDbType.VarChar, 50) { Value = cliente.CURP });
                cmd.Parameters.Add(new SqlParameter("@Activo", SqlDbType.Bit) { Value = cliente.Activo });
                cmd.Parameters.Add(new SqlParameter("@Municipio", SqlDbType.VarChar, 255) { Value = cliente.Municipio });
                cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.VarChar, 255) { Value = cliente.Estado });
                cmd.Parameters.Add(new SqlParameter("@Pais", SqlDbType.VarChar, 255) { Value = cliente.Pais });
                cmd.Parameters.Add(new SqlParameter("@Notas", SqlDbType.VarChar, 500) { Value = cliente.Notas });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> DeleteCliente(int id)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "DeleteClientes";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int) { Value = id });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        #endregion

        #region Pedidos
        public async Task<PedidosModel> SelectPedido(int id)
        {
            PedidosModel pedido = new();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectPedidosById";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdPedido", SqlDbType.Int) { Value = id });

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        pedido = new PedidosModel
                        {
                            IdPedido = reader.GetInt32("IdPedido"),
                            NumeroOrden = reader.GetInt32("NumeroOrden"),
                            IdCliente = reader.GetInt32("IdCliente"),
                            FechaPedido = reader.GetDateTime("FechaPedido"),
                            DireccionEntrega = reader.GetString("DireccionEntrega"),
                            FechaEntrega = reader.GetDateTime("FechaEntrega"),
                            Subtotal = reader.GetDecimal("Subtotal"),
                            IVA = reader.GetDecimal("IVA"),
                            Total = reader.GetDecimal("Total"),
                            Entregado = reader.GetBoolean("Entregado"),
                            Notas = reader.GetString("Notas"),
                            Cliente = await SelectCliente(reader.GetInt32("IdCliente")),
                            DetallePedido = []
                        };
                    }
                }
                pedido.DetallePedido = await SelectPedidosDetalle(pedido.IdPedido);

            });

            return pedido;
        }

        public async Task<List<PedidosModel>> SelectAllPedidos()
        {
            var pedidos = new List<PedidosModel>();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectAllPedidos";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var pedido = new PedidosModel
                        {
                            IdPedido = reader.GetInt32("IdPedido"),
                            NumeroOrden = reader.GetInt32("NumeroOrden"),
                            IdCliente = reader.GetInt32("IdCliente"),
                            FechaPedido = reader.GetDateTime("FechaPedido"),
                            DireccionEntrega = reader.GetString("DireccionEntrega"),
                            FechaEntrega = reader.GetDateTime("FechaEntrega"),
                            Subtotal = reader.GetDecimal("Subtotal"),
                            IVA = reader.GetDecimal("IVA"),
                            Total = reader.GetDecimal("Total"),
                            Entregado = reader.GetBoolean("Entregado"),
                            Notas = reader.GetString("Notas"),
                            Cliente = await SelectCliente(reader.GetInt32("IdCliente")),
                            DetallePedido = []
                        };

                        pedido.DetallePedido = await SelectPedidosDetalle(pedido.IdPedido);

                        pedidos.Add(pedido);
                    }
                }
            });

            return pedidos;
        }

        public async Task<List<PedidosModel>> SelectAllPedidosByIdCliente(int idCliente)
        {
            var pedidos = new List<PedidosModel>();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectPedidosByIdCliente";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@IdCliente", SqlDbType.Int) { Value = idCliente });

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var pedido = new PedidosModel
                        {
                            IdPedido = reader.GetInt32("IdPedido"),
                            NumeroOrden = reader.GetInt32("NumeroOrden"),
                            IdCliente = reader.GetInt32("IdCliente"),
                            FechaPedido = reader.GetDateTime("FechaPedido"),
                            DireccionEntrega = reader.GetString("DireccionEntrega"),
                            FechaEntrega = reader.GetDateTime("FechaEntrega"),
                            Subtotal = reader.GetDecimal("Subtotal"),
                            IVA = reader.GetDecimal("IVA"),
                            Total = reader.GetDecimal("Total"),
                            Entregado = reader.GetBoolean("Entregado"),
                            Notas = reader.GetString("Notas"),
                            Cliente = await SelectCliente(reader.GetInt32("IdCliente")),
                            DetallePedido = []
                        };

                        pedido.DetallePedido = await SelectPedidosDetalle(pedido.IdPedido);

                        pedidos.Add(pedido);
                    }
                }
            });

            return pedidos;
        }

        public async Task<bool> InsertPedido(PedidosModel pedido)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "InsertPedidosConDetalle";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();

                // Maestro
                cmd.Parameters.AddWithValue("@NumeroOrden", pedido.NumeroOrden);
                cmd.Parameters.AddWithValue("@IdCliente", pedido.IdCliente);
                cmd.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
                cmd.Parameters.AddWithValue("@DireccionEntrega", pedido.DireccionEntrega);
                cmd.Parameters.AddWithValue("@FechaEntrega", pedido.FechaEntrega);
                cmd.Parameters.AddWithValue("@Subtotal", pedido.Subtotal);
                cmd.Parameters.AddWithValue("@IVA", pedido.IVA);
                cmd.Parameters.AddWithValue("@Total", pedido.Total);
                cmd.Parameters.AddWithValue("@Entregado", pedido.Entregado);
                cmd.Parameters.AddWithValue("@Notas", pedido.Notas);

                // Detalle
                var detalleProductos = new DataTable();
                detalleProductos.Columns.Add("IdProducto", typeof(int)); 
                detalleProductos.Columns.Add("Cantidad", typeof(int));
                detalleProductos.Columns.Add("PrecioUnitario", typeof(decimal));
                detalleProductos.Columns.Add("Total", typeof(decimal));

                foreach (var detalle in pedido.DetallePedido)
                {
                    detalleProductos.Rows.Add(detalle.IdProducto, detalle.Cantidad, detalle.PrecioUnitario, detalle.Total);
                }

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@DetalleProductos",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "PedidosDetalleType",
                    Value = detalleProductos
                });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> UpdatePedido(PedidosModel pedido)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "UpdatePedidosConDetalle";
                cmd.CommandType = CommandType.StoredProcedure;

                // Maestro
                cmd.Parameters.AddWithValue("@IdPedido", pedido.IdPedido);
                cmd.Parameters.AddWithValue("@NumeroOrden", pedido.NumeroOrden);
                cmd.Parameters.AddWithValue("@IdCliente", pedido.IdCliente);
                cmd.Parameters.AddWithValue("@FechaPedido", pedido.FechaPedido);
                cmd.Parameters.AddWithValue("@DireccionEntrega", pedido.DireccionEntrega);
                cmd.Parameters.AddWithValue("@FechaEntrega", pedido.FechaEntrega);
                cmd.Parameters.AddWithValue("@Subtotal", pedido.Subtotal);
                cmd.Parameters.AddWithValue("@IVA", pedido.IVA);
                cmd.Parameters.AddWithValue("@Total", pedido.Total);
                cmd.Parameters.AddWithValue("@Entregado", pedido.Entregado);
                cmd.Parameters.AddWithValue("@Notas", pedido.Notas);

                // Detalle
                var detalleProductos = new DataTable();
                detalleProductos.Columns.Add("IdProducto", typeof(int));
                detalleProductos.Columns.Add("Cantidad", typeof(int));
                detalleProductos.Columns.Add("PrecioUnitario", typeof(decimal));
                detalleProductos.Columns.Add("Total", typeof(decimal));

                foreach (var detalle in pedido.DetallePedido)
                {
                    detalleProductos.Rows.Add(detalle.IdProducto, detalle.Cantidad, detalle.PrecioUnitario, detalle.Total);
                }

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@UpdatedDetalleProductos",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "PedidosDetalleType",
                    Value = detalleProductos
                });

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        public async Task<bool> DeletePedido(int id)
        {
            bool result = false;

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "DeletePedidosConDetalle";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPedido", id);

                result = await cmd.ExecuteNonQueryAsync() > 0;
            });

            return result;
        }

        private async Task<List<PedidosDetalleModel>> SelectPedidosDetalle(int idPedido)
        {
            var detalles = new List<PedidosDetalleModel>();

            await _BDconnection.Execute(async cmd =>
            {
                cmd.CommandText = "SelectPedidosDetalleByIdPedido";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@IdPedido", SqlDbType.Int) { Value = idPedido });

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var detalle = new PedidosDetalleModel
                        {
                            IdPedido = reader.GetInt32("IdPedido"),
                            IdProducto = reader.GetInt32("IdProducto"),
                            Cantidad = reader.GetInt32("Cantidad"),
                            PrecioUnitario = reader.GetDecimal("PrecioUnitario"),
                            Total = reader.GetDecimal("Total"),
                        };

                        detalle.Producto = await SelectProducto(detalle.IdProducto);

                        detalles.Add(detalle);
                    }
                }
            });

            return detalles;
        }

        #endregion
    }
}
