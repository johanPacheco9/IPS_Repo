using Domain.Common;
using Domain.DTOs;
using Domain.Enums;
using Domain.Requests;

namespace Domain.Interfaces;

public interface ICitaRepository
{
    Task<Result<CitaDto>> AgendarAsync(AgendarCitaRequest request, CancellationToken cancellationToken = default);
    Task<Result<CitaDto>> CambiarEstadoAsync(CambiarEstadoCitaRequest request, CancellationToken cancellationToken = default);
    Task<Result<CitaDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<CitaDto>>> GetByEstadoAsync(EstadoCita estado, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<CitaDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<DisponibilidadDto>> CrearDisponibilidadAsync(CrearDisponibilidadRequest request, CancellationToken cancellationToken = default);
}
