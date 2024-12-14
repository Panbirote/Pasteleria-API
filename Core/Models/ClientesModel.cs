namespace Core.Models
{
    public class ClientesModel
    {
        public int IdCliente { get; set; } = 0;
        public string IdUsuarioWeb { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = "Enrique Segoviano";
        public string RFC { get; set; } = string.Empty;
        public int CodigoPostal { get; set; } = 0;
        public string CURP { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public string Municipio { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string Notas { get; set; } = string.Empty;
    }
}
