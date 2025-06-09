namespace MidiMarkup;

public readonly struct AbsoluteTime {
    public static readonly DateTime Anchor = new(2020, 1, 1);

    public double SecondsSinceAnchor { get; }

    private AbsoluteTime(double seconds) => SecondsSinceAnchor = seconds;

    public AbsoluteTime(in DateTime dt) : this(
        (dt - Anchor).TotalSeconds
    ) { }

    public static AbsoluteTime Now() => new(DateTime.Now);

    public static AbsoluteTime operator +(
        AbsoluteTime time,
        RelativeTime delta
    ) => new(time.SecondsSinceAnchor + delta.Seconds);

    public static RelativeTime operator -(
        AbsoluteTime time1,
        AbsoluteTime time2
    ) => new(time1.SecondsSinceAnchor - time2.SecondsSinceAnchor);

    public DateTime ToDateTime() {
        return Anchor + TimeSpan.FromSeconds(SecondsSinceAnchor);
    }

    public override string ToString() => $"{SecondsSinceAnchor:0.000}s since 2000";
}

public readonly struct RelativeTime {
    public double Seconds { get; }

    public RelativeTime(double seconds) {
        Seconds = seconds;
    }

    public static RelativeTime operator +(
        RelativeTime d1,
        RelativeTime d2
    ) => new(d1.Seconds + d2.Seconds);

    public override string ToString() => $"{Seconds:0.000}s";
}
