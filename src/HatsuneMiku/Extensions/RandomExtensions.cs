using System;

namespace HatsuneMiku.Extensions;

public static class RandomExtensions
{
    public static T GetItem<T>(this Random random, T[] choices) =>
        random.GetItems(choices, 1)[0];

    public static T GetItem<T>(this Random random, ReadOnlySpan<T> choices) =>
        random.GetItems(choices, 1)[0];
}