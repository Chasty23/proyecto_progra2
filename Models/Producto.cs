namespace SuperFreshAPI.Models
{
    public class Producto
    {

        public int ProductoId { get; set; }

        public int Cantidad { get; set; }

        public string Nombre { get; set; }

        public string  Descripcion { get; set; }

        public decimal Precio { get; set; }

        public string ImagenFileName { get; set; }


        //Relaciones

        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }


    }
}
