using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Pasteleria.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PasteleriaController(IPasteleriaRepository pasteleriaRepository) : ControllerBase
    {
        private readonly IPasteleriaRepository _pasteleriaRepository = pasteleriaRepository;

        #region Materia Prima
        [HttpGet("GetMateriaPrimaById")]
        public async Task<IActionResult> GetMateriaPrima(int id)
        {
            var result = await _pasteleriaRepository.SelectMateriaPrima(id);
            return Ok(result);
        }

        [HttpGet("GetAllMateriasPrimas")]
        public async Task<IActionResult> GetAllMateriaPrima()
        {
            var result = await _pasteleriaRepository.SelectAllMateriasPrimas();
            return Ok(result);
        }

        [HttpPost("InsertMateriaPrima")]
        public async Task<IActionResult> InsertMateriaPrima([FromBody]MateriaPrimaModel materiaPrima)
        {
            var result = await _pasteleriaRepository.InsertMateriaPrima(materiaPrima);
            return Ok(result);
        }

        [HttpPut("UpdateMateriaPrima")]
        public async Task<IActionResult> UpdateMateriaPrima([FromBody]MateriaPrimaModel materiaPrima)
        {
            var result = await _pasteleriaRepository.UpdateMateriaPrima(materiaPrima);
            return Ok(result);
        }

        [HttpDelete("DeleteMateriaPrima")]
        public async Task<IActionResult> DeleteMateriaPrima(int id)
        {
            var result = await _pasteleriaRepository.DeleteMateriaPrima(id);
            return Ok(result);
        }

        #endregion

        #region Recetas
        [HttpGet("GetRecetaById")]
        public async Task<IActionResult> GetRecetaById(int id)
        {
            var result = await _pasteleriaRepository.SelectReceta(id);
            return Ok(result);
        }

        [HttpGet("GetAllRecetas")]
        public async Task<IActionResult> GetAllRecetas()
        {
            var result = await _pasteleriaRepository.SelectAllRecetas();
            return Ok(result);
        }

        [HttpPost("InsertReceta")]
        public async Task<IActionResult> InsertReceta([FromBody]RecetaModel receta)
        {
            var result = await _pasteleriaRepository.InsertReceta(receta);
            return Ok(result);
        }

        [HttpPut("UpdateReceta")]
        public async Task<IActionResult> UpdateReceta([FromBody]RecetaModel receta)
        {
            var result = await _pasteleriaRepository.UpdateReceta(receta);
            return Ok(result);
        }

        [HttpDelete("DeleteReceta")]
        public async Task<IActionResult> DeleteReceta(int id)
        {
            var result = await _pasteleriaRepository.DeleteReceta(id);
            return Ok(result);
        }

        #endregion

        #region Productos
        [HttpGet("GetProductoById")]
        public async Task<IActionResult> GetProductoById(int id)
        {
            var result = await _pasteleriaRepository.SelectProducto(id);
            return Ok(result);
        }

        [HttpGet("GetAllProductos")]
        public async Task<IActionResult> GetAllProductos()
        {
            var result = await _pasteleriaRepository.SelectAllProductos();
            return Ok(result);
        }

        [HttpPost("InsertProducto")]
        public async Task<IActionResult> InsertProducto([FromBody]ProductosModel producto)
        {
            var result = await _pasteleriaRepository.InsertProducto(producto);
            return Ok(result);
        }

        [HttpPut("UpdateProducto")]
        public async Task<IActionResult> UpdateProducto([FromBody]ProductosModel producto)
        {
            var result = await _pasteleriaRepository.UpdateProducto(producto);
            return Ok(result);
        }

        [HttpDelete("DeleteProducto")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var result = await _pasteleriaRepository.DeleteProducto(id);
            return Ok(result);
        }

        #endregion

        #region Clientes
        [HttpGet("GetClienteById")]
        public async Task<IActionResult> GetClientesById(int id)
        {
            var result = await _pasteleriaRepository.SelectCliente(id);
            return Ok(result);
        }

        [HttpGet("GetAllClientes")]
        public async Task<IActionResult> GetAllClientes()
        {
            var result = await _pasteleriaRepository.SelectAllClientes();
            return Ok(result);
        }

        [HttpPost("InsertCliente")]
        public async Task<IActionResult> InsertCliente([FromBody]ClientesModel cliente)
        {
            var result = await _pasteleriaRepository.InsertCliente(cliente);
            return Ok(result);
        }

        [HttpPut("UpdateCliente")]
        public async Task<IActionResult> UpdateCliente([FromBody]ClientesModel cliente)
        {
            var result = await _pasteleriaRepository.UpdateCliente(cliente);
            return Ok(result);
        }

        [HttpDelete("DeleteCliente")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var result = await _pasteleriaRepository.DeleteCliente(id);
            return Ok(result);
        }

        #endregion

        #region Pedidos
        [HttpGet("GetPedidoById")]
        public async Task<IActionResult> GetPedidoById(int id)
        {
            var result = await _pasteleriaRepository.SelectPedido(id);
            return Ok(result);
        }

        [HttpGet("GetAllPedidos")]
        public async Task<IActionResult> GetAllPedidos()
        {
            var result = await _pasteleriaRepository.SelectAllPedidos();
            return Ok(result);
        }

        [HttpGet("GetAllPedidosByIdCliente")]
        public async Task<IActionResult> GetPedidosByIdCliente(int idCliente)
        {
            var result = await _pasteleriaRepository.SelectAllPedidosByIdCliente(idCliente);
            return Ok(result);
        }

        [HttpPost("InsertPedido")]
        public async Task<IActionResult> InsertPedido([FromBody]PedidosModel pedido)
        {
            var result = await _pasteleriaRepository.InsertPedido(pedido);
            return Ok(result);
        }

        [HttpPut("UpdatePedido")]
        public async Task<IActionResult> UpdatePedido([FromBody]PedidosModel pedido)
        {
            var result = await _pasteleriaRepository.UpdatePedido(pedido);
            return Ok(result);
        }

        [HttpDelete("DeletePedido")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var result = await _pasteleriaRepository.DeletePedido(id);
            return Ok(result);
        }

        #endregion
    }
}
