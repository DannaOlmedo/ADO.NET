using System.Data;
using MySql.Data.MySqlClient;
using ProductoApi.Data;
using ProductoApi.Interfaces;
using ProductoApi.Models;

namespace ProductoApi.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly DatabaseConnection _dbConnection;

        public ProductoRepository(DatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public List<Producto> ObtenerTodos()
        {
            var lista = new List<Producto>();
            using var connection = (MySqlConnection)_dbConnection.CreateConnection();
            connection.Open();
            var query = "SELECT Id, Nombre, Precio, Stock FROM Productos";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                lista.Add(new Producto
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Precio = reader.GetDecimal("Precio"),
                    Stock = reader.GetInt32("Stock")
                });
            }
            return lista;
        }

        public Producto? ObtenerPorId(int id)
        {
            using var connection = (MySqlConnection)_dbConnection.CreateConnection();
            connection.Open();
            var query = "SELECT Id, Nombre, Precio, Stock FROM Productos WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new Producto
                {
                    Id = reader.GetInt32("Id"),
                    Nombre = reader.GetString("Nombre"),
                    Precio = reader.GetDecimal("Precio"),
                    Stock = reader.GetInt32("Stock")
                };
            }
            return null;
        }

        public int Insertar(Producto producto)
        {
            using var connection = (MySqlConnection)_dbConnection.CreateConnection();
            connection.Open();
            var query = "INSERT INTO Productos (Nombre, Precio, Stock) VALUES (@nombre, @precio, @stock); SELECT LAST_INSERT_ID();";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", producto.Nombre);
            command.Parameters.AddWithValue("@precio", producto.Precio);
            command.Parameters.AddWithValue("@stock", producto.Stock);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public bool Actualizar(Producto producto)
        {
            using var connection = (MySqlConnection)_dbConnection.CreateConnection();
            connection.Open();
            var query = "UPDATE Productos SET Nombre = @nombre, Precio = @precio, Stock = @stock WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", producto.Id);
            command.Parameters.AddWithValue("@nombre", producto.Nombre);
            command.Parameters.AddWithValue("@precio", producto.Precio);
            command.Parameters.AddWithValue("@stock", producto.Stock);

            return command.ExecuteNonQuery() > 0;
        }

        public bool Eliminar(int id)
        {
            using var connection = (MySqlConnection)_dbConnection.CreateConnection();
            connection.Open();
            var query = "DELETE FROM Productos WHERE Id = @id";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            return command.ExecuteNonQuery() > 0;
        }
    }
}