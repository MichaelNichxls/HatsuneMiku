using MediatR;

namespace HatsuneMiku.Data.MediatR.Notifications;

public static class ProgressCanceled
{
    public sealed record class Notification(object Sender, ProgressCanceledEventArgs EventArgs)
        : INotification, IEvent<object, ProgressCanceledEventArgs>;
}