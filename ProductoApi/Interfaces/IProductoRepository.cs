using ProductoApi.Models;

namespace ProductoApi.Interfaces
{
    public interface IProductoRepository
    {
        List<Producto> ObtenerTodos();
        Producto? ObtenerPorId(int id);
        int Insertar(Producto producto);
        bool Actualizar(Producto producto);
        bool Eliminar(int id);
    }
}