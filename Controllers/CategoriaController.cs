using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperFreshAPI.Dto;
using SuperFreshAPI.Interfaces;
using SuperFreshAPI.Models;
using System.Security.Cryptography;

namespace SuperFreshAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Categoria>))]

        public IActionResult GetCategorias()
        {
            var categorias = _mapper.Map<List<CategoriaDto>>(_categoriaRepository.GetCategorias());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            return Ok(categorias);
        }

        [HttpGet("{categoriaId}")]
        [ProducesResponseType(200, Type = typeof(Categoria))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoria(int categoriaId) 
        {
            if (!_categoriaRepository.CategoriaExists(categoriaId))
            {
                return NotFound();
            }

            var categoria = _mapper.Map<CategoriaDto>(_categoriaRepository.GetCategoria(categoriaId));
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(categoria);
        }

        [HttpGet("producto/{categoriaId}")]
        [ProducesResponseType(200, Type = typeof(Producto))]
        [ProducesResponseType(400)]

        public IActionResult GetProductosPorCategoria(int categoriaId)
        {
            var productos = _mapper.Map<List<ProductoDto>>(
                _categoriaRepository.GetProductosPorCategoria(categoriaId));

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(productos);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CrearCategoria([FromForm] CategoriaDto categoriaCreada)
        {
            if (categoriaCreada == null)
            {
                return BadRequest(ModelState);
            }
            var categoria = _categoriaRepository.GetCategorias()
                .Where(c => c.Nombre.Trim().ToUpper() == categoriaCreada.Nombre.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (categoria != null)
            {
                ModelState.AddModelError("","Categoria existente");
                return StatusCode(442,ModelState); 
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoriaMap = _mapper.Map<Categoria>(categoriaCreada);

            if (!_categoriaRepository.CreateCategoria(categoriaMap))
            {
                ModelState.AddModelError("","error");
                return StatusCode(500,ModelState);
            }

            return Ok("Creada correctamente");
        }

        [HttpPut("{categoriaId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult UpdateCategoria(int categoriaId, [FromForm] CategoriaDto actualizarCategoria) 
        {

            if (actualizarCategoria == null)
            {
                return BadRequest(ModelState);
            }

            if (categoriaId != actualizarCategoria.CategoriaId)
            {
                return BadRequest(ModelState);
            }

            if (!_categoriaRepository.CategoriaExists(categoriaId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var categoriaMap = _mapper.Map<Categoria>(actualizarCategoria);

            if (!_categoriaRepository.UpdateCategoria(categoriaMap))
            {
                ModelState.AddModelError("","Algo salio mal en la actualizacion");
                return StatusCode(500,ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoriaId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]

        public IActionResult DeleteCategoria(int categoriaId) {

            if (!_categoriaRepository.CategoriaExists(categoriaId))
            {
                return NotFound();
            }

            var categoriaEliminada = _categoriaRepository.GetCategoria(categoriaId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_categoriaRepository.DeleteCategoria(categoriaEliminada))
            {
                ModelState.AddModelError("","Error al momento de eliminar la categoria");
            }

            return NoContent();
        }

    }
}
