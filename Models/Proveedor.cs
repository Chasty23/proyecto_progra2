namespace SuperFreshAPI.Models
{
    public class Proveedor
    {
        public int ProveedorId { get; set; }

        public string Nombre { get; set; }

        public ICollection<Producto> Productos { get; set; }

    }
}
