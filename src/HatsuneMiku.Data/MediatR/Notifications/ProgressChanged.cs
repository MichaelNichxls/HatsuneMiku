using MediatR;

namespace HatsuneMiku.Data.MediatR.Notifications;

public static class ProgressChanged
{
    // Nullable
    public sealed record class Notification(object Sender, ProgressChangedEventArgs EventArgs)
        : INotification, IEvent<object, ProgressChangedEventArgs>;
}