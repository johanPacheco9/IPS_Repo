using Domain.Common;
using Domain.DTOs;
using Domain.Requests;

namespace Domain.Interfaces;

public interface IAdmisionRepository
{
    Task<Result<AdmisionDto>> RegistrarAsync(RegistrarAdmisionRequest request, CancellationToken cancellationToken = default);
    Task<Result<AdmisionDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<AdmisionDto>>> GetAllAsync(CancellationToken cancellationToken = default);
}
