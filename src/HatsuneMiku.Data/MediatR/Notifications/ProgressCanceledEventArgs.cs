using HatsuneMiku.Shared.Types;
using System;

namespace HatsuneMiku.Data.MediatR.Notifications;

public class ProgressCanceledEventArgs : EventArgs
{
    public ProgressContext? ProgressContext { get; }
    public DateTimeOffset CanceledAt { get; }

    public ProgressCanceledEventArgs(ProgressContext? progressContext, DateTimeOffset canceledAt)
    {
        ProgressContext = progressContext;
        CanceledAt      = canceledAt;
    }
}