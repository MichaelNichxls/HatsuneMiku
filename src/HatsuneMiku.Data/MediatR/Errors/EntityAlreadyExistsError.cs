using Remora.Results;

namespace HatsuneMiku.Data.MediatR.Errors;

// nameof Entity
// Change message
public sealed record class EntityAlreadyExistsError(string Message = "The entity already exists in the database.")
    : ResultError(Message);