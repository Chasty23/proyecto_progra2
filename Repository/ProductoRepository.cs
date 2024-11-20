using Microsoft.EntityFrameworkCore;
using SuperFreshAPI.Context;
using SuperFreshAPI.Interfaces;
using SuperFreshAPI.Models;

namespace SuperFreshAPI.Repository
{
    public class ProductoRepository : IProductoRepository
    {

        private readonly AppDBContext _context;

        public ProductoRepository(AppDBContext context) 
        { 
            _context = context;
        
        }
        public bool CreateProducto(Producto producto)
        {
            _context.Add(producto);
            return Save();
        }

        public bool DeleteProducto(Producto producto)
        {
            _context.Remove(producto);
            return Save();
        }

        public IEnumerable<Producto> GetProductos()
        {
            return _context.Productos.ToList();
        }

        public Producto GetProducto(int id)
        {
            return _context.Productos.Where(p => p.ProductoId == id).FirstOrDefault();
        }



        public bool ProductoExists(int productoId)
        {
            return _context.Productos.Any(p => p.ProductoId == productoId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateProducto(Producto producto)
        {
            _context.Update(producto);
            return Save();
        }
    }
}
