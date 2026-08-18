namespace Domain.Entities;

using Domain.Enums;

public class Paciente
{
    public Guid Id { get; set; }
    public TipoDocumento TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; } = string.Empty;
    public string PrimerNombre { get; set; } = string.Empty;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = string.Empty;
    public string? SegundoApellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public Genero Genero { get; set; }
    public int EpsId { get; set; }
    public Eps? EPS { get; set; }
    public RegimenSalud RegimenSalud { get; set; }
    public string Telefono { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public EstadoPaciente Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
}