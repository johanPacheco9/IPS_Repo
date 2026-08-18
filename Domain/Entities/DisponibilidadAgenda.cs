namespace Domain.Entities;

public class DisponibilidadAgenda
{
    public Guid Id { get; private set; }
    public Guid ProfesionalId { get; private set; }
    public DateTime Fecha { get; private set; }
    public TimeSpan HoraInicio { get; private set; }
    public TimeSpan HoraFin { get; private set; }
    public int DuracionMinutosCita { get; private set; }
    public bool Activo { get; private set; }

    // Navegación
    public ProfesionalSalud? Profesional { get; private set; }

    private DisponibilidadAgenda() { }

    public DisponibilidadAgenda(Guid profesionalId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int duracionMinutosCita = 20)
    {
        if (horaFin <= horaInicio)
            throw new ArgumentException("La hora final de la agenda debe ser mayor a la hora inicial.", nameof(horaFin));

        if (duracionMinutosCita <= 0)
            throw new ArgumentException("La duración de la cita debe ser mayor a 0 minutos.", nameof(duracionMinutosCita));

        Id = Guid.NewGuid();
        ProfesionalId = profesionalId;
        Fecha = fecha.Date;
        HoraInicio = horaInicio;
        HoraFin = horaFin;
        DuracionMinutosCita = duracionMinutosCita;
        Activo = true;
    }

    public void Desactivar() => Activo = false;
}
