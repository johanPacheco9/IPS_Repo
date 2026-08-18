using Domain.Common;
using Domain.DTOs;

namespace Domain.Interfaces;

public interface IEpsRepository
{
    Task<Result<IReadOnlyList<EpsDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<EpsDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
