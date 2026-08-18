using Domain.Entities;

namespace Domain.Interfaces;

public interface IHistoriaClinicaRepository
{
    Task<HistoriaClinica?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<HistoriaClinica?> GetByCitaIdAsync(Guid citaId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<HistoriaClinica>> GetByPacienteIdAsync(Guid pacienteId, CancellationToken cancellationToken = default);
    Task AddAsync(HistoriaClinica historia, CancellationToken cancellationToken = default);
    Task UpdateAsync(HistoriaClinica historia, CancellationToken cancellationToken = default);
}
