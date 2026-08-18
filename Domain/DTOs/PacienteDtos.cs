using Domain.Enums;

namespace Domain.DTOs;

public record PacienteDto(
    Guid Id,
    TipoDocumento TipoDocumento,
    string NumeroDocumento,
    string PrimerNombre,
    string? SegundoNombre,
    string PrimerApellido,
    string? SegundoApellido,
    string NombreCompleto,
    DateTime FechaNacimiento,
    int Edad,
    Genero Genero,
    int EpsId,
    string EpsNombre,
    RegimenSalud RegimenSalud,
    string Telefono,
    string Email,
    EstadoPaciente Estado
);

public record EpsDto(
    int Id,
    string Codigo,
    string Nombre,
    string Nit,
    bool Activo
);
