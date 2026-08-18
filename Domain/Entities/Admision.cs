using Domain.Enums;

namespace Domain.Entities;

public class Admision
{
    public Guid Id { get; private set; }
    public Guid CitaMedicaId { get; private set; }
    public Guid PacienteId { get; private set; }
    public TipoContrato TipoContrato { get; private set; }
    public string EPS { get; private set; } = string.Empty;
    public string NumeroAutorizacion { get; private set; } = string.Empty;
    public decimal ValorConsulta { get; private set; }
    public decimal CopagoOCuotaModeradora { get; private set; }
    public decimal ValorNeto { get; private set; }
    public DateTime FechaAdmision { get; private set; }
    public Guid RegistradoPorUsuarioId { get; private set; }

    // Navegación
    public CitaMedica? CitaMedica { get; private set; }
    public Paciente? Paciente { get; private set; }

    private Admision() { }

    public Admision(
        Guid citaMedicaId,
        Guid pacienteId,
        TipoContrato tipoContrato,
        string eps,
        string numeroAutorizacion,
        decimal valorConsulta,
        decimal copagoOCuotaModeradora,
        Guid registradoPorUsuarioId)
    {
        if (citaMedicaId == Guid.Empty) throw new ArgumentException("La cita médica es obligatoria.", nameof(citaMedicaId));
        if (pacienteId == Guid.Empty) throw new ArgumentException("El paciente es obligatorio.", nameof(pacienteId));

        if (tipoContrato != TipoContrato.Particular && string.IsNullOrWhiteSpace(numeroAutorizacion))
        {
            throw new ArgumentException("Para convenios EPS o Prepagada es obligatorio ingresar el Número de Autorización expedido por la entidad.", nameof(numeroAutorizacion));
        }

        Id = Guid.NewGuid();
        CitaMedicaId = citaMedicaId;
        PacienteId = pacienteId;
        TipoContrato = tipoContrato;
        EPS = eps?.Trim() ?? string.Empty;
        NumeroAutorizacion = tipoContrato == TipoContrato.Particular ? string.Empty : numeroAutorizacion.Trim();
        ValorConsulta = valorConsulta >= 0 ? valorConsulta : 0;
        CopagoOCuotaModeradora = copagoOCuotaModeradora >= 0 ? copagoOCuotaModeradora : 0;

        // Regla de Negocio: En atención Particular, se asume 100% cobro al paciente
        if (TipoContrato == TipoContrato.Particular)
        {
            CopagoOCuotaModeradora = ValorConsulta;
            ValorNeto = 0; // No hay cobro a entidad aseguradora
        }
        else
        {
            ValorNeto = Math.Max(0, ValorConsulta - CopagoOCuotaModeradora);
        }

        FechaAdmision = DateTime.UtcNow;
        RegistradoPorUsuarioId = registradoPorUsuarioId;
    }
}
