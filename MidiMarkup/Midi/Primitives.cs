namespace MidiMarkup.Midi;

public readonly struct MidiChannel {
    private const int MIN_CHANNEL = 0;
    private const int MAX_CHANNEL = 0x10 - 1;

    /// <summary>
    /// This is a 0-based channel index, NAudio uses 1-based channel indices.
    /// </summary>
    public int Value { get; }

    public int NAudioChannel => Value + 1;

    public MidiChannel(int channel) {
        Requires(channel is >= MIN_CHANNEL and <= MAX_CHANNEL);
        Value = channel;
    }

    public MidiChannel() : this(0) { }

    public static readonly MidiChannel Default = new();

    public override string ToString() => $"MidiChannel({Value})";
}

public readonly struct MidiPitch {
    private const int MIN_PITCH = 0;
    private const int MAX_PITCH = 0x80 - 1;

    public int NoteNumber { get; }

    public MidiPitch(int note) {
        Requires(note is >= MIN_PITCH and <= MAX_PITCH);
        NoteNumber = note;
    }

    private const int C4_PITCH = 60;
    private const int A4_PITCH = 69;

    public MidiPitch() : this(C4_PITCH) { }

    public static readonly MidiPitch C4 = new(C4_PITCH);
    public static readonly MidiPitch A4 = new(A4_PITCH);

    public static int ToMidiNoteNumber(PitchClass @class, int octave) {
        // convert(C4 ) == 60
        // convert(C3 ) == 48
        // convert(C2 ) == 36
        // convert(C1 ) == 24
        // convert(C0 ) == 12
        // convert(C-1) ==  0
        return 12 * (octave + 1) + @class.Offset;
    }

    public MidiPitch(PitchClass @class, int octave) : this(
        ToMidiNoteNumber(@class, octave)
    ) { }

    public override string ToString() => $"MidiPitch({NoteNumber})";
}

public readonly struct MidiVelocity {
    private const int MIN_VELOCITY = 0;
    private const int MAX_VELOCITY = 0x80 - 1;

    public int Value { get; }

    public MidiVelocity(int velocity) {
        Requires(velocity is >= MIN_VELOCITY and <= MAX_VELOCITY);
        Value = velocity;
    }

    public MidiVelocity() : this(64) { }

    public static readonly MidiVelocity Default = new();

    public override string ToString() => $"MidiVelocity({Value})";
}

public static class ToMidiPrimitiveConversion {
    public static MidiPitch ToMidiPitch(this Pitch self) {
        var (@class, octave) = self;
        return new(@class, octave);
    }
}
