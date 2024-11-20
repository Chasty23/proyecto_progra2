using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.OpenApi.Writers;
using SuperFreshAPI.Dto;
using SuperFreshAPI.Interfaces;
using SuperFreshAPI.Models;

namespace SuperFreshAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        public readonly IProductoRepository _productoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public ProductoController(IProductoRepository productoRepository, ICategoriaRepository categoriaRepository,
            IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
            _productoRepository = productoRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Producto>))]
        public IActionResult GetProductos()
        {
            var productos = _mapper.Map<List<ProductoDto>>(_productoRepository.GetProductos());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(productos);
        }

        [HttpGet("{productoId}")]
        [ProducesResponseType(200, Type = typeof(Producto))]
        [ProducesResponseType(400)]
        public IActionResult GetProducto(int productoId)
        {
            if (!_productoRepository.ProductoExists(productoId))
            {
                return NotFound();
            }

            var producto = _mapper.Map<ProductoDto>(_productoRepository.GetProducto(productoId));

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(producto);
        }



        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateProducto([FromQuery] int categoriaId, [FromForm] ProductoDto crearProducto)
        {
            if (crearProducto == null)
            {
                return BadRequest(ModelState);
            }

            var productos = _productoRepository.GetProductos()
                 .Where(c => c.Nombre.Trim().ToUpper() == crearProducto.Nombre.TrimEnd().ToUpper())
                 .FirstOrDefault();

            if (productos != null)
            {
                ModelState.AddModelError("", "Producto Existente");
                return StatusCode(442, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapear el DTO al modelo
            var productoMap = _mapper.Map<Producto>(crearProducto);
            productoMap.Categoria = _categoriaRepository.GetCategoria(categoriaId);

            // Verificar y guardar la imagen
            if (crearProducto.ImagenFile != null && crearProducto.ImagenFile.Length > 0)
            {
                // Obtener la ruta de almacenamiento (ajustar según tus necesidades)
                var folderPath = Path.Combine("wwwroot", "images");
                Directory.CreateDirectory(folderPath);  // Crear la carpeta si no existe

                // Asignar un nombre único al archivo
                var FileName = $"{crearProducto.ImagenFile.FileName}";
                var filePath = Path.Combine(folderPath, FileName);

                // Guardar el archivo en la ruta especificada
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await crearProducto.ImagenFile.CopyToAsync(stream);
                }

                // Asignar el nombre del archivo al campo `ImagenFileName` en el modelo
                productoMap.ImagenFileName = FileName;
            }

            if (!_productoRepository.CreateProducto(productoMap))
            {
                ModelState.AddModelError("", "Algo salió mal mientras se guardaba el producto");
                return StatusCode(500, ModelState);
            }

            return Ok("Creado correctamente");
        }

        [HttpPut("{productoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateProducto(int productoId, [FromForm] ProductoDto actualizarProducto)
        {
            if (actualizarProducto == null || productoId <= 0)
            {
                return BadRequest(ModelState);
            }

            if (!_productoRepository.ProductoExists(productoId))
            {
                ModelState.AddModelError("", "El producto no existe");
                return NotFound(ModelState);
            }

            var productoMap = _productoRepository.GetProducto(productoId);
            if (productoMap == null)
            {
                return NotFound("Producto no encontrado");
            }

            // Mapear los datos del DTO al modelo existente
            _mapper.Map(actualizarProducto, productoMap);

            // Verificar y guardar la nueva imagen, si existe
            if (actualizarProducto.ImagenFile != null && actualizarProducto.ImagenFile.Length > 0)
            {
                // Eliminar la imagen anterior, si existe
                if (!string.IsNullOrEmpty(productoMap.ImagenFileName))
                {
                    var oldImagePath = Path.Combine("wwwroot", "images", productoMap.ImagenFileName);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Guardar la nueva imagen
                var folderPath = Path.Combine("wwwroot", "images");
                Directory.CreateDirectory(folderPath);  // Crear la carpeta si no existe

                var FileName = $"{actualizarProducto.ImagenFile.FileName}";
                var filePath = Path.Combine(folderPath, FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await actualizarProducto.ImagenFile.CopyToAsync(stream);
                }

                productoMap.ImagenFileName = FileName;
            }

            if (!_productoRepository.UpdateProducto(productoMap))
            {
                ModelState.AddModelError("", "Algo salió mal mientras se actualizaba el producto");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{productoId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteProducto(int productoId)
        {
            if (!_productoRepository.ProductoExists(productoId))
            {
                return NotFound();
            }

            var productoEliminado = _productoRepository.GetProducto(productoId);

            // Eliminar la imagen asociada, si existe
            if (!string.IsNullOrEmpty(productoEliminado.ImagenFileName))
            {
                var imagePath = Path.Combine("wwwroot", "images", productoEliminado.ImagenFileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_productoRepository.DeleteProducto(productoEliminado))
            {
                ModelState.AddModelError("","Error al momento de eliminar el producto");
            }

            return NoContent();
        }

    }
}
