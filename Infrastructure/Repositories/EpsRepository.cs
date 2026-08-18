using Domain.Common;
using Domain.DTOs;
using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EpsRepository : IEpsRepository
{
    private readonly MainDataContext _context;

    public EpsRepository(MainDataContext context)
    {
        _context = context;
    }

    public async Task<Result<IReadOnlyList<EpsDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var lista = await _context.Eps
            .Where(e => e.Activo)
            .OrderBy(e => e.Nombre)
            .Select(e => new EpsDto(e.Id, e.Codigo, e.Nombre, e.Nit, e.Activo))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<EpsDto>>.Success(lista);
    }

    public async Task<Result<EpsDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var eps = await _context.Eps.FindAsync(new object[] { id }, cancellationToken);
        if (eps == null)
            return Result<EpsDto>.Failure("EPS no encontrada.");

        return Result<EpsDto>.Success(new EpsDto(eps.Id, eps.Codigo, eps.Nombre, eps.Nit, eps.Activo));
    }
}
