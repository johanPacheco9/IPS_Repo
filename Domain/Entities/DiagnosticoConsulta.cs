using Domain.Enums;

namespace Domain.Entities;

public class DiagnosticoConsulta
{
    public Guid Id { get; private set; }
    public Guid HistoriaClinicaId { get; private set; }
    public string CodigoCIE11 { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public TipoDiagnostico TipoDiagnostico { get; private set; }
    public bool EsPrincipal { get; private set; }

    private DiagnosticoConsulta() { }

    internal DiagnosticoConsulta(Guid historiaClinicaId, string codigoCIE11, string descripcion, TipoDiagnostico tipoDiagnostico, bool esPrincipal)
    {
        Id = Guid.NewGuid();
        HistoriaClinicaId = historiaClinicaId;
        CodigoCIE11 = codigoCIE11.Trim();
        Descripcion = descripcion?.Trim() ?? string.Empty;
        TipoDiagnostico = tipoDiagnostico;
        EsPrincipal = esPrincipal;
    }
}
