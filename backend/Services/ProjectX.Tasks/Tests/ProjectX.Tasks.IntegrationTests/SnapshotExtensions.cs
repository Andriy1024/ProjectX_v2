using FluentAssertions.Collections;
using FluentAssertions.Primitives;
using Snapshooter;
using Snapshooter.Xunit;
using System;

namespace ProjectX.Tasks.IntegrationTests;

public static class SnapshotExtensions
{
    public static void MatchSnapshot(
        this ObjectAssertions result,
        string? snapshotName = null,
        Func<MatchOptions, MatchOptions>? matchOptions = null)
    {
        result.Subject.MatchSnapshot(SnapshotNameExtension.Create(snapshotName), matchOptions);
    }

    public static void MatchSnapshot<T>(
        this GenericCollectionAssertions<T> result,
        string? snapshotName = null,
        Func<MatchOptions, MatchOptions>? matchOptions = null) 
    {
        result.Subject.MatchSnapshot(SnapshotNameExtension.Create(snapshotName), matchOptions);
    }
}
