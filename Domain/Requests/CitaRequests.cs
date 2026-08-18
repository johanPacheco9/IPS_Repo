using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Requests;

public class CrearDisponibilidadRequest
{
    [Required(ErrorMessage = "Debe seleccionar un profesional")]
    public Guid ProfesionalId { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime Fecha { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La hora de inicio es obligatoria")]
    public TimeSpan HoraInicio { get; set; } = new TimeSpan(8, 0, 0);

    [Required(ErrorMessage = "La hora final es obligatoria")]
    public TimeSpan HoraFin { get; set; } = new TimeSpan(17, 0, 0);

    public int DuracionMinutosCita { get; set; } = 20;
}

public class AgendarCitaRequest
{
    [Required(ErrorMessage = "Debe seleccionar un paciente")]
    public Guid PacienteId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un profesional")]
    public Guid ProfesionalId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar la fecha y hora")]
    public DateTime FechaHora { get; set; } = DateTime.Now.AddHours(1);

    public int DuracionMinutos { get; set; } = 20;

    [Required(ErrorMessage = "El motivo de consulta es obligatorio")]
    public string MotivoConsulta { get; set; } = string.Empty;

    public string Observaciones { get; set; } = string.Empty;
}

public class CambiarEstadoCitaRequest
{
    public Guid CitaId { get; set; }
    public EstadoCita NuevoEstado { get; set; }
    public string Motivo { get; set; } = string.Empty;
}
