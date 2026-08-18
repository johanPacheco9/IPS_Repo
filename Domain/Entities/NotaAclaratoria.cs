namespace Domain.Entities;

public class NotaAclaratoria
{
    public Guid Id { get; private set; }
    public Guid HistoriaClinicaId { get; private set; }
    public Guid ProfesionalId { get; private set; }
    public DateTime FechaHora { get; private set; }
    public string Contenido { get; private set; } = string.Empty;

    // Navegación
    public ProfesionalSalud? Profesional { get; private set; }

    private NotaAclaratoria() { }

    internal NotaAclaratoria(Guid historiaClinicaId, Guid profesionalId, string contenido)
    {
        Id = Guid.NewGuid();
        HistoriaClinicaId = historiaClinicaId;
        ProfesionalId = profesionalId;
        FechaHora = DateTime.UtcNow;
        Contenido = contenido.Trim();
    }
}
