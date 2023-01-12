﻿namespace ProjectX.Core.Threading;

public readonly struct UpgradeableReadLock : IDisposable
{
    private readonly ReaderWriterLockSlim _readerWriterLockSlim;

    public UpgradeableReadLock(ReaderWriterLockSlim readerWriterLockSlim)
    {
        _readerWriterLockSlim = readerWriterLockSlim;
        _readerWriterLockSlim.EnterUpgradeableReadLock();
    }

    public void Dispose()
    {
        _readerWriterLockSlim.ExitUpgradeableReadLock();
    }
}