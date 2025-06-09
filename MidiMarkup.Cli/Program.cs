namespace MidiMarkup.Cli;

using MidiMarkup.Midi;

using NAudio.Midi;

public sealed class Program(MidiOut device) {
    private static MidiOut PickDevice() {
        var deviceCount = MidiOut.NumberOfDevices;
        if (deviceCount == 0) {
            throw new Exception($"no midi outputs found");
        } else if (deviceCount > 1) {
            throw new Exception($"too many midi outputs found");
        } else {
            return new MidiOut(0);
        }
    }

    private void RunArrangement() {
        var MySong1_Final = new Arrangement.Melody([
                new Arrangement.Harmony([
                new Arrangement.Note(new Pitch(PitchClass.C, 4), NoteLength.Whole),
                new Arrangement.Note(new Pitch(PitchClass.E, 4), NoteLength.Whole),
                new Arrangement.Note(new Pitch(PitchClass.G, 4), NoteLength.Whole),
            ]),
            new Arrangement.Rest(NoteLength.Whole),
            new Arrangement.Harmony([
                new Arrangement.Note(new Pitch(PitchClass.C, 4), NoteLength.Whole),
                new Arrangement.Note(new Pitch(PitchClass.C + 3, 4), NoteLength.Whole),
                new Arrangement.Note(new Pitch(PitchClass.C + 7, 4), NoteLength.Whole)
            ]),
            new Arrangement.Rest(NoteLength.Whole),
            new Arrangement.Melody([
                new Arrangement.Note(new Pitch(PitchClass.C, 4), NoteLength.Quarter),
                new Arrangement.Note(new Pitch(PitchClass.E, 4), NoteLength.Quarter),
                new Arrangement.Note(new Pitch(PitchClass.G, 4), NoteLength.Quarter),
                new Arrangement.Rest(NoteLength.Quarter),
            ]),
            new Arrangement.Rest(NoteLength.Whole),
        ]);

        var now = AbsoluteTime.Now();
        var sequencePreDelay = new RelativeTime(0.5);
        Console.WriteLine($"arranging...");
        var (events, totalDuration) = Sequencer.Sequence(MySong1_Final, now + sequencePreDelay, new TimingInfo {
            TimeSignature = TimeSignature.CommonTime,
            Tempo = new Tempo(240),
        });

        var endTime = now + sequencePreDelay + totalDuration;

        Console.WriteLine($"playing...");
        foreach (var @event in events) {
            var targetTime = @event.Time.SecondsSinceAnchor;
            var actualTime = (now = AbsoluteTime.Now()).SecondsSinceAnchor;

            if (actualTime < targetTime) {
                var delta = targetTime - actualTime;
                Thread.Sleep((int)(delta * 1000));
            }
            device.Send(@event.GetNoteEvent().GetAsShortMessage());
        }

        Thread.Sleep((int)(1000 * Math.Max((endTime - AbsoluteTime.Now()).Seconds, 0)));
        Console.WriteLine($"done");
    }

    public static void Main(string[] _) {
        Console.OutputEncoding = Console.InputEncoding = Encoding.UTF8;
        try {
            using var device = PickDevice();
            new Program(device).RunArrangement();
        } catch (Exception e) {
            Console.Error.WriteLine($"\x1b[91m{e.Message}\x1b[39m");
        }
    }
}
