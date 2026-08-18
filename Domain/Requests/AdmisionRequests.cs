using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Requests;

public class RegistrarAdmisionRequest
{
    [Required(ErrorMessage = "La cita médica es obligatoria")]
    public Guid CitaMedicaId { get; set; }

    [Required(ErrorMessage = "El paciente es obligatorio")]
    public Guid PacienteId { get; set; }

    [Required(ErrorMessage = "El tipo de contrato es obligatorio")]
    public TipoContrato TipoContrato { get; set; } = TipoContrato.ConvenioEPS;

    public string EPS { get; set; } = string.Empty;

    public string NumeroAutorizacion { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "El valor de la consulta debe ser positivo")]
    public decimal ValorConsulta { get; set; } = 80000;

    [Range(0, double.MaxValue, ErrorMessage = "El copago debe ser positivo")]
    public decimal CopagoOCuotaModeradora { get; set; } = 5000;

    public Guid RegistradoPorUsuarioId { get; set; }
}
