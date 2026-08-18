using Domain.Common;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Requests;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdmisionRepository : IAdmisionRepository
{
    private readonly MainDataContext _context;

    public AdmisionRepository(MainDataContext context)
    {
        _context = context;
    }

    public async Task<Result<AdmisionDto>> RegistrarAsync(RegistrarAdmisionRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var cita = await _context.Citas.Include(c => c.Paciente).FirstOrDefaultAsync(c => c.Id == request.CitaMedicaId, cancellationToken);
            if (cita == null)
                return Result<AdmisionDto>.Failure("La cita médica seleccionada no existe.");

            var admision = new Admision(
                request.CitaMedicaId,
                cita.PacienteId,
                request.TipoContrato,
                request.EPS,
                request.NumeroAutorizacion,
                request.ValorConsulta,
                request.CopagoOCuotaModeradora,
                request.RegistradoPorUsuarioId
            );

            await _context.Admisiones.AddAsync(admision, cancellationToken);

            if (cita.Estado == Domain.Enums.EstadoCita.Programada)
            {
                cita.MarcarEnSalaDeEspera();
                _context.Citas.Update(cita);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Result<AdmisionDto>.Success(MapToDto(admision, cita.Paciente));
        }
        catch (Exception ex)
        {
            return Result<AdmisionDto>.Failure($"Error al registrar admisión: {ex.Message}");
        }
    }

    public async Task<Result<AdmisionDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var admision = await _context.Admisiones
            .Include(a => a.Paciente)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (admision == null)
            return Result<AdmisionDto>.Failure("Admisión no encontrada.");

        return Result<AdmisionDto>.Success(MapToDto(admision, admision.Paciente));
    }

    public async Task<Result<IReadOnlyList<AdmisionDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var lista = await _context.Admisiones
            .Include(a => a.Paciente)
            .OrderByDescending(a => a.FechaAdmision)
            .ToListAsync(cancellationToken);

        var dtos = lista.Select(a => MapToDto(a, a.Paciente)).ToList();
        return Result<IReadOnlyList<AdmisionDto>>.Success(dtos);
    }

    private static AdmisionDto MapToDto(Admision a, Paciente? paciente)
    {
        var nombrePaciente = paciente != null ? $"{paciente.PrimerNombre} {paciente.PrimerApellido}" : "N/A";
        var docPaciente = paciente != null ? $"{paciente.TipoDocumento} {paciente.NumeroDocumento}" : "N/A";

        return new AdmisionDto(
            a.Id,
            a.CitaMedicaId,
            a.PacienteId,
            nombrePaciente,
            docPaciente,
            a.TipoContrato,
            a.EPS,
            a.NumeroAutorizacion,
            a.ValorConsulta,
            a.CopagoOCuotaModeradora,
            a.ValorNeto,
            a.FechaAdmision
        );
    }
}
