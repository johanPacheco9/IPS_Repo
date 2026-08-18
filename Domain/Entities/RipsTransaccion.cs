namespace Domain.Entities;

public class RipsTransaccion
{
    public Guid Id { get; private set; }
    public Guid FacturaId { get; private set; }
    public string CUV { get; private set; } = string.Empty; // Código Único de Validación Ministerio de Salud
    public string JsonRips { get; private set; } = string.Empty; // Estructura JSON Res 2275 de 2023
    public DateTime FechaGeneracion { get; private set; }
    public string EstadoValidacion { get; private set; } = "Pendiente";

    // Navegación
    public FacturaSalud? Factura { get; private set; }

    private RipsTransaccion() { }

    public RipsTransaccion(Guid facturaId, string jsonRips, string cuv = "")
    {
        Id = Guid.NewGuid();
        FacturaId = facturaId;
        JsonRips = jsonRips;
        CUV = cuv;
        FechaGeneracion = DateTime.UtcNow;
        EstadoValidacion = string.IsNullOrWhiteSpace(cuv) ? "Pendiente" : "Validado";
    }

    public void AsignarCUV(string cuv)
    {
        if (string.IsNullOrWhiteSpace(cuv))
            throw new ArgumentException("El CUV es obligatorio.");

        CUV = cuv.Trim();
        EstadoValidacion = "Validado";
    }
}
