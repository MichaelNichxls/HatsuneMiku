using System;

namespace HatsuneMiku.Data.MediatR.Notifications;

public class ProgressFinishedEventArgs : EventArgs
{
    public DateTimeOffset FinishedAt { get; }

    public ProgressFinishedEventArgs(DateTimeOffset finishedAt) =>
        FinishedAt = finishedAt;
}