using Domain.Common;
using Domain.DTOs;
using Domain.Enums;
using Domain.Requests;

namespace Domain.Interfaces;

public interface IPacienteRepository
{
    Task<Result<PacienteDto>> CreateAsync(CreatePacienteRequest request, CancellationToken cancellationToken = default);
    Task<Result<PacienteDto>> UpdateAsync(UpdatePacienteRequest request, CancellationToken cancellationToken = default);
    Task<Result<PacienteDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<PacienteDto>> GetByDocumentoAsync(TipoDocumento tipoDocumento, string numeroDocumento, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<PacienteDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<PacienteDto>>> SearchAsync(string query, CancellationToken cancellationToken = default);
}
