namespace MidiMarkup;

public abstract partial class Arrangement {
    private Arrangement() { }

    public abstract NoteLength Length { get; }

    public sealed partial class Note : Arrangement {
        public Pitch Pitch { get; }
        public override NoteLength Length { get; }

        public Note(Pitch pitch, NoteLength length) {
            Pitch = pitch;
            Length = length;
        }
    }

    public sealed partial class Rest : Arrangement {
        public override NoteLength Length { get; }

        public Rest(NoteLength length) => Length = length;
    }

    public sealed partial class Harmony : Arrangement {
        public ImmutableList<Arrangement> Parts { get; }
        public override NoteLength Length { get; }

        public Harmony(IEnumerable<Arrangement> parts) {
            Parts = ImmutableList.CreateRange(parts);
            Length = Parts.Aggregate(
                NoteLength.Zero,
                static (longestSoFar, part) => NoteLength.Max(longestSoFar, part.Length)
            );
        }
    }

    public sealed partial class Melody : Arrangement {
        public ImmutableList<Arrangement> Parts { get; }
        public override NoteLength Length { get; }

        public Melody(IEnumerable<Arrangement> parts) {
            Parts = ImmutableList.CreateRange(parts);
            Length = Parts.Aggregate(
                NoteLength.Zero,
                static (totalLength, part) => totalLength + part.Length
            );
        }
    }
}

// Really should fix a source generator to generate this nonsense
// Or https://github.com/dotnet/csharplang/issues/485 (`closed` hierarchies) gets implemented 
public abstract partial class Arrangement {
    public interface IVisitor {
        void Visit(Note arrangement);
        void Visit(Rest arrangement);
        void Visit(Harmony arrangement);
        void Visit(Melody arrangement);
    }

    public interface IVisitor<R> {
        R Visit(Note arrangement);
        R Visit(Rest arrangement);
        R Visit(Harmony arrangement);
        R Visit(Melody arrangement);
    }

    public interface IVisitor<T, R> {
        R Visit(Note arrangement, T context);
        R Visit(Rest arrangement, T context);
        R Visit(Harmony arrangement, T context);
        R Visit(Melody arrangement, T context);
    }

    public abstract void Accept(IVisitor visitor);
    public abstract R Accept<R>(IVisitor<R> visitor);
    public abstract R Accept<T, R>(IVisitor<T, R> visitor, T context);

    public sealed partial class Note : Arrangement {
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
        public override R Accept<R>(IVisitor<R> visitor) => visitor.Visit(this);
        public override R Accept<T, R>(IVisitor<T, R> visitor, T context) => visitor.Visit(this, context);
    }

    public sealed partial class Rest : Arrangement {
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
        public override R Accept<R>(IVisitor<R> visitor) => visitor.Visit(this);
        public override R Accept<T, R>(IVisitor<T, R> visitor, T context) => visitor.Visit(this, context);
    }

    public sealed partial class Harmony : Arrangement {
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
        public override R Accept<R>(IVisitor<R> visitor) => visitor.Visit(this);
        public override R Accept<T, R>(IVisitor<T, R> visitor, T context) => visitor.Visit(this, context);
    }

    public sealed partial class Melody : Arrangement {
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
        public override R Accept<R>(IVisitor<R> visitor) => visitor.Visit(this);
        public override R Accept<T, R>(IVisitor<T, R> visitor, T context) => visitor.Visit(this, context);
    }
}
