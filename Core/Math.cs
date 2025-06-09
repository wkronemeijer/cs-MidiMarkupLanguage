namespace Core;

using System.Numerics;

public static class NumericExtension {
    /// <summary>
    /// Returns the modulus (and not the remainder <c>%</c>). 
    /// </summary>
    /// <returns>An integer in the range [0, <paramref name="n"/>)</returns>
    public static T Modulo<T>(this T i, T n) where T : IBinaryInteger<T> {
        return ((i % n) + n) % n;
    }

    public static T Gcd<T>(this T a, T b) where T : IBinaryInteger<T> {
        while (b != T.Zero) {
            var t = b;
            b = a.Modulo(b);
            a = t;
        }
        return a;
    }
}
