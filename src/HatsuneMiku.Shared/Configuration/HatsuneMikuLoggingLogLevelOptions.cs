using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace HatsuneMiku.Shared.Configuration;

public sealed class HatsuneMikuLoggingLogLevelOptions
{
    [EnumDataType(typeof(LogLevel))]
    public required LogLevel Default { get; init; }
}