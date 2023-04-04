using MediatR;

namespace HatsuneMiku.Data.MediatR.Notifications;

public static class ProgressStarted
{
    public sealed record class Notification(object Sender, ProgressStartedEventArgs EventArgs)
        : INotification, IEvent<object, ProgressStartedEventArgs>;
}