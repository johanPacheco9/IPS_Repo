using Domain.Enums;

namespace Domain.Entities;

public class HistoriaClinica
{
    public Guid Id { get; private set; }
    public Guid PacienteId { get; private set; }
    public Guid ProfesionalId { get; private set; }
    public Guid CitaMedicaId { get; private set; }
    public DateTime FechaApertura { get; private set; }
    public DateTime? FechaCierre { get; private set; }
    public bool EsInmutable { get; private set; }

    // Campos Anamnesis
    public string MotivoConsulta { get; private set; } = string.Empty;
    public string EnfermedadActual { get; private set; } = string.Empty;
    public string RevisionPorSistemas { get; private set; } = string.Empty;
    public string AntecedentesPersonales { get; private set; } = string.Empty;
    public string AntecedentesFamiliares { get; private set; } = string.Empty;
    public string ExamenFisico { get; private set; } = string.Empty;

    // Listas / Colecciones
    private readonly List<DiagnosticoConsulta> _diagnosticos = new();
    public IReadOnlyCollection<DiagnosticoConsulta> Diagnosticos => _diagnosticos.AsReadOnly();

    private readonly List<ProcedimientoConsulta> _procedimientos = new();
    public IReadOnlyCollection<ProcedimientoConsulta> Procedimientos => _procedimientos.AsReadOnly();

    private readonly List<NotaAclaratoria> _notasAclaratorias = new();
    public IReadOnlyCollection<NotaAclaratoria> NotasAclaratorias => _notasAclaratorias.AsReadOnly();

    // Navegación
    public Paciente? Paciente { get; private set; }
    public ProfesionalSalud? Profesional { get; private set; }
    public CitaMedica? CitaMedica { get; private set; }

    private HistoriaClinica() { }

    public HistoriaClinica(Guid pacienteId, Guid profesionalId, Guid citaMedicaId, string motivoConsulta)
    {
        if (pacienteId == Guid.Empty) throw new ArgumentException("El paciente es obligatorio.");
        if (profesionalId == Guid.Empty) throw new ArgumentException("El profesional es obligatorio.");
        if (citaMedicaId == Guid.Empty) throw new ArgumentException("La cita médica es obligatoria.");

        Id = Guid.NewGuid();
        PacienteId = pacienteId;
        ProfesionalId = profesionalId;
        CitaMedicaId = citaMedicaId;
        FechaApertura = DateTime.UtcNow;
        MotivoConsulta = motivoConsulta?.Trim() ?? string.Empty;
        EsInmutable = false;
    }

    public void ActualizarAnamnesis(
        string enfermedadActual,
        string revisionPorSistemas,
        string antecedentesPersonales,
        string antecedentesFamiliares,
        string examenFisico)
    {
        ValidarInmutabilidad();

        EnfermedadActual = enfermedadActual?.Trim() ?? string.Empty;
        RevisionPorSistemas = revisionPorSistemas?.Trim() ?? string.Empty;
        AntecedentesPersonales = antecedentesPersonales?.Trim() ?? string.Empty;
        AntecedentesFamiliares = antecedentesFamiliares?.Trim() ?? string.Empty;
        ExamenFisico = examenFisico?.Trim() ?? string.Empty;
    }

    public void AgregarDiagnosticoCIE11(string codigoCIE11, string descripcion, TipoDiagnostico tipoDiagnostico, bool esPrincipal)
    {
        ValidarInmutabilidad();

        if (string.IsNullOrWhiteSpace(codigoCIE11))
            throw new ArgumentException("El código CIE-11 es obligatorio.", nameof(codigoCIE11));

        if (esPrincipal && _diagnosticos.Any(d => d.EsPrincipal))
        {
            throw new InvalidOperationException("Ya existe un diagnóstico principal registrado en la consulta.");
        }

        _diagnosticos.Add(new DiagnosticoConsulta(Id, codigoCIE11, descripcion, tipoDiagnostico, esPrincipal));
    }

    public void AgregarProcedimientoCUPS(string codigoCUPS, string nombreProcedimiento, int cantidad = 1, string observaciones = "")
    {
        ValidarInmutabilidad();

        if (string.IsNullOrWhiteSpace(codigoCUPS))
            throw new ArgumentException("El código CUPS es obligatorio.", nameof(codigoCUPS));

        _procedimientos.Add(new ProcedimientoConsulta(Id, codigoCUPS, nombreProcedimiento, cantidad, observaciones));
    }

    /// <summary>
    /// Regla de Oro de Auditoría e Inmutabilidad (Colombia):
    /// Cierra la Historia Clínica bloqueándola permanentemente contra cualquier edición o eliminación.
    /// </summary>
    public void CerrarHistoriaClinica()
    {
        ValidarInmutabilidad();

        if (!_diagnosticos.Any(d => d.EsPrincipal))
        {
            throw new InvalidOperationException("Por ley en Colombia, debe existir al menos un diagnóstico principal CIE-11 registrado para cerrar la Historia Clínica.");
        }

        EsInmutable = true;
        FechaCierre = DateTime.UtcNow;
    }

    /// <summary>
    /// Correcciones posteriores al cierre de la historia solo se permiten mediante Notas Aclaratorias.
    /// </summary>
    public void AgregarNotaAclaratoria(Guid profesionalId, string contenido)
    {
        if (!EsInmutable)
        {
            throw new InvalidOperationException("Las notas aclaratorias solo aplican para Historias Clínicas cerradas e inmutables.");
        }

        if (string.IsNullOrWhiteSpace(contenido))
        {
            throw new ArgumentException("El contenido de la nota aclaratoria es obligatorio.", nameof(contenido));
        }

        _notasAclaratorias.Add(new NotaAclaratoria(Id, profesionalId, contenido));
    }

    private void ValidarInmutabilidad()
    {
        if (EsInmutable)
        {
            throw new InvalidOperationException("La Historia Clínica se encuentra CERRADA e INMUTABLE por ley colombiana. No se permiten modificaciones directas.");
        }
    }
}
