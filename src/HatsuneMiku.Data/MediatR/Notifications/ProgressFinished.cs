using MediatR;

namespace HatsuneMiku.Data.MediatR.Notifications;

public static class ProgressFinished
{
    public sealed record class Notification(object Sender, ProgressFinishedEventArgs EventArgs)
        : INotification, IEvent<object, ProgressFinishedEventArgs>;
}