using SuperFreshAPI.Models;

namespace SuperFreshAPI.Interfaces
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetCategorias();

        Categoria GetCategoria(int id);
        IEnumerable<Producto> GetProductosPorCategoria(int categoriaId);

        bool CategoriaExists(int id);
        bool CreateCategoria(Categoria categoria);

        bool UpdateCategoria(Categoria categoria);

        bool DeleteCategoria(Categoria categoria);

        bool Save();

    }
}
