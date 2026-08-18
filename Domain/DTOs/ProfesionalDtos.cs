using Domain.Enums;

namespace Domain.DTOs;

public record ProfesionalDto(
    Guid Id,
    TipoDocumento TipoDocumento,
    string NumeroDocumento,
    string PrimerNombre,
    string? SegundoNombre,
    string PrimerApellido,
    string? SegundoApellido,
    string NombreCompleto,
    string NumeroRethus,
    string Especialidad,
    string Telefono,
    string Email,
    bool Activo
);
