using Domain.Enums;

namespace Domain.DTOs;

public record AdmisionDto(
    Guid Id,
    Guid CitaMedicaId,
    Guid PacienteId,
    string NombrePaciente,
    string DocumentoPaciente,
    TipoContrato TipoContrato,
    string EPS,
    string NumeroAutorizacion,
    decimal ValorConsulta,
    decimal CopagoOCuotaModeradora,
    decimal ValorNeto,
    DateTime FechaAdmision
);
