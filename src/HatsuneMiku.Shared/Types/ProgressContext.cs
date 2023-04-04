using System;

// Move
namespace HatsuneMiku.Shared.Types;

// struct?
// [DebuggerDisplay]
public class ProgressContext
{
    private int _value;
    private int _minimum;
    private int _maximum;

    public int Value
    {
        get => _value;
        set => _value = Math.Clamp(value, Minimum, Maximum);
    }

    // Rename maybe
    public Percent Percentage => (Value - Minimum) / (double)(Maximum - Minimum);

    // arg name param
    public int Minimum
    {
        get => _minimum;
        set => _minimum = value is < 0 || value > Maximum
            ? throw new ArgumentOutOfRangeException(message: $"Value must be less than {nameof(ProgressContext)}.{nameof(Maximum)}, and greater than 0.", null)
            : value;
    }

    public int Maximum
    {
        get => _maximum;
        set => _maximum = value is < 0 || value < Minimum
            ? throw new ArgumentOutOfRangeException(message: $"Value must be greater than {nameof(ProgressContext)}.{nameof(Minimum)}, and greater than 0.", null)
            : value;
    }

    public int Step { get; set; }

    public ProgressContext(int value = 0, int minimum = 0, int maximum = 100, int step = 1)
    {
        Value   = value;
        Minimum = minimum;
        Maximum = maximum;
        Step    = step;
    }

    // etc.
    public override string ToString() =>
        Percentage.ToString();

    public void Increment(int value) =>
        Value += value;

    public void PerformStep() =>
        Value += Step;
    
    public static ProgressContext operator +(ProgressContext left, int right)
    {
        left.Increment(right);
        return left;
    }

    public static ProgressContext operator ++(ProgressContext value)
    {
        value.PerformStep();
        return value;
    }

    // other operators
}