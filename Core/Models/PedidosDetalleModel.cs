namespace Core.Models
{
    public class PedidosDetalleModel
    {
        public int IdPedido { get; set; } = 0;
        public int IdProducto { get; set; } = 0;
        public ProductosModel Producto { get; set; } = new();
        public int Cantidad { get; set; } = 0;
        public decimal PrecioUnitario { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
    }
}
