namespace Core;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;

public readonly struct Ratio<T> :
    IEquatable<Ratio<T>>,
    IComparable<Ratio<T>>
where T :
    IBinaryInteger<T>,
    ISignedNumber<T> {
    // `new Ratio<T>()` and `default(Ratio<T>)` are not the same
    // Invariant: if a == 0, b is ignored (i.e. allowed to be 0)

    [SuppressMessage("Style", "IDE0032:Use auto property",
        Justification = "a and b are classic rational variables")
    ]
    private readonly T a;
    private readonly T b;

    public readonly T Numerator => a;
    public readonly T Denonimator => a == T.Zero ? T.One : b;

    private static (T, T) Reduce(T num, T denom) {
        if (denom == T.Zero) {
            throw new DivideByZeroException();
        }
        if (T.IsNegative(num) && T.IsNegative(denom)) {
            num *= T.NegativeOne;
            denom *= T.NegativeOne;
        }
        var gcd = num.Gcd(denom);
        num /= gcd;
        denom /= gcd;
        return (num, denom);
    }

    public Ratio(T num, T denom) {
        (a, b) = Reduce(num, denom);
    }

    public void Deconstruct(out T num, out T denom) {
        num = Numerator;
        denom = Denonimator;
    }

    public static readonly Ratio<T> NegativeOne = T.NegativeOne;
    public static readonly Ratio<T> Zero = T.Zero;
    public static readonly Ratio<T> One = T.One;

    // Note: no loss of precision whatsoever
    public static implicit operator Ratio<T>(T value) => new(value, T.One);

    public Ratio<T> Reciprocal => new(b, a);

    public bool Equals(Ratio<T> other) {
        if (Numerator == T.Zero) {
            return other.Numerator == T.Zero;
        } else {
            return Denonimator == other.Denonimator;
        }
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        return obj is Ratio<T> other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Numerator, Denonimator);
    }

    public int CompareTo(Ratio<T> other) {
        var (a1, b1) = this;
        var (a2, b2) = other;
        return T.Sign(a1 * b2 - a2 * b1);
    }

    public static bool operator ==(Ratio<T> lhs, Ratio<T> rhs) {
        return lhs.Equals(rhs);
    }

    public static bool operator !=(Ratio<T> lhs, Ratio<T> rhs) {
        return !(lhs == rhs);
    }

    public static Ratio<T> operator +(Ratio<T> value) => value;

    public static Ratio<T> operator +(Ratio<T> lhs, Ratio<T> rhs) => new(
        lhs.a * rhs.b + rhs.a * lhs.b,
        lhs.b * rhs.b
    );

    public static Ratio<T> operator -(Ratio<T> value) => new(
        -value.a,
        value.b
    );

    public static Ratio<T> operator -(Ratio<T> lhs, Ratio<T> rhs) => new(
        lhs.a * rhs.b - rhs.a * lhs.b,
        lhs.b * rhs.b
    );

    public static Ratio<T> operator *(Ratio<T> lhs, Ratio<T> rhs) => new(
        lhs.a * rhs.a,
        lhs.b * rhs.b
    );

    public static Ratio<T> operator /(Ratio<T> lhs, Ratio<T> rhs) => new(
        lhs.a * rhs.b,
        lhs.b * rhs.a
    );

    public override string ToString() {
        if (a == T.Zero) {
            return "0";
        } else {
            return $"{a}/{b}";
        }
    }

    public double ToDouble() {
        return double.CreateChecked(a) / double.CreateChecked(b);
    }
}

public static class RatioSandbox {
    public static void Test() {
        var x0 = new Ratio<int>(3, 4);
        var x1 = new Ratio<long>(3, 4);
        var x2 = new Ratio<sbyte>(3, 4);
        var x3 = new Ratio<BigInteger>(3, 4);
    }
}
