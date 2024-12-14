namespace Core.Models
{
    public class RecetaModel
    {
        public int IdReceta { get; set; } = 0;
        public string NombreReceta { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<RecetaDetalleModel> DetalleReceta { get; set; } = [];
    }
    public class RecetaDetalleModel
    {
        public int IdReceta { get; set; }
        public int IdMateriaPrima { get; set; } = 0;
        public int Cantidad { get; set; } = 0;
        public string Descripcion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
    }
}
