using System;

namespace HatsuneMiku.Data.MediatR.Notifications;

public class ProgressStartedEventArgs : EventArgs
{
    public DateTimeOffset StartedAt { get; }

    public ProgressStartedEventArgs(DateTimeOffset startedAt) =>
        StartedAt = startedAt;
}