namespace Core;

public sealed class DisposableStack() : IDisposable {
    readonly Stack<Action> disposers = [];
    bool hasDisposed = false;

    public void Use(IDisposable? disposable) {
        ObjectDisposedException.ThrowIf(hasDisposed, this);
        if (disposable is not null) {
            disposers.Push(disposable.Dispose);
        }
    }

    public void Defer(Action action) {
        ObjectDisposedException.ThrowIf(hasDisposed, this);
        disposers.Push(action);
    }

    public void Dispose() {
        if (!hasDisposed) {
            while (disposers.TryPop(out var disposer)) {
                disposer();
            }
            hasDisposed = true;
        }
    }
}
