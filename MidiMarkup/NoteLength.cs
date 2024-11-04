namespace MidiMarkup;
// Technically note "value"
// but a value of type "Note" has a pitch and a length
// so note value is somewhat ambigous

public readonly struct NoteLength {
    public Ratio<int> Value { get; }

    public NoteLength(Ratio<int> ratio) => Value = ratio;
    public NoteLength(int numerator, int denominator) : this(new(numerator, denominator)) { }

    public static readonly NoteLength Zero = new(0);

    public static readonly NoteLength Whole = new(1, 1);
    public static readonly NoteLength Half = new(1, 2);
    public static readonly NoteLength Quarter = new(1, 4);
    public static readonly NoteLength Eigth = new(1, 8);

    public static NoteLength Max(NoteLength a, NoteLength b) {
        return b.Value.CompareTo(a.Value) > 0 ? b : a;
    }

    public static NoteLength operator +(NoteLength l1, NoteLength l2) => new(l1.Value + l2.Value);

    public static double GetSecondsPerNote(TimingInfo timing) {
        var beatsPerNote = (double)timing.TimeSignature.BeatsPerNote;
        var beatsPerMinute = timing.Tempo.BeatsPerMinute;
        var beatsPerSecond = beatsPerMinute / 60;
        var secondsPerNote = beatsPerNote / beatsPerSecond;
        return secondsPerNote;
    }

    public RelativeTime GetDuration(TimingInfo timing) {
        var seconds = Value.ToDouble() * GetSecondsPerNote(timing);
        return new(seconds);
    }

    public override string ToString() => Value.ToString();
}
