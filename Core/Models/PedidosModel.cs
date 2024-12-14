namespace Core.Models
{
    public class PedidosModel
    {
        public int IdPedido { get; set; } = 0;
        public int NumeroOrden { get; set; } = 0;
        public int IdCliente { get; set; } = 0;
        public ClientesModel Cliente { get; set; } = new();
        public DateTime FechaPedido { get; set; } = new();
        public string DireccionEntrega { get; set; } = string.Empty;
        public DateTime FechaEntrega { get; set; } = new();
        public decimal Subtotal { get; set; } = 0M;
        public decimal IVA { get; set; } = 0M;
        public decimal Total { get; set; } = 0M;
        public bool Entregado { get; set; } = false;
        public string Notas { get; set; } = string.Empty;
        public List<PedidosDetalleModel> DetallePedido { get; set; } = [];
    }
}
