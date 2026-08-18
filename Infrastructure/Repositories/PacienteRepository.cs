using Domain.Common;
using Domain.DTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Requests;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly MainDataContext _context;

    public PacienteRepository(MainDataContext context)
    {
        _context = context;
    }

    public async Task<Result<PacienteDto>> CreateAsync(CreatePacienteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var existe = await _context.Pacientes
                .AnyAsync(p => p.TipoDocumento == request.TipoDocumento && p.NumeroDocumento == request.NumeroDocumento, cancellationToken);

            if (existe)
            {
                return Result<PacienteDto>.Failure($"Ya existe un paciente registrado con el documento {request.TipoDocumento} {request.NumeroDocumento}.");
            }

            var eps = await _context.Eps.FindAsync(new object[] { request.EpsId }, cancellationToken);

            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                TipoDocumento = request.TipoDocumento,
                NumeroDocumento = request.NumeroDocumento.Trim(),
                PrimerNombre = request.PrimerNombre.Trim(),
                SegundoNombre = request.SegundoNombre?.Trim(),
                PrimerApellido = request.PrimerApellido.Trim(),
                SegundoApellido = request.SegundoApellido?.Trim(),
                FechaNacimiento = request.FechaNacimiento.ToUtcKind(),
                Genero = request.Genero,
                EpsId = request.EpsId,
                RegimenSalud = request.RegimenSalud,
                Telefono = request.Telefono?.Trim() ?? string.Empty,
                Email = request.Email?.Trim() ?? string.Empty,
                Estado = EstadoPaciente.Activo,
                FechaRegistro = DateTime.UtcNow
            };

            await _context.Pacientes.AddAsync(paciente, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = MapToDto(paciente, eps?.Nombre ?? "Particular");
            return Result<PacienteDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<PacienteDto>.Failure($"Error al guardar el paciente: {ex.Message}");
        }
    }

    public async Task<Result<PacienteDto>> UpdateAsync(UpdatePacienteRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var paciente = await _context.Pacientes.Include(p => p.EPS).FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (paciente == null)
            {
                return Result<PacienteDto>.Failure("Paciente no encontrado.");
            }

            paciente.TipoDocumento = request.TipoDocumento;
            paciente.NumeroDocumento = request.NumeroDocumento.Trim();
            paciente.PrimerNombre = request.PrimerNombre.Trim();
            paciente.SegundoNombre = request.SegundoNombre?.Trim();
            paciente.PrimerApellido = request.PrimerApellido.Trim();
            paciente.SegundoApellido = request.SegundoApellido?.Trim();
            paciente.FechaNacimiento = request.FechaNacimiento.ToUtcKind();
            paciente.Genero = request.Genero;
            paciente.EpsId = request.EpsId;
            paciente.RegimenSalud = request.RegimenSalud;
            paciente.Telefono = request.Telefono?.Trim() ?? string.Empty;
            paciente.Email = request.Email?.Trim() ?? string.Empty;
            paciente.Estado = request.Estado;

            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = MapToDto(paciente, paciente.EPS?.Nombre ?? "Particular");
            return Result<PacienteDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Result<PacienteDto>.Failure($"Error al actualizar el paciente: {ex.Message}");
        }
    }

    public async Task<Result<PacienteDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var paciente = await _context.Pacientes
            .Include(p => p.EPS)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        if (paciente == null)
            return Result<PacienteDto>.Failure("Paciente no encontrado.");

        return Result<PacienteDto>.Success(MapToDto(paciente, paciente.EPS?.Nombre ?? "Particular"));
    }

    public async Task<Result<PacienteDto>> GetByDocumentoAsync(TipoDocumento tipoDocumento, string numeroDocumento, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroDocumento))
            return Result<PacienteDto>.Failure("El número de documento es requerido.");

        var doc = numeroDocumento.Trim();
        var paciente = await _context.Pacientes
            .Include(p => p.EPS)
            .FirstOrDefaultAsync(p => p.TipoDocumento == tipoDocumento && p.NumeroDocumento == doc, cancellationToken);

        if (paciente == null)
            return Result<PacienteDto>.Failure($"No se encontró ningún paciente registrado con {tipoDocumento} {doc}.");

        return Result<PacienteDto>.Success(MapToDto(paciente, paciente.EPS?.Nombre ?? "Particular"));
    }

    public async Task<Result<IReadOnlyList<PacienteDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var lista = await _context.Pacientes
            .Include(p => p.EPS)
            .OrderBy(p => p.PrimerApellido)
            .ThenBy(p => p.PrimerNombre)
            .ToListAsync(cancellationToken);

        var dtos = lista.Select(p => MapToDto(p, p.EPS?.Nombre ?? "Particular")).ToList();
        return Result<IReadOnlyList<PacienteDto>>.Success(dtos);
    }

    public async Task<Result<IReadOnlyList<PacienteDto>>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllAsync(cancellationToken);

        var q = query.Trim().ToLower();
        var lista = await _context.Pacientes
            .Include(p => p.EPS)
            .Where(p => p.NumeroDocumento.ToLower().Contains(q) 
                     || p.PrimerNombre.ToLower().Contains(q) 
                     || p.PrimerApellido.ToLower().Contains(q))
            .OrderBy(p => p.PrimerApellido)
            .ToListAsync(cancellationToken);

        var dtos = lista.Select(p => MapToDto(p, p.EPS?.Nombre ?? "Particular")).ToList();
        return Result<IReadOnlyList<PacienteDto>>.Success(dtos);
    }

    private static PacienteDto MapToDto(Paciente p, string epsNombre)
    {
        var nombreCompleto = $"{p.PrimerNombre} {p.SegundoNombre} {p.PrimerApellido} {p.SegundoApellido}".Replace("  ", " ").Trim();
        var hoy = DateTime.UtcNow;
        var edad = hoy.Year - p.FechaNacimiento.Year;
        if (p.FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;

        return new PacienteDto(
            p.Id,
            p.TipoDocumento,
            p.NumeroDocumento,
            p.PrimerNombre,
            p.SegundoNombre,
            p.PrimerApellido,
            p.SegundoApellido,
            nombreCompleto,
            p.FechaNacimiento,
            edad,
            p.Genero,
            p.EpsId,
            epsNombre,
            p.RegimenSalud,
            p.Telefono,
            p.Email,
            p.Estado
        );
    }
}
