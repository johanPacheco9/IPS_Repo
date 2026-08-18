using Domain.Enums;

namespace Domain.Entities;

public class CitaMedica
{
    public Guid Id { get; private set; }
    public Guid PacienteId { get; private set; }
    public Guid ProfesionalId { get; private set; }
    public DateTime FechaHora { get; private set; }
    public int DuracionMinutos { get; private set; }
    public EstadoCita Estado { get; private set; }
    public string MotivoConsulta { get; private set; } = string.Empty;
    public string Observaciones { get; private set; } = string.Empty;
    public DateTime FechaCreacion { get; private set; }

    // Propiedades de navegación EF
    public Paciente? Paciente { get; private set; }
    public ProfesionalSalud? Profesional { get; private set; }

    private CitaMedica() { }

    public CitaMedica(Guid pacienteId, Guid profesionalId, DateTime fechaHora, int duracionMinutos, string motivoConsulta, string observaciones = "")
    {
        if (pacienteId == Guid.Empty) throw new ArgumentException("El paciente es obligatorio.", nameof(pacienteId));
        if (profesionalId == Guid.Empty) throw new ArgumentException("El profesional de la salud es obligatorio.", nameof(profesionalId));
        if (string.IsNullOrWhiteSpace(motivoConsulta)) throw new ArgumentException("El motivo de consulta es obligatorio.", nameof(motivoConsulta));

        Id = Guid.NewGuid();
        PacienteId = pacienteId;
        ProfesionalId = profesionalId;
        FechaHora = fechaHora;
        DuracionMinutos = duracionMinutos > 0 ? duracionMinutos : 20;
        MotivoConsulta = motivoConsulta.Trim();
        Observaciones = observaciones?.Trim() ?? string.Empty;
        Estado = EstadoCita.Programada;
        FechaCreacion = DateTime.UtcNow;
    }

    public void MarcarEnSalaDeEspera()
    {
        if (Estado != EstadoCita.Programada)
            throw new InvalidOperationException($"No se puede admitir una cita en estado '{Estado}'. Debe estar Programada.");

        Estado = EstadoCita.EnSalaDeEspera;
    }

    public void IniciarConsulta()
    {
        if (Estado != EstadoCita.EnSalaDeEspera)
            throw new InvalidOperationException($"No se puede iniciar consulta para una cita que no esté 'En Sala de Espera'. Estado actual: {Estado}.");

        Estado = EstadoCita.EnConsulta;
    }

    public void Atender()
    {
        if (Estado != EstadoCita.EnConsulta)
            throw new InvalidOperationException($"Solo se pueden marcar como Atendida las citas que estén 'En Consulta'. Estado actual: {Estado}.");

        Estado = EstadoCita.Atendida;
    }

    public void Cancelar(string motivoCancelacion)
    {
        if (Estado == EstadoCita.Atendida)
            throw new InvalidOperationException("No se puede cancelar una cita que ya fue atendida.");

        Estado = EstadoCita.Cancelada;
        Observaciones = string.IsNullOrWhiteSpace(Observaciones) 
            ? $"[CANCELADA]: {motivoCancelacion}" 
            : $"{Observaciones} | [CANCELADA]: {motivoCancelacion}";
    }
}
