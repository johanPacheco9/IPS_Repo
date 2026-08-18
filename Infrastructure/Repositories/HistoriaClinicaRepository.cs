using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class HistoriaClinicaRepository : IHistoriaClinicaRepository
{
    private readonly MainDataContext _context;

    public HistoriaClinicaRepository(MainDataContext context)
    {
        _context = context;
    }

    public async Task<HistoriaClinica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.HistoriasClinicas
            .Include(h => h.Paciente)
            .Include(h => h.Profesional)
            .Include(h => h.Diagnosticos)
            .Include(h => h.Procedimientos)
            .Include(h => h.NotasAclaratorias)
                .ThenInclude(n => n.Profesional)
            .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
    }

    public async Task<HistoriaClinica?> GetByCitaIdAsync(Guid citaId, CancellationToken cancellationToken = default)
    {
        return await _context.HistoriasClinicas
            .Include(h => h.Paciente)
            .Include(h => h.Profesional)
            .Include(h => h.Diagnosticos)
            .Include(h => h.Procedimientos)
            .Include(h => h.NotasAclaratorias)
            .FirstOrDefaultAsync(h => h.CitaMedicaId == citaId, cancellationToken);
    }

    public async Task<IReadOnlyList<HistoriaClinica>> GetByPacienteIdAsync(Guid pacienteId, CancellationToken cancellationToken = default)
    {
        return await _context.HistoriasClinicas
            .Include(h => h.Profesional)
            .Include(h => h.Diagnosticos)
            .Where(h => h.PacienteId == pacienteId)
            .OrderByDescending(h => h.FechaApertura)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(HistoriaClinica historia, CancellationToken cancellationToken = default)
    {
        await _context.HistoriasClinicas.AddAsync(historia, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(HistoriaClinica historia, CancellationToken cancellationToken = default)
    {
        _context.HistoriasClinicas.Update(historia);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
