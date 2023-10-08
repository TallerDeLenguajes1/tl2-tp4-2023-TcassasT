using Microsoft.AspNetCore.Mvc;
using EspacioCadeteria;
using EspacioPedido;
using EspacioCadete;
using EspacioInforme;

[ApiController]
[Route("[controller]")]
public class CadeteriaController: ControllerBase {
  private Cadeteria cadeteria = Cadeteria.GetCadeteria();
  private readonly ILogger<CadeteriaController> _logger;
  public CadeteriaController(ILogger<CadeteriaController> logger) {
    _logger = logger;
  }

  [HttpGet("GetPedidos")]
  public IEnumerable<Pedido> GetPedidos() {
    return this.cadeteria.Pedidos;
  }

  [HttpGet("GetCadetes")]
  public IEnumerable<Cadete> GetCadetes() {
    return this.cadeteria.ListadoCadetes;
  }

  [HttpGet("GetInforme")]
  public Informe GetInforme() {
    return this.cadeteria.GenerarInforme();
  }

  [HttpPost("AsignarPedido")]
  public IActionResult AddPedido(int pedidoNro, int cadeteId) {
    Pedido pedidoAModificar = this.cadeteria.GetPedidoByNro(pedidoNro);
    if (pedidoAModificar == null) {
      return NotFound("No se encontro pedido de numero: " + pedidoNro);
    }

    Cadete cadeteAAsignar = this.cadeteria.GetCadeteById(cadeteId);
    if (cadeteAAsignar == null) {
      return NotFound("No se encontro cadete de id: " + cadeteId);
    }

    return Ok(this.cadeteria.AsignarPedidoACadete(pedidoAModificar.Nro, cadeteAAsignar.Id));
  }

  [HttpPost("AgregarPedido")]
  public IActionResult AgregarPedido(Pedido pedido) {
    this.cadeteria.AgregarPedido(pedido);
    return Ok("Pedido agregado");
  }

  [HttpPost("CambiarEstadoPedido")]
  public IActionResult CambiarEstadoPedido(int pedidoNro, PEDIDO_ESTADOS nuevoEstado) {
    Pedido pedidoAModificar = this.cadeteria.GetPedidoByNro(pedidoNro);
    if (pedidoAModificar == null) {
      return NotFound("No se encontro pedido de numero: " + pedidoNro);
    }

    pedidoAModificar.ActualizarEstado(nuevoEstado);
    if (pedidoAModificar.Estado.Equals(nuevoEstado)) {
      return Ok(); 
    } else {
      return BadRequest("No se pudo actualizar estado de pedido");
    }
  }

  [HttpPost("CambiarCadetePedido")]
  public IActionResult CambiarCadetePedido(int pedidoNro, int idNuevoCadete) {
    Pedido pedidoAModificar = this.cadeteria.GetPedidoByNro(pedidoNro);
    if (pedidoAModificar == null) {
      return NotFound("No se encontro pedido de numero: " + pedidoNro);
    }

    Cadete nuevoCadete = this.cadeteria.GetCadeteById(idNuevoCadete);
    if (nuevoCadete == null) {
      return NotFound("No se encontro cadete de id: " + idNuevoCadete);
    }

    pedidoAModificar.AsignarPedido(nuevoCadete.Id);

    if (pedidoAModificar.CadeteId.Equals(nuevoCadete)) {
      return Ok(); 
    } else {
      return BadRequest("No se pudo actualizar asignacin de pedido");
    }
  }

  [HttpGet("pedidos/{pedidoNro}")]
  public IActionResult GetOne(int pedidoNro) {
    Pedido pedido = this.cadeteria.GetPedidoByNro(pedidoNro);

    if (pedido != null) {
      return Ok(pedido);
    } else {
      return NotFound("No existe pedido de numero " + pedidoNro);
    }
  }
}