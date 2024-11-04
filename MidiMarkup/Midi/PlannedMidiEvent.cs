namespace MidiMarkup.Midi;

using NAudio.Midi;

public enum PlannedMidiEventKind {
    NoteOn = 1,
    NoteOff = 2,
}

public sealed class PlannedMidiEvent() : IComparable<PlannedMidiEvent> {
    public required PlannedMidiEventKind Kind { get; init; }
    public required MidiChannel Channel { get; init; }
    public required MidiPitch Pitch { get; init; }
    public required MidiVelocity Velocity { get; init; }
    public required AbsoluteTime Time { get; init; }

    public NoteEvent GetNoteEvent() {
        // Time is ignored for real-time midi events
        var noteOnEvent = new NoteOnEvent(
            absoluteTime: 0L,
            channel: Channel.NAudioChannel,
            noteNumber: Pitch.NoteNumber,
            velocity: Velocity.Value,
            duration: 0
        );

        return Kind switch {
            PlannedMidiEventKind.NoteOn => noteOnEvent,
            PlannedMidiEventKind.NoteOff => noteOnEvent.OffEvent,
            _ => throw new($"invalid value for '{Kind}'"),
        };
    }

    private int CompareToNonNull(PlannedMidiEvent other) {
        return Math.Sign((Time - other.Time).Seconds);
    }

    public int CompareTo(PlannedMidiEvent? other) {
        // https://stackoverflow.com/questions/17025900/override-compareto-what-to-do-with-null-case
        return other is null ? 1 : CompareToNonNull(other);
    }

    public override string ToString() {
        return $"{Kind}({Channel} {Pitch} {Velocity} {Time})";
    }
}
