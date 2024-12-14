namespace Core.Models
{
    public class MateriaPrimaModel
    {
        public int IdMateriaPrima { get; set; } = 0;
        public string Descripcion { get; set; } = string.Empty;
        public int Entradas { get; set; } = 0;
        public int Salidas { get; set; } = 0;
        public int StockActual { get; set; } = 0;
        public string UnidadMedida { get; set; } = string.Empty;
    }
}
