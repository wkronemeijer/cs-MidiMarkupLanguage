namespace MidiMarkup.Midi;

public sealed class Sequencer : Arrangement.IVisitor<AbsoluteTime, RelativeTime> {
    private readonly List<PlannedMidiEvent> events = [];
    private readonly TimingInfo timing;

    private Sequencer(TimingInfo timing) => this.timing = timing;

    public RelativeTime Visit(Arrangement.Note note, AbsoluteTime now) {
        var channel = MidiChannel.Default; // WIP: Allow different channels
        var pitch = note.Pitch.ToMidiPitch();
        var velocity = MidiVelocity.Default;
        var duration = note.Length.GetDuration(timing);

        events.Add(new() {
            Kind = PlannedMidiEventKind.NoteOn,
            Channel = channel,
            Pitch = pitch,
            Velocity = velocity,
            Time = now,
        });
        events.Add(new() {
            Kind = PlannedMidiEventKind.NoteOff,
            Channel = channel,
            Pitch = pitch,
            Velocity = velocity,
            Time = now + duration,
        });
        return duration;
    }

    public RelativeTime Visit(Arrangement.Rest rest, AbsoluteTime now) {
        return rest.Length.GetDuration(timing);
    }

    public RelativeTime Visit(Arrangement.Harmony harmony, AbsoluteTime now) {
        var overallDuration = harmony.Length.GetDuration(timing);
        foreach (var part in harmony.Parts) {
            // Harmony is simultaneous
            part.Accept(this, now);
        }
        return overallDuration;
    }

    public RelativeTime Visit(Arrangement.Melody melody, AbsoluteTime now) {
        var totalDuration = melody.Length.GetDuration(timing);
        foreach (var part in melody.Parts) {
            // Melody is sequential
            now += part.Accept(this, now);
        }
        return totalDuration;
    }

    public static (List<PlannedMidiEvent> Events, RelativeTime TotalDuration) Sequence(
        Arrangement root,
        AbsoluteTime start,
        TimingInfo timing
    ) {
        var sequencer = new Sequencer(timing);
        var totalDuration = root.Accept(sequencer, start);
        var events = sequencer.events;
        events.Sort();
        return (events, totalDuration);
    }
}
