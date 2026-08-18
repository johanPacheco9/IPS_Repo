using Domain.Enums;

namespace Domain.DTOs;

public record CitaDto(
    Guid Id,
    Guid PacienteId,
    string NombrePaciente,
    string DocumentoPaciente,
    Guid ProfesionalId,
    string NombreProfesional,
    string Especialidad,
    DateTime FechaHora,
    int DuracionMinutos,
    EstadoCita Estado,
    string MotivoConsulta,
    string Observaciones
);

public record DisponibilidadDto(
    Guid Id,
    Guid ProfesionalId,
    string NombreProfesional,
    DateTime Fecha,
    TimeSpan HoraInicio,
    TimeSpan HoraFin,
    int DuracionMinutosCita
);
