﻿namespace D20Tek.Functional;

public sealed class None<T> : Option<T>
    where T : notnull
{
    internal None()
    {
    }

    public override TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone) =>
        onNone();
}
