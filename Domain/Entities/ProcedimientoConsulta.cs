namespace Domain.Entities;

public class ProcedimientoConsulta
{
    public Guid Id { get; private set; }
    public Guid HistoriaClinicaId { get; private set; }
    public string CodigoCUPS { get; private set; } = string.Empty;
    public string NombreProcedimiento { get; private set; } = string.Empty;
    public int Cantidad { get; private set; }
    public string Observaciones { get; private set; } = string.Empty;

    private ProcedimientoConsulta() { }

    internal ProcedimientoConsulta(Guid historiaClinicaId, string codigoCUPS, string nombreProcedimiento, int cantidad, string observaciones)
    {
        Id = Guid.NewGuid();
        HistoriaClinicaId = historiaClinicaId;
        CodigoCUPS = codigoCUPS.Trim();
        NombreProcedimiento = nombreProcedimiento?.Trim() ?? string.Empty;
        Cantidad = cantidad > 0 ? cantidad : 1;
        Observaciones = observaciones?.Trim() ?? string.Empty;
    }
}
