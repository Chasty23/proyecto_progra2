using SuperFreshAPI.Models;

namespace SuperFreshAPI.Interfaces
{
    public interface IProductoRepository
    {

        IEnumerable<Producto> GetProductos();

        Producto GetProducto(int id);

        bool ProductoExists(int productoId);

        bool CreateProducto(Producto producto);
        bool UpdateProducto(Producto producto);

        bool DeleteProducto(Producto producto);

        bool Save();
    }
}
