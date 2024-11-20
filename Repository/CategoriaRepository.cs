using Microsoft.EntityFrameworkCore.Diagnostics;
using SuperFreshAPI.Context;
using SuperFreshAPI.Interfaces;
using SuperFreshAPI.Models;

namespace SuperFreshAPI.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {

        private readonly AppDBContext _context;

        public CategoriaRepository(AppDBContext context)
        {
            _context = context;
        }

        public bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(c => c.CategoriaId == id);
        }

        public bool CreateCategoria(Categoria categoria)
        {
            _context.Add(categoria);
            return Save();
        }

        public bool DeleteCategoria(Categoria categoria)
        {
            _context.Remove(categoria); 
            return Save();
        }

        public IEnumerable<Categoria> GetCategorias() 
        {
            return _context.Categorias.ToList();
        }

        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.Where(e  => e.CategoriaId == id).FirstOrDefault();
        }

        public IEnumerable<Producto> GetProductosPorCategoria(int categoriaId)
        {
            return _context.Productos
                   .Where(p => p.CategoriaId == categoriaId)
                   .ToList();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategoria(Categoria categoria)
        {
            _context.Update(categoria);
            return Save();
        }
    }
}
