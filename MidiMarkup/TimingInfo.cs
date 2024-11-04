namespace MidiMarkup;

public sealed class TimeSignature {
    public int BeatsPerMeasure { get; }
    public int BeatsPerNote { get; }

    public TimeSignature(int upper, int lower) {
        BeatsPerMeasure = upper;
        BeatsPerNote = lower;
    }

    public static readonly TimeSignature TripleTime = new(3, 4);
    public static readonly TimeSignature CommonTime = new(4, 4);
}

public sealed class Tempo {
    public double BeatsPerMinute { get; }

    public Tempo(double beatsPerMinute) {
        BeatsPerMinute = beatsPerMinute;
    }
}

public sealed class TimingInfo {
    public required TimeSignature TimeSignature { get; init; }
    public required Tempo Tempo { get; init; }
}
