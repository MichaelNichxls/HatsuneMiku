using System;

// Move
namespace HatsuneMiku.Data.MediatR.Notifications;

// Necessary
public interface IEvent<TSender, TArgs>
    where TArgs : EventArgs
{
    TSender Sender { get; }
    TArgs EventArgs { get; }
}