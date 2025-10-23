namespace Karel.Common;

/// <summary>
/// Extensions for System.Random to produce values of arbitrary numeric types.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Returns a random value of type T in the range [minValue, maxValue).
    /// T must be a numeric value type convertible via IConvertible.
    /// </summary>
    /// <typeparam name="T">Numeric value type (byte, short, int, long, float, double, decimal, etc.).</typeparam>
    /// <param name="rng">The Random instance.</param>
    /// <param name="minValue">Inclusive minimum value.</param>
    /// <param name="maxValue">Exclusive maximum value.</param>
    /// <returns>Random value of type T within the specified range.</returns>
    public static T Next<T>(this Random rng, T minValue, T maxValue)
        where T : struct, IConvertible
    {
        ArgumentNullException.ThrowIfNull(rng);

        // Convert endpoints to decimal for broad-range arithmetic
        decimal min = Convert.ToDecimal(minValue);
        decimal max = Convert.ToDecimal(maxValue);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(min, max, nameof(minValue));
       
        if (min == max) return minValue;

        // Random.NextDouble returns [0.0, 1.0), so result is in [min, max)
        decimal sample = (decimal)rng.NextDouble();
        decimal scaled = min + sample * (max - min);

        var type = typeof(T);
        var typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
                // For integer types, take the floor to produce an integral result in [min, max)
                decimal integral = Math.Floor(scaled);
                return (T)Convert.ChangeType(integral, type);

            case TypeCode.Single:
                return (T)Convert.ChangeType((float)scaled, type);

            case TypeCode.Double:
                return (T)Convert.ChangeType((double)scaled, type);

            case TypeCode.Decimal:
                return (T)Convert.ChangeType(scaled, type);

            default:
                throw new NotSupportedException($"Type '{type.FullName}' is not supported by RandomExtensions.Next<T>.");
        }
    }
}
