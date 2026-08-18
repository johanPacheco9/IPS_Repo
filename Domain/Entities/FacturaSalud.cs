namespace Domain.Entities;

public class FacturaSalud
{
    public Guid Id { get; private set; }
    public Guid AdmisionId { get; private set; }
    public Guid PacienteId { get; private set; }
    public string NumeroFactura { get; private set; } = string.Empty;
    public DateTime FechaEmision { get; private set; }
    public decimal ValorBruto { get; private set; }
    public decimal ValorCopago { get; private set; }
    public decimal ValorDescuento { get; private set; }
    public decimal ValorNeto { get; private set; }
    public string CUFE { get; private set; } = string.Empty; // Código Único de Facturación Electrónica DIAN
    public string Estado { get; private set; } = "Emitida";

    // Navegación
    public Admision? Admision { get; private set; }
    public Paciente? Paciente { get; private set; }

    private FacturaSalud() { }

    public FacturaSalud(
        Guid admisionId,
        Guid pacienteId,
        string numeroFactura,
        decimal valorBruto,
        decimal valorCopago,
        decimal valorDescuento = 0,
        string cufe = "")
    {
        Id = Guid.NewGuid();
        AdmisionId = admisionId;
        PacienteId = pacienteId;
        NumeroFactura = numeroFactura;
        FechaEmision = DateTime.UtcNow;
        ValorBruto = valorBruto;
        ValorCopago = valorCopago;
        ValorDescuento = valorDescuento;
        ValorNeto = Math.Max(0, valorBruto - valorCopago - valorDescuento);
        CUFE = string.IsNullOrWhiteSpace(cufe) ? Guid.NewGuid().ToString("N") : cufe;
        Estado = "Emitida";
    }
}
