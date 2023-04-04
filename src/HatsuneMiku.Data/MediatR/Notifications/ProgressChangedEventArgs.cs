using HatsuneMiku.Shared.Types;
using System;

namespace HatsuneMiku.Data.MediatR.Notifications;

public class ProgressChangedEventArgs : EventArgs
{
    public ProgressContext ProgressContext { get; }

    public ProgressChangedEventArgs(ProgressContext progressContext) =>
        ProgressContext = progressContext;
}