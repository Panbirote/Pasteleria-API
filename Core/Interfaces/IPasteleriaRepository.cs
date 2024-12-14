using Core.Models;

namespace Core.Interfaces
{
    public interface IPasteleriaRepository
    {
        #region MateriaPrima
        Task<MateriaPrimaModel> SelectMateriaPrima(int id);
        Task<List<MateriaPrimaModel>> SelectAllMateriasPrimas();
        Task<bool> InsertMateriaPrima(MateriaPrimaModel materiaPrima);
        Task<bool> UpdateMateriaPrima(MateriaPrimaModel materiaPrima);
        Task<bool> DeleteMateriaPrima(int id);
        #endregion

        #region Recetas
        Task<RecetaModel> SelectReceta(int id);
        Task<List<RecetaModel>> SelectAllRecetas();
        Task<bool> InsertReceta(RecetaModel receta);
        Task<bool> UpdateReceta(RecetaModel receta);
        Task<bool> DeleteReceta(int id);
        #endregion

        #region Productos
        Task<ProductosModel> SelectProducto(int id);
        Task<List<ProductosModel>> SelectAllProductos();
        Task<bool> InsertProducto(ProductosModel producto);
        Task<bool> UpdateProducto(ProductosModel producto);
        Task<bool> DeleteProducto(int id);
        #endregion

        #region Clientes
        Task<ClientesModel> SelectCliente(int id);
        Task<List<ClientesModel>> SelectAllClientes();
        Task<bool> InsertCliente(ClientesModel cliente);
        Task<bool> UpdateCliente(ClientesModel cliente);
        Task<bool> DeleteCliente(int id);
        #endregion

        #region Pedidos
        Task<PedidosModel> SelectPedido(int id);
        Task<List<PedidosModel>> SelectAllPedidos();
        Task<List<PedidosModel>> SelectAllPedidosByIdCliente(int idCliente);
        Task<bool> InsertPedido(PedidosModel pedido);
        Task<bool> UpdatePedido(PedidosModel pedido);
        Task<bool> DeletePedido(int id);
        #endregion
    }
}
