using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Requests;

public class CreateProfesionalRequest
{
    [Required(ErrorMessage = "El tipo de documento es obligatorio")]
    public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.CC;

    [Required(ErrorMessage = "El número de documento es obligatorio")]
    public string NumeroDocumento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    public string PrimerNombre { get; set; } = string.Empty;

    public string? SegundoNombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    public string PrimerApellido { get; set; } = string.Empty;

    public string? SegundoApellido { get; set; }

    [Required(ErrorMessage = "El número RETHUS es obligatorio por ley colombiana")]
    public string NumeroRethus { get; set; } = string.Empty;

    [Required(ErrorMessage = "La especialidad es obligatoria")]
    public string Especialidad { get; set; } = "Medicina General";

    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateProfesionalRequest : CreateProfesionalRequest
{
    public Guid Id { get; set; }
    public bool Activo { get; set; } = true;
}
