namespace Core.Models
{
    public class ProductosModel
    {
        public int IdProducto { get; set; } = 0;
        public int IdReceta { get; set; } = 0;
        public RecetaModel Receta { get; set; } = new();
        public int Entradas { get; set; } = 0;
        public int Salidas { get; set; } = 0;
        public decimal PrecioUnitario { get; set; } = 0M;
        public decimal IVA { get; set; } = 0M;
        public string NombreProducto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int StockActual { get; set; } = 0;
    }
}
