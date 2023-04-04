using MediatR;

namespace HatsuneMiku.Data.MediatR.Notifications;

public static class ProgressExceptionThrown
{
    public sealed record class Notification(object Sender, ProgressExceptionThrownEventArgs EventArgs)
        : INotification, IEvent<object, ProgressExceptionThrownEventArgs>;
}