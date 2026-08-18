using Domain.Common;
using Domain.DTOs;
using Domain.Requests;

namespace Domain.Interfaces;

public interface IProfesionalRepository
{
    Task<Result<ProfesionalDto>> CreateAsync(CreateProfesionalRequest request, CancellationToken cancellationToken = default);
    Task<Result<ProfesionalDto>> UpdateAsync(UpdateProfesionalRequest request, CancellationToken cancellationToken = default);
    Task<Result<ProfesionalDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProfesionalDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ProfesionalDto>>> GetActivosAsync(CancellationToken cancellationToken = default);
}
