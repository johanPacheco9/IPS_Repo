using Domain.Common;
using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Requests;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CitaRepository : ICitaRepository
{
    private readonly MainDataContext _context;

    public CitaRepository(MainDataContext context)
    {
        _context = context;
    }

    public async Task<Result<CitaDto>> AgendarAsync(AgendarCitaRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var paciente = await _context.Pacientes.FindAsync(new object[] { request.PacienteId }, cancellationToken);
            if (paciente == null)
                return Result<CitaDto>.Failure("Paciente no encontrado.");

            var profesional = await _context.Profesionales.FindAsync(new object[] { request.ProfesionalId }, cancellationToken);
            if (profesional == null)
                return Result<CitaDto>.Failure("Profesional no encontrado.");

            var cita = new CitaMedica(
                request.PacienteId,
                request.ProfesionalId,
                request.FechaHora.ToUtcKind(),
                request.DuracionMinutos,
                request.MotivoConsulta,
                request.Observaciones
            );

            await _context.Citas.AddAsync(cita, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<CitaDto>.Success(MapToDto(cita, paciente, profesional));
        }
        catch (Exception ex)
        {
            return Result<CitaDto>.Failure($"Error al agendar cita: {ex.Message}");
        }
    }

    public async Task<Result<CitaDto>> CambiarEstadoAsync(CambiarEstadoCitaRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var cita = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Profesional)
                .FirstOrDefaultAsync(c => c.Id == request.CitaId, cancellationToken);

            if (cita == null)
                return Result<CitaDto>.Failure("Cita médica no encontrada.");

            switch (request.NuevoEstado)
            {
                case EstadoCita.EnSalaDeEspera:
                    cita.MarcarEnSalaDeEspera();
                    break;
                case EstadoCita.EnConsulta:
                    cita.IniciarConsulta();
                    break;
                case EstadoCita.Atendida:
                    cita.Atender();
                    break;
                case EstadoCita.Cancelada:
                    cita.Cancelar(request.Motivo);
                    break;
            }

            _context.Citas.Update(cita);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<CitaDto>.Success(MapToDto(cita, cita.Paciente, cita.Profesional));
        }
        catch (Exception ex)
        {
            return Result<CitaDto>.Failure($"Error al cambiar estado de la cita: {ex.Message}");
        }
    }

    public async Task<Result<CitaDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cita = await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Profesional)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (cita == null)
            return Result<CitaDto>.Failure("Cita no encontrada.");

        return Result<CitaDto>.Success(MapToDto(cita, cita.Paciente, cita.Profesional));
    }

    public async Task<Result<IReadOnlyList<CitaDto>>> GetByEstadoAsync(EstadoCita estado, CancellationToken cancellationToken = default)
    {
        var lista = await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Profesional)
            .Where(c => c.Estado == estado)
            .OrderBy(c => c.FechaHora)
            .ToListAsync(cancellationToken);

        var dtos = lista.Select(c => MapToDto(c, c.Paciente, c.Profesional)).ToList();
        return Result<IReadOnlyList<CitaDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<CitaDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var lista = await _context.Citas
            .Include(c => c.Paciente)
            .Include(c => c.Profesional)
            .OrderBy(c => c.FechaHora)
            .ToListAsync(cancellationToken);

        var dtos = lista.Select(c => MapToDto(c, c.Paciente, c.Profesional)).ToList();
        return Result<IReadOnlyList<CitaDto>>.Success(dtos);
    }

    public async Task<Result<DisponibilidadDto>> CrearDisponibilidadAsync(CrearDisponibilidadRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var profesional = await _context.Profesionales.FindAsync(new object[] { request.ProfesionalId }, cancellationToken);
            if (profesional == null)
                return Result<DisponibilidadDto>.Failure("Profesional no encontrado.");

            var disponibilidad = new DisponibilidadAgenda(
                request.ProfesionalId,
                request.Fecha.ToUtcKind(),
                request.HoraInicio,
                request.HoraFin,
                request.DuracionMinutosCita
            );

            await _context.Disponibilidades.AddAsync(disponibilidad, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = new DisponibilidadDto(
                disponibilidad.Id,
                disponibilidad.ProfesionalId,
                $"{profesional.PrimerNombre} {profesional.PrimerApellido}",
                disponibilidad.Fecha,
                disponibilidad.HoraInicio,
                disponibilidad.HoraFin,
                disponibilidad.DuracionMinutosCita
            );

            return Result<DisponibilidadDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<DisponibilidadDto>.Failure($"Error al crear disponibilidad: {ex.Message}");
        }
    }

    private static CitaDto MapToDto(CitaMedica c, Paciente? paciente, ProfesionalSalud? profesional)
    {
        var nombrePaciente = paciente != null ? $"{paciente.PrimerNombre} {paciente.PrimerApellido}" : "N/A";
        var docPaciente = paciente != null ? $"{paciente.TipoDocumento} {paciente.NumeroDocumento}" : "N/A";
        var nombreProf = profesional != null ? $"Dr(a). {profesional.PrimerNombre} {profesional.PrimerApellido}" : "N/A";

        return new CitaDto(
            c.Id,
            c.PacienteId,
            nombrePaciente,
            docPaciente,
            c.ProfesionalId,
            nombreProf,
            profesional?.Especialidad ?? "General",
            c.FechaHora,
            c.DuracionMinutos,
            c.Estado,
            c.MotivoConsulta,
            c.Observaciones
        );
    }
}
