namespace SuperFreshAPI.Dto
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public IFormFile ImagenFile { get; set; } 
    }
}
