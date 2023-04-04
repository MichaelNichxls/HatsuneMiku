using HatsuneMiku.Shared.Types;
using System;

namespace HatsuneMiku.Data.MediatR.Notifications;

public class ProgressExceptionThrownEventArgs : EventArgs
{
    public ProgressContext? ProgressContext { get; }
    public Exception Exception { get; }
    public DateTimeOffset ExceptionThrownAt { get; }

    public ProgressExceptionThrownEventArgs(ProgressContext? progressContext, Exception exception, DateTimeOffset exceptionThrownAt)
    {
        ProgressContext     = progressContext;
        Exception           = exception;
        ExceptionThrownAt   = exceptionThrownAt;
    }
}