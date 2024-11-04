namespace MidiMarkup;

using static PitchClass;

public readonly struct PitchClass(int offset) {
    public const int SEMITONE_COUNT = 12;
    /// <summary>
    ///     <para>
    ///         Positive number of semitones between this pitch class and C.
    ///     </para>
    ///     <para>
    ///         Always in 0 ..&lt; <see cref="SEMITONE_COUNT"/>
    ///     </para>
    /// </summary>
    public int Offset { get; } = offset.Modulo(SEMITONE_COUNT);

    public static readonly PitchClass C = new(0);
    public static readonly PitchClass D = new(2);
    public static readonly PitchClass E = new(4);
    public static readonly PitchClass F = new(5);
    public static readonly PitchClass G = new(7);
    public static readonly PitchClass A = new(9);
    public static readonly PitchClass B = new(11);

    public static explicit operator int(PitchClass pc) => pc.Offset;

    public override string ToString() => Offset switch {
        0 => "C",
        1 => "C#",
        2 => "D",
        3 => "D#",
        4 => "E",
        5 => "F",
        6 => "F#",
        7 => "G",
        8 => "G#",
        9 => "A",
        10 => "A#",
        11 => "B",
        _ => throw new Exception("unreachable")
    };

    public static PitchClass operator +(PitchClass @class, Interval i) => new(@class.Offset + i.Value);
}

public readonly struct Pitch {
    private readonly int value;

    private Pitch(int value) => this.value = value;

    //////////////////////
    // Conversion logic //
    //////////////////////

    private static int Combine(PitchClass pc, int octave) {
        return pc.Offset + SEMITONE_COUNT * octave;
    }

    public PitchClass PitchClass => new(value.Modulo(SEMITONE_COUNT));
    public int Octave => value / SEMITONE_COUNT;

    //////////////////////////
    // Constructing pitches //
    //////////////////////////

    public Pitch(PitchClass pc, int octave) : this(Combine(pc, octave)) { }

    public void Deconstruct(out PitchClass @class, out int octave) {
        @class = PitchClass;
        octave = Octave;
    }

    public static readonly Pitch C4 = new(C, 4);
    public static readonly Pitch A4 = new(A, 4);

    public static Pitch operator +(Pitch pitch, Interval i) => new(pitch.value + i.Value);
    public static Pitch operator -(Pitch pitch, Interval i) => new(pitch.value - i.Value);

    public static Interval operator -(Pitch pitch1, Pitch pitch2) => new(pitch1.value - pitch2.value);

    public override string ToString() {
        var (@class, octave) = this;
        return $"{@class}{octave}";
    }
}

public readonly struct Interval {
    public readonly int Value { get; }

    public Interval(int value) => Value = value;

    public static Interval operator +(Interval i1, Interval i2) => new(i1.Value + i2.Value);
    public static Interval operator -(Interval i1, Interval i2) => new(i1.Value - i2.Value);

    public static implicit operator Interval(int value) => new(value);

    public override string ToString() => Value switch {
        > 0 => $"+{Value}",
        _ => $"{Value}",
    };
}
