using EspacioCliente;
using EspacioCadete;

namespace EspacioPedido;

public enum PEDIDO_ESTADOS {
  CANCELADO,
  PENDIENTE,
  ASIGNADO,
  EN_CAMINO,
  COMPLETADO,
}

public class Pedido {
  private int nro;
  private string? obs;
  private Cliente cliente;
  private PEDIDO_ESTADOS estado;
  private int cadeteId;

  public int Nro { get => nro; set => nro = value; }
  public string? Obs { get => obs; set => obs = value; }
  public PEDIDO_ESTADOS Estado { get => estado; set { estado = Enum.TryParse<PEDIDO_ESTADOS>(value.ToString(), out PEDIDO_ESTADOS nuevoEstado) ? nuevoEstado : PEDIDO_ESTADOS.PENDIENTE; }}
  public int CadeteId { get => cadeteId; set => cadeteId = value; }
  public Cliente Cliente { get => cliente; set => cliente = value; }
  public const int PRECIO_PEDIDO = 500;

  public Pedido(int nro, string? obs, Cliente cliente) {
    this.nro = nro;
    this.obs = obs;
    this.cliente = cliente;
    this.estado = PEDIDO_ESTADOS.PENDIENTE;
  }
  public Pedido() {}

  public void ActualizarEstado(PEDIDO_ESTADOS nuevoEstado) {
    this.estado = nuevoEstado;
  }

  public Pedido AsignarPedido(int cadeteId) {
    int cadeteAnterior = this.cadeteId;
    this.cadeteId = cadeteId;
    return this;
  }

  public void Cancelar() {
    this.estado = PEDIDO_ESTADOS.CANCELADO;
  }

  public override string ToString() {
    string pedidoString = "Pedido N° " + this.Nro;
    if (!String.IsNullOrEmpty(this.Obs)) {
      pedidoString += " (" + this.Obs + ").";
    }
    return pedidoString;
  }
}