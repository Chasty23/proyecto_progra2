namespace SuperFreshAPI.Models
{
    public class Categoria
    {

        public int CategoriaId { get; set; }

        public string Nombre { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}
