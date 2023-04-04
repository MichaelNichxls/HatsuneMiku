using Remora.Results;

namespace HatsuneMiku.Data.MediatR.Errors;

// nameof Entity
public sealed record class EntityNotFoundError(string Message = "The searched-for entity was not found in the database.")
    : NotFoundError(Message);