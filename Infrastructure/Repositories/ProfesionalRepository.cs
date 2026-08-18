using Domain.Common;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Requests;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProfesionalRepository : IProfesionalRepository
{
    private readonly MainDataContext _context;

    public ProfesionalRepository(MainDataContext context)
    {
        _context = context;
    }

    public async Task<Result<ProfesionalDto>> CreateAsync(CreateProfesionalRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existe = await _context.Profesionales.AnyAsync(p => p.NumeroRethus == request.NumeroRethus, cancellationToken);
            if (existe)
            {
                return Result<ProfesionalDto>.Failure($"Ya existe un profesional registrado con el número RETHUS {request.NumeroRethus}.");
            }

            var profesional = new ProfesionalSalud
            {
                Id = Guid.NewGuid(),
                TipoDocumento = request.TipoDocumento,
                NumeroDocumento = request.NumeroDocumento.Trim(),
                PrimerNombre = request.PrimerNombre.Trim(),
                SegundoNombre = request.SegundoNombre?.Trim(),
                PrimerApellido = request.PrimerApellido.Trim(),
                SegundoApellido = request.SegundoApellido?.Trim(),
                NumeroRethus = request.NumeroRethus.Trim(),
                Especialidad = request.Especialidad.Trim(),
                Telefono = request.Telefono?.Trim() ?? string.Empty,
                Email = request.Email?.Trim() ?? string.Empty,
                Activo = true,
                FechaRegistro = DateTime.UtcNow
            };

            await _context.Profesionales.AddAsync(profesional, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<ProfesionalDto>.Success(MapToDto(profesional));
        }
        catch (Exception ex)
        {
            return Result<ProfesionalDto>.Failure($"Error al registrar el profesional: {ex.Message}");
        }
    }

    public async Task<Result<ProfesionalDto>> UpdateAsync(UpdateProfesionalRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var profesional = await _context.Profesionales.FindAsync(new object[] { request.Id }, cancellationToken);
            if (profesional == null)
            {
                return Result<ProfesionalDto>.Failure("Profesional no encontrado.");
            }

            profesional.TipoDocumento = request.TipoDocumento;
            profesional.NumeroDocumento = request.NumeroDocumento.Trim();
            profesional.PrimerNombre = request.PrimerNombre.Trim();
            profesional.SegundoNombre = request.SegundoNombre?.Trim();
            profesional.PrimerApellido = request.PrimerApellido.Trim();
            profesional.SegundoApellido = request.SegundoApellido?.Trim();
            profesional.NumeroRethus = request.NumeroRethus.Trim();
            profesional.Especialidad = request.Especialidad.Trim();
            profesional.Telefono = request.Telefono?.Trim() ?? string.Empty;
            profesional.Email = request.Email?.Trim() ?? string.Empty;
            profesional.Activo = request.Activo;

            _context.Profesionales.Update(profesional);
            await _context.SaveChangesAsync(cancellationToken);

            return Result<ProfesionalDto>.Success(MapToDto(profesional));
        }
        catch (Exception ex)
        {
            return Result<ProfesionalDto>.Failure($"Error al actualizar el profesional: {ex.Message}");
        }
    }

    public async Task<Result<ProfesionalDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var profesional = await _context.Profesionales.FindAsync(new object[] { id }, cancellationToken);
        if (profesional == null)
            return Result<ProfesionalDto>.Failure("Profesional no encontrado.");

        return Result<ProfesionalDto>.Success(MapToDto(profesional));
    }

    public async Task<Result<IReadOnlyList<ProfesionalDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var lista = await _context.Profesionales
            .OrderBy(p => p.PrimerApellido)
            .ThenBy(p => p.PrimerNombre)
            .ToListAsync(cancellationToken);

        var dtos = lista.Select(MapToDto).ToList();
        return Result<IReadOnlyList<ProfesionalDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<ProfesionalDto>>> GetActivosAsync(CancellationToken cancellationToken = default)
    {
        var lista = await _context.Profesionales
            .Where(p => p.Activo)
            .OrderBy(p => p.PrimerApellido)
            .ThenBy(p => p.PrimerNombre)
            .ToListAsync(cancellationToken);

        var dtos = lista.Select(MapToDto).ToList();
        return Result<IReadOnlyList<ProfesionalDto>>.Success(dtos);
    }

    private static ProfesionalDto MapToDto(ProfesionalSalud p)
    {
        var nombreCompleto = $"Dr(a). {p.PrimerNombre} {p.SegundoNombre} {p.PrimerApellido} {p.SegundoApellido}".Replace("  ", " ").Trim();
        return new ProfesionalDto(
            p.Id,
            p.TipoDocumento,
            p.NumeroDocumento,
            p.PrimerNombre,
            p.SegundoNombre,
            p.PrimerApellido,
            p.SegundoApellido,
            nombreCompleto,
            p.NumeroRethus,
            p.Especialidad,
            p.Telefono,
            p.Email,
            p.Activo
        );
    }
}
