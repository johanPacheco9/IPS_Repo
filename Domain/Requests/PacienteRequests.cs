using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Requests;

public class CreatePacienteRequest
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

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateTime FechaNacimiento { get; set; } = DateTime.Today.AddYears(-25);

    [Required(ErrorMessage = "El género es obligatorio")]
    public Genero Genero { get; set; } = Genero.Masculino;

    [Required(ErrorMessage = "Debe seleccionar una EPS")]
    public int EpsId { get; set; }

    [Required(ErrorMessage = "El régimen de salud es obligatorio")]
    public RegimenSalud RegimenSalud { get; set; } = RegimenSalud.Contributivo;

    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdatePacienteRequest : CreatePacienteRequest
{
    public Guid Id { get; set; }
    public EstadoPaciente Estado { get; set; } = EstadoPaciente.Activo;
}
