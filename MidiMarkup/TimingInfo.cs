namespace MidiMarkup;

public sealed class TimeSignature {
    public int BeatsPerMeasure { get; }
    public int BeatsPerNote { get; }

    public TimeSignature(int top, int bottom) {
        Requires(top is > 0, $"{nameof(top)} must positive");
        Requires(bottom is > 0, $"{nameof(bottom)} must positive");
        BeatsPerMeasure = top;
        BeatsPerNote = bottom;
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
