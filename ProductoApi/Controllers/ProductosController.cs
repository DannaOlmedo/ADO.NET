using Microsoft.AspNetCore.Mvc;
using ProductoApi.Interfaces;
using ProductoApi.Models;

namespace ProductoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IProductoRepository _productoRepository;

    public ProductosController(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    [HttpGet]
    public ActionResult<List<Producto>> Get()
    {
        return Ok(_productoRepository.ObtenerTodos());
    }

    [HttpGet("{id:int}")]
    public ActionResult<Producto> GetById(int id)
    {
        var producto = _productoRepository.ObtenerPorId(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpPost]
    public ActionResult<Producto> Post([FromBody] Producto producto)
    {
        var id = _productoRepository.Insertar(producto);
        producto.Id = id;
        return CreatedAtAction(nameof(GetById), new { id }, producto);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, [FromBody] Producto producto)
    {
        if (id != producto.Id)
        {
            return BadRequest();
        }

        var actualizado = _productoRepository.Actualizar(producto);
        return actualizado ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var eliminado = _productoRepository.Eliminar(id);
        return eliminado ? NoContent() : NotFound();
    }
}
